using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetAmmo : MonoBehaviour {

	[SerializeField]
	private PlayerWeaponBehavior weaponBehavior;
	[SerializeField]
	private Text text;

	private void Awake()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		weaponBehavior = player.GetComponent<PlayerWeaponBehavior>();
		text = GetComponent<Text>();
	}

	private void Update()
	{
		string magasineSize = weaponBehavior.playerWeapon[weaponBehavior.currentWeaponIndex].magasinSize.ToString();
		string currentAmmo = weaponBehavior.playerWeapon[weaponBehavior.currentWeaponIndex].ammo.ToString();
		text.text = magasineSize + " / " + currentAmmo;
	}
}
