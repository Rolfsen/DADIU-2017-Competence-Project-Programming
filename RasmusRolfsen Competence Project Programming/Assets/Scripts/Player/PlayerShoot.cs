using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{

	public enum weapons { gun, uzi, assaultRifle, RocketLauncher };
	[SerializeField]
	private bool[] unlocked;
	[SerializeField]
	private float[] ammo;

	private void Awake()
	{

	}
}
