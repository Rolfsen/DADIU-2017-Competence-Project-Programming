using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	public float maxHealth;
	public float currentHealth;

	[SerializeField]
	private string HealthEvent;


	private void Awake()
	{
		currentHealth = maxHealth;
		EventManager.StartListening(HealthEvent, ChangeHealth);
	}

	private void ChangeHealth(object healthChange, object none)
	{
		currentHealth += (float) healthChange;
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