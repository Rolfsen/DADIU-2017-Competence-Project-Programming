using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExampleTrigger : MonoBehaviour
{

	[SerializeField]
	Vector2 message;

	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			EventManager2.TriggerEvent("DebugEvent", message);
		}
	}
}
