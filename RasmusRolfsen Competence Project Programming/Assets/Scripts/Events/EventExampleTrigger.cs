using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExampleTrigger : MonoBehaviour
{

	[SerializeField]
	private string message;

	[SerializeField]
	private int swag;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			EventManager.TriggerEvent("DebugEvent", message, swag);
		}
	}
}
