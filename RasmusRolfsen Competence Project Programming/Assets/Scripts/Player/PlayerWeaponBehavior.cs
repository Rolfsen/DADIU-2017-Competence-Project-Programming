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
	public float reloadTime;
	public float attackSpeed;
}

public class PlayerWeaponBehavior : MonoBehaviour
{
	// WeaponsCount
	private const int WeaponCount = 5;

	// move to struct.
	[SerializeField]
	private Weapons[] PlayerWeapons = new Weapons[WeaponCount];
	[SerializeField]
	private KeyCode[] weaponKeys = new KeyCode[WeaponCount] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
	[SerializeField] // Unlocked Flags
	private bool[] unlocked = new bool[WeaponCount];

	[SerializeField]
	private float changeWeaponTime = 3f;
	[SerializeField]
	private float shotDist = 1000;

	// rename name + currentWeaponIndex + isCooldown?
	private bool changingWeapon = false;
	private int currentWeapon = 0;
	private bool shootCooldown = false;



	private void Awake()
	{
		EventManager.StartListening("WeaponUnlock", UnlockWeapon);

		currentWeapon = 0;
		shootCooldown = false;
		changingWeapon = false;

	}


	private void UnlockWeapon(object weaponIndex)
	{
		unlocked[(int)weaponIndex] = true;
	}
	private void LockWeapon(object weaponIndex)
	{
		unlocked[(int)weaponIndex] = false;
	}

	private void Update()
	{
		if (Input.anyKey)
		{
			if (Input.GetMouseButton(0) && PlayerWeapons[currentWeapon].ammo > 0 && !shootCooldown)
			{
				StartCoroutine(Shoot());
			}

			if (!changingWeapon)
			{
				for (int i = 0; i < weaponKeys.Length; i++)
				{
					if (Input.GetKeyDown(weaponKeys[i]) && unlocked[i])
					{
						StartCoroutine(ChangeWeapon(i));
						break;
					}
				}
			}
		}
	}

	private void DissArm(object e)
	{
		// sets cooldowns
		shootCooldown = true;
		changingWeapon = true;
		StartCoroutine(ReArm((float)e));
	}

	IEnumerator ChangeWeapon(int newWeapon)
	{
		changingWeapon = true;
		shootCooldown = true;
		yield return new WaitForSeconds(changeWeaponTime);
		currentWeapon = newWeapon;
		shootCooldown = false;
		changingWeapon = false;
		Debug.Log("ChangeWeapon");
	}

	IEnumerator Shoot()
	{
		Vector3 pScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pScreenPos;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, shotDist))
		{
			Debug.Log(hit.transform.gameObject);
		}
		Debug.Log("Shoot");
		shootCooldown = true;
		if (currentWeapon != 0)
		{
			PlayerWeapons[currentWeapon].ammo--;
		}
		yield return new WaitForSeconds(PlayerWeapons[currentWeapon].attackSpeed);
		shootCooldown = false;
	}

	IEnumerator Reload()
	{
		yield return new WaitForSeconds(PlayerWeapons[currentWeapon].reloadTime);
		Debug.Log("Reload");
	}

	IEnumerator ReArm(float time)
	{
		yield return new WaitForSeconds(time);
		shootCooldown = false;
		changingWeapon = false;
	}
}