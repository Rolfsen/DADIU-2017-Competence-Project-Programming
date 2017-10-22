using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
	[SerializeField]
	private float thresholdMax = -0.3f;
	[SerializeField]
	private float thresholdMin = -0.6f;



	private void OnCollisionStay(Collision collission)
	{

		switch (collission.gameObject.tag)
		{
			case "Ground":
				GroundCollision(collission);
				break;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "Enemy":
				EnemyCollision(other);
				break;
			default:
				break;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.gameObject.tag)
		{
			case "ZoneSpawner":
				SpawnZoneCollision(other.transform);
				break;
			default:
				break;
		}
	}

	private void SpawnZoneCollision(Transform other)
	{
		EventManager.TriggerEvent("SpawnZone", other.transform, null);
		Destroy(other.gameObject);
	}



	private void GroundCollision(Collision collission)
	{
		if (GetComponent<PlayerMovement>().currentState == PlayerMovement.PlayerState.airMovement)
		{
			foreach (ContactPoint contact in collission.contacts)
			{
				Vector3 dir = collission.contacts[0].point - transform.position;

				if (dir.y > thresholdMin && dir.y < thresholdMax)
				{
					EventManager.TriggerEvent("GroundCollision", null, null);
					break;
				}
			}
		}
	}

	private void EnemyCollision(Collider collider)
	{
		EventManager.TriggerEvent("DamagePlayer",5,null);
	}

}
