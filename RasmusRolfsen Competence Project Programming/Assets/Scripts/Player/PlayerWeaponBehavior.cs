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
	private float[] reloadTime;
	[SerializeField]
	float[] timeBetweenShots;
	[SerializeField]
	private int weaponCount;
	private int currentWeapon;
	

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
			if (Input.GetMouseButton(1))
			{
				StartCoroutine(Shoot());
			}
		}
	}

	IEnumerator Shoot()
	{

		yield return new WaitForSeconds(timeBetweenShots[currentWeapon]);
	}

	IEnumerator Reload()
	{
		yield return new WaitForSeconds(reloadTime[currentWeapon]);
	}
}
