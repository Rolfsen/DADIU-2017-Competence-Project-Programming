using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

	[SerializeField]
	private float trapDamage;
	[SerializeField]
	private int particleAmount;
	[SerializeField]
	private string particleEvent = "";

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Player")
		{
			EventManager.TriggerEvent("PlayerHealth", -trapDamage,null);
			EventManager.TriggerEvent(particleEvent, transform.position, particleAmount);
			Destroy(gameObject);
		}
	}
}
