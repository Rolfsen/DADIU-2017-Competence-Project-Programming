using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	[SerializeField]
	private string HealthEvent;
	[SerializeField]
	private int maxHealth;
	private int currentHealth;


	private void Awake()
	{
		currentHealth = maxHealth;
		EventManager.StartListening(HealthEvent, ChangeHealth);
	}

	private void ChangeHealth(object healthChange, object none)
	{
		currentHealth += (int) healthChange;
		Debug.Log(currentHealth);
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
		else if (currentHealth <= 0)
		{
			EventManager.TriggerEvent("PlayerDied",0,0);			
		}
	}
}
