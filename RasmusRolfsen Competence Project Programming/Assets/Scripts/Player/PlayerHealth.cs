using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{

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
		try
		{
			currentHealth += (float)healthChange;
			if (currentHealth > maxHealth)
			{
				currentHealth = maxHealth;
			}
			else if (currentHealth <= 0)
			{
				EventManager.TriggerEvent("PlayerDied", 0, 0);
			}

		}
		catch
		{
			try
			{
				int tmpVar = (int)healthChange;
				float convertedVar = (float)tmpVar;
				currentHealth += (float)convertedVar;
				if (currentHealth > maxHealth)
				{
					currentHealth = maxHealth;
				}
				else if (currentHealth <= 0)
				{
					EventManager.TriggerEvent("PlayerDied", 0, 0);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e);
			}
		}

	}
}