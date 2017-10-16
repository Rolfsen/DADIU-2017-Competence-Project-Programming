using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponBehavior : MonoBehaviour
{

	public enum weapons { gun, uzi, riffle, assaultRifle, rocketLauncher };
	[SerializeField, Tooltip ("Checks if a weapon is unlocked")]
	private bool[] unlocked;
	[SerializeField]
	private float[] ammo;
	[SerializeField]
	private float reloadTime = 2.5f; 

	private void Awake()
	{
	}

	private void UnlockWeapon()
	{

	}
	IEnumerator Reload ()
	{
		yield return new WaitForSeconds(reloadTime);
	}
}
