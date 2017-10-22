﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCustomEvent : MonoBehaviour
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
			EventManager.TriggerEvent(eventname, variable1, variable2);
			Destroy(gameObject);
		}
	}
}