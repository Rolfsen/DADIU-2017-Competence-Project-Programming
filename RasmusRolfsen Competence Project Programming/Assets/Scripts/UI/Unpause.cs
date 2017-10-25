using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unpause : MonoBehaviour {
	[SerializeField]
	private GameObject player = null;
	[SerializeField]
	private GameObject canvas = null;
	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public void UnpauseGame()
	{
		player.SetActive(true);
		canvas.SetActive(false);
		Time.timeScale = 1;
	}
}
