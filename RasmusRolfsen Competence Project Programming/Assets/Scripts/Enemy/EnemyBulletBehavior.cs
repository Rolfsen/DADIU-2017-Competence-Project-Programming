using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehavior : MonoBehaviour
{

	public int damage;
	public float moveSpeed;
	public Vector3 moveDir;

	private void Update()
	{
		transform.Translate(moveDir*moveSpeed*Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other)
	{
		switch (other.tag)
		{
			case ("Player"):
				break;
			case ("Ground"):
				break;
			default:
				break;
		}
	}
}