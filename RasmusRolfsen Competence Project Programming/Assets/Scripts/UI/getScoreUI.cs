using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class getScoreUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int gainedScore = PlayerPrefs.GetInt("GameScore");

		GetComponent<Text>().text = GetComponent<Text>().text + " " + gainedScore;		
	}
}
