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
		EventManager2.StopListening("DebugEvent", DebugEvent);
	}

	// Enable the event
	private void OnEnable()
	{
		EventManager2.StartListening("DebugEvent", DebugEvent);
	}

	// Disable the event
	private void OnDisable()
	{
		EventManager2.StopListening("DebugEvent", DebugEvent);
	}

	// Function To Be Executed
	void DebugEvent(object e)
	{
		Debug.Log(e);
	}
}
