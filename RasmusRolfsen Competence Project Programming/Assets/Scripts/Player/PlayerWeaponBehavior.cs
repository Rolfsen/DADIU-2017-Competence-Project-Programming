using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Weapons
{
	public string weaponName;
	public int ammo;
	public int reserveAmmo;
	public int magasinSize;
	public int weaponDamage;
	public float reloadTime;
	public float attackSpeed;
	public bool isUnlocked;
	public KeyCode changeWeaponKey;
}

public class PlayerWeaponBehavior : MonoBehaviour
{
	private const int WeaponCount = 5;

	[SerializeField]
	private Weapons[] PlayerWeapons = new Weapons[WeaponCount];

	[SerializeField]
	private KeyCode reloadKey = KeyCode.R;

	[SerializeField]
	private float changeWeaponTime = 3f;
	[SerializeField]
	private float shotDist = 1000;

	// rename name + currentWeaponIndex + isCooldown?
	private bool changingWeapon = false;
	private int currentWeaponIndex = 0;
	private bool isShootCooldown = false;
	private bool isBlocking = false;
	private bool isReloading = false;
	private LineRenderer line = null;


	private void Awake()
	{
		EventManager.StartListening("WeaponUnlock", WeaponLockState);
		EventManager.StartListening("PlayerBlockState", PlayerBlocking);

		currentWeaponIndex = 0;
		isShootCooldown = false;
		changingWeapon = false;
		isReloading = false;
		line = GetComponent<LineRenderer>();
	}

	private void PlayerBlocking(object blockState, object none)
	{
		isBlocking = (bool)blockState;
	}


	private void WeaponLockState(object weaponIndex, object newState)
	{
		if ((bool)newState)
		{
			PlayerWeapons[currentWeaponIndex].isUnlocked = true;
		}
		else
		{
			PlayerWeapons[currentWeaponIndex].isUnlocked = false;
		}

	}

	private void Update()
	{
		if (Input.GetKeyDown(reloadKey))
		{
			StartCoroutine(Reload());
		}

		if (Input.anyKey && !isBlocking && !isReloading)
		{
			if (Input.GetMouseButton(0) && PlayerWeapons[currentWeaponIndex].ammo > 0 && !isShootCooldown)
			{
				StartCoroutine(Shoot());
			}
			else if (Input.GetMouseButton(0) && PlayerWeapons[currentWeaponIndex].ammo < 1 && !isShootCooldown)
			{
				StartCoroutine(Reload());
			}

			if (!changingWeapon)
			{
				for (int i = 0; i < PlayerWeapons.Length; i++)
				{
					if (Input.GetKeyDown(PlayerWeapons[i].changeWeaponKey) && PlayerWeapons[i].isUnlocked)
					{
						StartCoroutine(ChangeWeapon(i));
						break;
					}
				}
			}
		}
	}

	private void DisableAttack(object time, object none)
	{
		// sets cooldowns
		isShootCooldown = true;
		changingWeapon = true;
		StartCoroutine(EnableAttack((float)time));
	}

	IEnumerator ChangeWeapon(int newWeapon)
	{
		changingWeapon = true;
		isShootCooldown = true;
		yield return new WaitForSeconds(changeWeaponTime);
		currentWeaponIndex = newWeapon;
		isShootCooldown = false;
		changingWeapon = false;
	}

	IEnumerator Shoot()
	{
		Vector3 pScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pScreenPos;
		dir.z = 0;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;

		line.SetPosition(0, transform.position);
		if (Physics.Raycast(ray, out hit, shotDist))
		{
			switch (hit.transform.gameObject.tag)
			{
				case "Enemy":
					EnemyHit(hit.transform.gameObject);
					break;
				case "Ground":
					GroundHit(hit);
					break;
			}
			line.SetPosition(1, hit.point);
		}
		else
		{
			line.SetPosition(1, dir);
		}

		StartCoroutine(LineLifeTime());


		isShootCooldown = true;
		PlayerWeapons[currentWeaponIndex].ammo--;
		if (PlayerWeapons[currentWeaponIndex].ammo < 1)
		{
			StartCoroutine(Reload());
		}
		else
		{
			yield return new WaitForSeconds(PlayerWeapons[currentWeaponIndex].attackSpeed);
		}

		isShootCooldown = false;
	}

	private void EnemyHit(GameObject hit)
	{
		hit.GetComponent<EnemyHealth>().LoseHealth(PlayerWeapons[currentWeaponIndex].weaponDamage);
	}

	private void GroundHit(RaycastHit hit)
	{
		EventManager.TriggerEvent("SpawnDust", hit.point, 15);
	}

	IEnumerator LineLifeTime()
	{
		line.enabled = true;
		yield return new WaitForSeconds(0.01f);
		line.enabled = false;
	}

	IEnumerator Reload()
	{
		isReloading = true;
		if (currentWeaponIndex != 0)
		{
			if (PlayerWeapons[currentWeaponIndex].reserveAmmo < 1)
			{
				isReloading = false;
				StartCoroutine(ChangeWeapon(0));
				StopCoroutine(Reload());
			}
			else if (PlayerWeapons[currentWeaponIndex].reserveAmmo >= PlayerWeapons[currentWeaponIndex].magasinSize)
			{
				PlayerWeapons[currentWeaponIndex].ammo = PlayerWeapons[currentWeaponIndex].magasinSize;
				PlayerWeapons[currentWeaponIndex].reserveAmmo -= PlayerWeapons[currentWeaponIndex].magasinSize;
			}
			else
			{
				PlayerWeapons[currentWeaponIndex].ammo = PlayerWeapons[currentWeaponIndex].reserveAmmo;
				PlayerWeapons[currentWeaponIndex].reserveAmmo = 0;
			}
		}
		else
		{
			PlayerWeapons[currentWeaponIndex].ammo = PlayerWeapons[currentWeaponIndex].magasinSize;
		}
		yield return new WaitForSeconds(PlayerWeapons[currentWeaponIndex].reloadTime);
		isReloading = false;
	}

	IEnumerator EnableAttack(float time)
	{
		yield return new WaitForSeconds(time);
		isShootCooldown = false;
		changingWeapon = false;
	}
}