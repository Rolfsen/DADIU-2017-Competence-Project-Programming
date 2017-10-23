using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildTriggers : MonoBehaviour {

	[SerializeField]
	EnemyStates.enemyState enterState;

	[SerializeField]
	EnemyStates.enemyState exitState;

	[SerializeField]
	private int parentColorIndex;

	EnemyStates parentState;
	private Color previousColor;

	private void Start()
	{
		parentState = GetComponentInParent<EnemyStates>();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			parentState.objectState = enterState;
			previousColor = GetComponentInParent<Renderer>().material.color;
			GetComponentInParent<Renderer>().material.color = GetComponentInParent<EnemyStates>().unitColors[parentColorIndex];
		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			GetComponentInParent<Renderer>().material.color = previousColor;
			parentState.objectState = exitState;			
		}
	}
}