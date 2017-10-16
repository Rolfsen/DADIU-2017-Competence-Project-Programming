using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponBehavior : MonoBehaviour
{

	private enum weapons { gun, uzi, riffle, assaultRifle, rocketLauncher };
	[SerializeField]
	private bool[] unlocked;
	[SerializeField]
	private float[] ammo;
	[SerializeField]
	private float[] reserveAmmo;
	[SerializeField]
	private float[] magasinSize;
	[SerializeField]
	private float[] reloadTime;
	[SerializeField]
	private float[] timeBetweenShots;
	[SerializeField]
	private KeyCode[] WeaponKeys;
	[SerializeField]
	private int weaponCount;
	[SerializeField]
	private float changeWeaponTime = 3f;
	private bool changingWeapon = false;

	[SerializeField]
	private int currentWeapon;
	private bool shootCooldown;


	private void Awake()
	{
		EventManager.StartListening("WeaponUnlock", UnlockWeapon);
	}

	private void Start()
	{
		unlocked = new bool[weaponCount];
		unlocked[0] = true;

		ammo = new float[weaponCount];
		reloadTime = new float[weaponCount];
		timeBetweenShots = new float[weaponCount];
		reserveAmmo = new float[weaponCount];
		magasinSize = new float[weaponCount];
		WeaponKeys = new KeyCode[weaponCount];

		currentWeapon = 0;
		shootCooldown = false;

		changingWeapon = false;
	}

	private void UnlockWeapon(object e)
	{
		unlocked[(int)e] = true;
	}
	private void LockWeapon(object e)
	{
		unlocked[(int)e] = false;
	}

	private void Update()
	{
		if (Input.anyKey)
		{
			if (Input.GetMouseButton(1) && ammo[currentWeapon] != 0 && !shootCooldown)
			{
				StartCoroutine(Shoot());
			}

			if (!changingWeapon)
			{
				if (Input.GetKeyDown(KeyCode.Alpha1) && unlocked[0])
				{
					StartCoroutine(ChangeWeapon(0));
				}
				else if (Input.GetKeyDown(KeyCode.Alpha2) && unlocked[1])
				{
					StartCoroutine(ChangeWeapon(1));
				}
				else if (Input.GetKeyDown(KeyCode.Alpha3) && unlocked[2])
				{
					StartCoroutine(ChangeWeapon(2));
				}
				else if (Input.GetKeyDown(KeyCode.Alpha4) && unlocked[3])
				{
					StartCoroutine(ChangeWeapon(3));
				}
			}
		}
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
		shootCooldown = true;
		if (currentWeapon != 0)
		{
			ammo[currentWeapon]--;
		}
		yield return new WaitForSeconds(timeBetweenShots[currentWeapon]);
		shootCooldown = false;
		Debug.Log("Shoot");
	}

	IEnumerator Reload()
	{
		yield return new WaitForSeconds(reloadTime[currentWeapon]);
		Debug.Log("Reload");
	}
}