using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SpawnableZones
{
	public GameObject level;
	public float difficulty;
}

public class ZoneHandler : MonoBehaviour
{

	[SerializeField]
	private List<GameObject> CurrentZones;

	[SerializeField]
	private AnimationCurve increaseDifficulty;

	[SerializeField]
	private float startDifficulty;

	[SerializeField]
	private List<SpawnableZones> zones;


	private void Awake()
	{
		EventManager.StartListening("SpawnZone", SpawnNewZone);
	}

	private void SpawnNewZone(object triggerPosition, object none)
	{

		int nextZone = Random.Range(0, zones.Count);

		GameObject newZone = zones[nextZone].level;

		Vector3 spawnPosition = CurrentZones[1].GetComponent<ZoneKeyElements>().levelEnd.transform.position - CurrentZones[0].GetComponent<ZoneKeyElements>().levelStart.transform.localPosition; // change this
		Quaternion spawnRotation = transform.rotation;

		GameObject spawnedLevel = Instantiate(newZone,spawnPosition,spawnRotation);


		CurrentZones.Insert(0, newZone);

		CurrentZones[1].GetComponent<ZoneKeyElements>().levelBlocker.SetActive(true);

		if (CurrentZones.Count == 3)
		{
			var tmp = CurrentZones[1];
			CurrentZones.RemoveAt(1);
			DestroyImmediate(tmp,true);
		}

		

	}
}
