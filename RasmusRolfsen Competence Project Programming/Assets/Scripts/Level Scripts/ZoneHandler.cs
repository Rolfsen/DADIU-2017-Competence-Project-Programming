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
	private List<GameObject> CurrentZones = null;
	[SerializeField]
	private List<SpawnableZones> zones = null;

	private void Awake()
	{
		EventManager.StartListening("SpawnZone", SpawnNewZone);
	}

	private void SpawnNewZone(object triggerPosition, object none)
	{
		int nextZone = Random.Range(0, zones.Count);
		GameObject newZone = zones[nextZone].level;
		GameObject spawnedLevel = Instantiate(newZone);
		CurrentZones.Insert(0, spawnedLevel);
		Vector3 spawnPosition = CurrentZones[1].GetComponent<ZoneKeyElements>().levelEnd.transform.position - CurrentZones[0].GetComponent<ZoneKeyElements>().levelStart.transform.localPosition; 
		spawnedLevel.transform.position = spawnPosition;
		CurrentZones[1].GetComponent<ZoneKeyElements>().levelBlocker.SetActive(true);
		if (CurrentZones.Count == 3)
		{
			var tmp = CurrentZones[2];
			CurrentZones.RemoveAt(2);
			Destroy(tmp);
		}
	}
}
