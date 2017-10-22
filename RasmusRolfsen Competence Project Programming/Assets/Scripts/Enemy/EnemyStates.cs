using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EnemyAttackTypes
{
	public float attackSpeed;
	public float bulletSpeed;
	public int damage;
}

public class EnemyStates : MonoBehaviour
{


	public enum enemyState { idle, patrol, notice, attack, chase, returnToPosition };

	[SerializeField]
	public enemyState state;

	private Transform player;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	// Update is called once per frame
	void Update()
	{

		switch (state)
		{
			case (enemyState.idle):
				IdleBehavior();
				break;
			case (enemyState.attack):
				AttackBehavior();
				break;
			case (enemyState.notice):
				NoticeBehavior();
				break;
		}
	}

	private void IdleBehavior()
	{
		//
	}

	private void AttackBehavior()
	{
		//
	}

	private void NoticeBehavior()
	{
		//
	}
}
