using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggers : MonoBehaviour {

	[SerializeField]
	EnemyStates.enemyState enterState;

	[SerializeField]
	EnemyStates.enemyState exitState;

	EnemyStates parentState;

	private void Start()
	{
		parentState = GetComponentInParent<EnemyStates>();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			parentState.objectState = enterState;
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			parentState.objectState = exitState;			
		}
	}
}