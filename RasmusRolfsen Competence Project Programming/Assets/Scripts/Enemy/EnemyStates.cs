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
	public Color bulletColor;
}

public class EnemyStates : MonoBehaviour
{

	public Color[] unitColors = new Color[3]; 

	public enum enemyState { idle, patrol, notice, attack, chase, returnToPosition };

	[SerializeField]
	public enemyState objectState;

	private Transform player;

	private bool isAttackReady = true;

	[SerializeField]
	private List<EnemyAttackTypes> enemyAttacks = null;



	private void Awake()
	{
		isAttackReady = true;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		GetComponent<Renderer>().material.color = unitColors[0];
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
		if (isAttackReady)
		{
			Vector3 spawnPosition = transform.position;
			Quaternion spawnRotation = transform.rotation;

			int getAttackSeed = Random.Range(0, enemyAttacks.Count);

			GameObject bullet = Instantiate(enemyAttacks[getAttackSeed].bulletType, spawnPosition, spawnRotation);
			EnemyBulletBehavior bulletBehavior = bullet.GetComponent<EnemyBulletBehavior>();


			bulletBehavior.moveDir = player.transform.position - spawnPosition;
			bulletBehavior.moveSpeed = enemyAttacks[getAttackSeed].bulletSpeed;
			bulletBehavior.damage = enemyAttacks[getAttackSeed].damage;
			bullet.GetComponent<Renderer>().material.color = enemyAttacks[getAttackSeed].bulletColor;

			isAttackReady = false;
			StartCoroutine(Cooldown(enemyAttacks[getAttackSeed].attackSpeed));
		}
	}

	private void NoticeBehavior()
	{
		//
	}

	IEnumerator Cooldown (float cooldownTime)
	{
		yield return new WaitForSeconds(cooldownTime);
		isAttackReady = true;
	}
}
