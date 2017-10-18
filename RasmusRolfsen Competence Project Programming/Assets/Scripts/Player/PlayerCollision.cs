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

	private void GroundCollision(Collision collission)
	{
		bool check = false;

		if (GetComponent<PlayerMovement1>().currentState == PlayerMovement1.PlayerState.jumping)
		{
			foreach (ContactPoint contact in collission.contacts)
			{
				Vector3 dir = collission.contacts[0].point - transform.position;

				if (dir.y > thresholdMin && dir.y < thresholdMax)
				{
					check = true;
					break;
				}
			}
			if (check == true)
			{
				EventManager.TriggerEvent("GroundCollision", null);
			}
		}

	}

}
