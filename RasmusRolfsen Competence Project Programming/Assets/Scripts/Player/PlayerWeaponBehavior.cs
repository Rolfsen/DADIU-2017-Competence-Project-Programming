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

	public Weapons[] playerWeapon = new Weapons[WeaponCount];
	public int currentWeaponIndex = 0;
	[SerializeField]
	private float changeWeaponTime = 3f;
	[SerializeField]
	private float shotDist = 1000;
	[SerializeField]
	private KeyCode reloadKey = KeyCode.R;

	private bool changingWeapon = false;
	private bool isShootCooldown = false;
	private bool isBlocking = false;
	private bool isReloading = false;
	private LineRenderer line = null;


	private void Awake()
	{
		EventManager.StartListening("WeaponUnlock", WeaponLockState);
		EventManager.StartListening("PlayerBlockState", PlayerBlocking);
		EventManager.StartListening("AddAmmo",GainAmmo);

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
			playerWeapon[currentWeaponIndex].isUnlocked = true;
		}
		else
		{
			playerWeapon[currentWeaponIndex].isUnlocked = false;
		}

	}

	private void GainAmmo (object weaponIndex ,object amount)
	{
		int weaponInd = (int)weaponIndex;
		int addedAmmo = (int)amount;
		playerWeapon[weaponInd].reserveAmmo += addedAmmo;
	}

	private void Update()
	{
		if (Input.GetKeyDown(reloadKey))
		{
			StartCoroutine(Reload());
		}

		if (Input.anyKey && !isBlocking && !isReloading)
		{
			if (Input.GetMouseButton(0) && playerWeapon[currentWeaponIndex].ammo > 0 && !isShootCooldown)
			{
				StartCoroutine(Shoot());
			}
			else if (Input.GetMouseButton(0) && playerWeapon[currentWeaponIndex].ammo < 1 && !isShootCooldown)
			{
				StartCoroutine(Reload());
			}

			if (!changingWeapon)
			{
				for (int i = 0; i < playerWeapon.Length; i++)
				{
					if (Input.GetKeyDown(playerWeapon[i].changeWeaponKey) && playerWeapon[i].isUnlocked)
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
			line.SetPosition(1, transform.position + dir*shotDist);
		}

		StartCoroutine(LineLifeTime());


		isShootCooldown = true;
		playerWeapon[currentWeaponIndex].ammo--;
		if (playerWeapon[currentWeaponIndex].ammo < 1)
		{
			StartCoroutine(Reload());
		}
		else
		{
			yield return new WaitForSeconds(playerWeapon[currentWeaponIndex].attackSpeed);
		}

		isShootCooldown = false;
	}

	private void EnemyHit(GameObject hit)
	{
		hit.GetComponent<EnemyHealth>().LoseHealth(playerWeapon[currentWeaponIndex].weaponDamage);
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
			if (playerWeapon[currentWeaponIndex].reserveAmmo < 1)
			{
				isReloading = false;
				StartCoroutine(ChangeWeapon(0));
				StopCoroutine(Reload());
			}
			else if (playerWeapon[currentWeaponIndex].reserveAmmo >= playerWeapon[currentWeaponIndex].magasinSize)
			{
				playerWeapon[currentWeaponIndex].ammo = playerWeapon[currentWeaponIndex].magasinSize;
				playerWeapon[currentWeaponIndex].reserveAmmo -= playerWeapon[currentWeaponIndex].magasinSize;
			}
			else
			{
				playerWeapon[currentWeaponIndex].ammo = playerWeapon[currentWeaponIndex].reserveAmmo;
				playerWeapon[currentWeaponIndex].reserveAmmo = 0;
			}
		}
		else
		{
			playerWeapon[currentWeaponIndex].ammo = playerWeapon[currentWeaponIndex].magasinSize;
		}
		yield return new WaitForSeconds(playerWeapon[currentWeaponIndex].reloadTime);
		isReloading = false;
	}

	IEnumerator EnableAttack(float time)
	{
		yield return new WaitForSeconds(time);
		isShootCooldown = false;
		changingWeapon = false;
	}
}