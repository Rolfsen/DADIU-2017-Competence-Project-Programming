using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuesDamage : MonoBehaviour {

	[SerializeField]
	private float damageToPlayer;

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject.tag == "Player")
		{
			float damage = damageToPlayer * Time.deltaTime;
			EventManager.TriggerEvent("PlayerHealth", -damage, null);
		}
	}
}
