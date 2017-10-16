using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


[System.Serializable]
public class ThisEvent : UnityEvent<int>
{

}


public class EventManager1 : MonoBehaviour
{

	private Dictionary<string, ThisEvent> eventDictionary;
	private static EventManager1 eventManagerInt;

	public static EventManager1 instance
	{
		get
		{
			if (!eventManagerInt)
			{
				eventManagerInt = FindObjectOfType(typeof(EventManager1)) as EventManager1;

				if (!eventManagerInt)
				{
					Debug.LogError("Put an active Event Manager into the scene");
				}
				else
				{
					eventManagerInt.Init();
				}
			}
			return eventManagerInt;
		}
	}

	void Init()
	{
		if (eventDictionary == null)
		{
			eventDictionary = new Dictionary<string, ThisEvent>();
		}
	}

	public static void StartListening(string eventName, UnityAction<int> listener)
	{
		ThisEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.AddListener(listener);
		}
		else
		{
			thisEvent = new ThisEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction<int> listener)
	{
		if (eventManagerInt == null) return;

		ThisEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.RemoveListener(listener);
		}
	}

	public static void RemoveAllListerners()
	{
		instance.eventDictionary.Clear();
	}

	public static void TriggerEvent(string eventName, int value)
	{
		ThisEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent))
		{
			thisEvent.Invoke(value);
		}
	}
}