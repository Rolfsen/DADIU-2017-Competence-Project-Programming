using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {




	[SerializeField]
	private int particleAmount = 50;
	[SerializeField]
	private int maxHealth = 100;
	[SerializeField]
	private int killScore = 40;
	private int currentHealth = 100;

	private ParticleSystem blood = null;

	// Use this for initialization
	void Awake () {
		currentHealth = maxHealth;
		blood = GetComponent<ParticleSystem>();
		blood.Stop();
	}

	public void LoseHealth (int amount)
	{
		blood.Emit(particleAmount);
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			Dead();
		}
	}

	private void Dead()
	{
		Destroy(gameObject);
	}

	private void OnDestroy()
	{
		EventManager.TriggerEvent("SpawnBlood", transform.position);
		EventManager.TriggerEvent("ScoreChange", killScore);
	}
}
