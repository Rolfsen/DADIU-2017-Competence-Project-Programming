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
	private const int weaponCount = 5;

	[SerializeField]
	private bool[] unlocked = new bool[weaponCount];
	[SerializeField]
	private KeyCode[] weaponKeys = new KeyCode[weaponCount] { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5 };

	[SerializeField]
	private float changeWeaponTime = 3f;
	private bool changingWeapon = false;

	[SerializeField]
	private int currentWeapon;
	private bool shootCooldown;

	[SerializeField]
	private Weapons[] PlayerWeapons = new Weapons[weaponCount];


	private void Awake()
	{
		EventManager.StartListening("WeaponUnlock", UnlockWeapon);

		Weapons Gun = new Weapons();
		Gun.ammo = 5;
		Debug.Log(Gun.ammo);
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
			if (Input.GetMouseButton(1) && PlayerWeapons[currentWeapon].ammo != 0 && !shootCooldown)
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
		Vector3 pScreenPos = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 dir = Input.mousePosition - pScreenPos;
		Debug.DrawRay(transform.position, dir, Color.blue);
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 400))
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