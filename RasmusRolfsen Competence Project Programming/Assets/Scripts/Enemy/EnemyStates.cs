using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyAttackTypes
{
	public float attackSpeed;
	public float bulletSpeed;
	public int damage;
	public GameObject bulletType;
}

public class EnemyStates : MonoBehaviour
{


	public enum enemyState { idle, patrol, notice, attack, chase, returnToPosition };

	[SerializeField]
	public enemyState objectState;

	private Transform player;

	private bool isAttackCoold;

	[SerializeField]
	private List<EnemyAttackTypes> enemyAttacks;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}


	// Update is called once per frame
	void Update()
	{

		switch (objectState)
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
		Vector3 spawnPosition = transform.position;
		Quaternion spawnRotation = transform.rotation;

		int getAttackSeed = Random.Range(0,enemyAttacks.Count-1);

		GameObject bullet = Instantiate(enemyAttacks[getAttackSeed].bulletType, spawnPosition, spawnRotation);
		EnemyBulletBehavior bulletBehavior = bullet.GetComponent<EnemyBulletBehavior>();


		bulletBehavior.moveDir = player.transform.position - spawnPosition;
		bulletBehavior.moveSpeed = enemyAttacks[getAttackSeed].bulletSpeed;
		bulletBehavior.damage = enemyAttacks[getAttackSeed].damage;
	}

	private void NoticeBehavior()
	{
		//
	}
}
