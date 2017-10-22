using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{

	[SerializeField]
	private int variable1 = 0;

	[SerializeField]
	private int variable2 = 5;

	[SerializeField]
	private string eventname;


	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			Debug.Log(other.gameObject.tag);
			EventManager.TriggerEvent(eventname, variable1, variable2);
			Destroy(gameObject);
		}
	}

	private void Awake()
	{
		BoxCollider col = GetComponent<BoxCollider>();
	}
}
