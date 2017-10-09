using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventExampleTrigger : MonoBehaviour
{


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			EventManager.TriggerEvent("DebugEvent");
		}
	}
}
