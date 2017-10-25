using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetHealth : MonoBehaviour {

	[SerializeField]
	private PlayerHealth health;
	[SerializeField]
	private Text text;

	private void Awake()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		health = player.GetComponent<PlayerHealth>();
		text = GetComponent<Text>();
	}

	private void Update()
	{
		text.text = health.maxHealth + " / " + health.currentHealth.ToString();
	}
}
