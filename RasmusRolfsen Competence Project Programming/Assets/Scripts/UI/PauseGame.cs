using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour {

	[SerializeField]
	private GameObject player = null;
	[SerializeField]
	private GameObject pauseCanvas = null;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Time.timeScale = 0;
			pauseCanvas.SetActive(true);
			player.SetActive(false);
		}		
	}
}
