using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHighScore : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("HighScore"))
		{
			GetComponent<Text>().text = "Highscore: " + PlayerPrefs.GetInt("HighScore");
		}
		else
		{
			GetComponent<Text>().text = "";
		}		
	}
}
