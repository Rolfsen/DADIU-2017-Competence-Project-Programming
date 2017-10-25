﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour {

	[SerializeField]
	private string HealthEvent;
	[SerializeField]
	private float maxHealth;
	private float currentHealth;


	private void Awake()
	{
		currentHealth = maxHealth;
		EventManager.StartListening(HealthEvent, ChangeHealth);
	}

	private void ChangeHealth(object healthChange, object none)
	{
		currentHealth += (float) healthChange;
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
