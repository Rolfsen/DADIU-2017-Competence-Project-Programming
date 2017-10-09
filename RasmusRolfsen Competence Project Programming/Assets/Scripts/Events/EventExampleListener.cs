using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventExampleListener : MonoBehaviour
{
	[SerializeField]
	private string logMessage;

	// Remove event when object is destroyed
	private void OnDestroy()
	{
		EventManager.StopListening("DebugEvent", DebugEvent);
	}

	// Enable the event
	private void OnEnable()
	{
		EventManager.StartListening("DebugEvent", DebugEvent);
	}

	// Disable the event
	private void OnDisable()
	{
		EventManager.StopListening("DebugEvent", DebugEvent);
	}

	// Function To Be Executed
	void DebugEvent()
	{
		Debug.Log(logMessage);
	}
}
