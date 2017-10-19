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
	public bool isUnlocked;
	public KeyCode changeWeaponKey;
}

public class PlayerWeaponBehavior : MonoBehaviour
{
	// WeaponsCount
	private const int WeaponCount = 5;

	// move to struct.
	[SerializeField]
	private Weapons[] PlayerWeapons = new Weapons[WeaponCount];
	/*[SerializeField]
	private KeyCode[] weaponKeys = new KeyCode[WeaponCount] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
	[SerializeField] // Unlocked Flags
	private bool[] unlocked = new bool[WeaponCount];*/

	[SerializeField]
	private float changeWeaponTime = 3f;
	[SerializeField]
	private float shotDist = 1000;

	// rename name + currentWeaponIndex + isCooldown?
	private bool changingWeapon = false;
	private int currentWeaponIndex = 0;
	private bool isShootCooldown = false;



	private void Awake()
	{
		EventManager.StartListening("WeaponUnlock", UnlockWeapon);

		currentWeaponIndex = 0;
		isShootCooldown = false;
		changingWeapon = false;

	}


	private void UnlockWeapon(object weaponIndex)
	{
		PlayerWeapons[currentWeaponIndex].isUnlocked = true;
	}
	private void LockWeapon(object weaponIndex)
	{
		PlayerWeapons[currentWeaponIndex].isUnlocked = false;
	}

	private void Update()
	{
		if (Input.anyKey)
		{
			if (Input.GetMouseButton(0) && PlayerWeapons[currentWeaponIndex].ammo > 0 && !isShootCooldown)
			{
				StartCoroutine(Shoot());
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

	private void DissArm(object e)
	{
		// sets cooldowns
		isShootCooldown = true;
		changingWeapon = true;
		StartCoroutine(ReArm((float)e));
	}

	IEnumerator ChangeWeapon(int newWeapon)
	{
		changingWeapon = true;
		isShootCooldown = true;
		yield return new WaitForSeconds(changeWeaponTime);
		currentWeaponIndex = newWeapon;
		isShootCooldown = false;
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
		isShootCooldown = true;
		if (currentWeaponIndex != 0)
		{
			PlayerWeapons[currentWeaponIndex].ammo--;
		}
		yield return new WaitForSeconds(PlayerWeapons[currentWeaponIndex].attackSpeed);
		isShootCooldown = false;
	}

	IEnumerator Reload()
	{
		yield return new WaitForSeconds(PlayerWeapons[currentWeaponIndex].reloadTime);
		Debug.Log("Reload");
	}

	IEnumerator ReArm(float time)
	{
		yield return new WaitForSeconds(time);
		isShootCooldown = false;
		changingWeapon = false;
	}
}