using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.



public class PlayerWeaponBehaviorOLD : MonoBehaviour
{
	private const int weaponCount = 5;
	private enum weapons { gun, uzi, riffle, assaultRifle, rocketLauncher };
	[SerializeField]
	private bool[] unlocked = new bool[weaponCount] { true, false, false, false, false };
	[SerializeField]
	private int[] ammo = new int[weaponCount] { 1, 0, 0, 0, 0 };
	[SerializeField]
	private int[] reserveAmmo = new int[weaponCount] {0,0,0,0,0};
	[SerializeField]
	private int[] magasinSize = new int[weaponCount] { 12, 30, 7, 30, 3 };
	[SerializeField]
	private float[] reloadTime = new float[weaponCount] { 1, 1, 2, 2, 4 };
	[SerializeField]
	private float[] timeBetweenShots = new float[weaponCount] { 0.5f, 0.05f, 1.5f, 0.15f, 5f };
	[SerializeField]
	private KeyCode[] weaponKeys = new KeyCode[weaponCount] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

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

			if (Input.GetKeyDown(KeyCode.Alpha9))
			{
				ResetVals();
			}

			if (!changingWeapon)
			{
				for (int i = 0; i < weaponKeys.Length; i++)
				{
					if (Input.GetKeyDown(weaponKeys[i]) && unlocked[i])
					{
						StartCoroutine(ChangeWeapon(i));
					}
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
		Debug.Log("Shoot");
		shootCooldown = true;
		if (currentWeapon != 0)
		{
			ammo[currentWeapon]--;
		}
		yield return new WaitForSeconds(timeBetweenShots[currentWeapon]);
		shootCooldown = false;
	}

	IEnumerator Reload()
	{
		yield return new WaitForSeconds(reloadTime[currentWeapon]);
		Debug.Log("Reload");
	}

	private void ResetVals()
	{
		unlocked = new bool[weaponCount] { true, false, false, false, false };
		ammo = new int[weaponCount] { 1, 0, 0, 0, 0 };
		reloadTime = new float[weaponCount] { 1, 1, 2, 2, 4 };
		timeBetweenShots = new float[weaponCount] { 0.5f, 0.05f, 1.5f, 0.15f, 5f }; ;
		reserveAmmo = new int[weaponCount] { 0, 0, 0, 0, 0 }; ;
		magasinSize = new int[weaponCount] { 12, 30, 7, 30, 3 };
		weaponKeys = new KeyCode[weaponCount] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };
	}

	private void DissArm(object e)
	{
		StartCoroutine(ReArm((float)e));
	}

	IEnumerator ReArm(float time)
	{
		shootCooldown = true;
		changingWeapon = true;
		yield return new WaitForSeconds(time);
		shootCooldown = false;
		changingWeapon = false;
	}
}