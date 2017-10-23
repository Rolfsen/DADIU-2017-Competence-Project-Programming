using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

	[SerializeField]
	private string newScene;

	public void nextScene()
	{
		EventManager.RemoveAllListerners();
		SceneManager.LoadScene(newScene);
	}
}
