using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour {

	[SerializeField]
	private int score;
	[SerializeField]
	private int currentHighScore;
	[SerializeField]
	private string deathScene;
	[SerializeField]
	private bool pauseGame;

	private void Awake()
	{
		EventManager.StartListening("PlayerDied",PlayerDied);
		EventManager.StartListening("ScoreChange",ScoreChange);
	}


	private void PlayerDied (object none,object none2)
	{
		PlayerPrefs.SetInt("GameScore",score);
		if (PlayerPrefs.HasKey("HighScore"))
		{
			if (PlayerPrefs.GetInt("HighScore") < score)
			{
				PlayerPrefs.SetInt("HighScore", score);
			}
		}
		else
		{
			PlayerPrefs.SetInt("HighScore", score);
		}

		EventManager.RemoveAllListerners();
		SceneManager.LoadScene(deathScene);		
	}

	private void ScoreChange (object change, object none)
	{
		int addedScore = (int)change;
		score += addedScore;
	}
}