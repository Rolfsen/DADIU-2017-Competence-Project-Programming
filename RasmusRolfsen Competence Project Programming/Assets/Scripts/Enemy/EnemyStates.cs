using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct EnemyAttackTypes
{
	public float attackSpeed;
	public float bulletSpeed;
	public int damage;
	public GameObject bulletType;
	public Color bulletColor;
	public List<ParticleEffect> particleEffect;
}

[System.Serializable]
public struct ParticleEffect
{
	public string eventName;
	public int amountOfParticles;
}

[System.Serializable]
public struct PatrolRoute
{
	public List<Transform> patrolRoute;
	public int patrolTarget;
	public float speed;
	public int walkingDir;
	public enum patrolTypes { closedLoop, openLoop };
	public patrolTypes myPatrolType;
}

public class EnemyStates : MonoBehaviour
{
	public Color[] unitColors = new Color[3];
	public enum enemyState { idle, notice, attack };
	public enemyState objectState;
	public enum enemyType { patrol, turrent }

	[SerializeField]
	private enemyType thisType;
	[SerializeField]
	private List<EnemyAttackTypes> enemyAttacks = null;
	[SerializeField]
	private PatrolRoute patrolRoute;
	private Transform player;
	private bool isAttackReady = true;



	private void Awake()
	{
		patrolRoute.walkingDir = -1;
		if (thisType == enemyType.turrent)
		{
			patrolRoute.patrolRoute = null;
		}
		isAttackReady = true;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		GetComponent<Renderer>().material.color = unitColors[0];
	}


	// Update is called once per frame
	void Update()
	{

		GetEnemyState();
		Debug.Log(objectState);

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
		switch (thisType)
		{
			case (enemyType.turrent):
				IdleTurrent();
				break;
			case (enemyType.patrol):
				IdlePatrol();
				break;
			default:
				Debug.LogError("Unimplemented Idle Behaivior");
				break;
		}
	}

	private void IdleTurrent()
	{
		Debug.Log("Idle Turrent");
	}

	private void IdlePatrol()
	{
		if (patrolRoute.patrolRoute.Count < 1)
		{
			Debug.LogError("No assigned waypoints for patrol - create a patrol route or pick Turrent instead");
		}
		else
		{
			if (transform.position == patrolRoute.patrolRoute[patrolRoute.patrolTarget].position)
			{
				if (patrolRoute.patrolTarget == patrolRoute.patrolRoute.Count - 1 || patrolRoute.patrolTarget == 0)
				{
					patrolRoute.walkingDir *= -1;
				}
				patrolRoute.patrolTarget += patrolRoute.walkingDir;
			}
			transform.position = Vector3.MoveTowards(transform.position, patrolRoute.patrolRoute[patrolRoute.patrolTarget].position, patrolRoute.speed * Time.deltaTime);
		}
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
			if (enemyAttacks[getAttackSeed].particleEffect.Count > 0)
			{
				for (int i = 0; i < enemyAttacks[getAttackSeed].particleEffect.Count; i++)
				{
					bulletBehavior.particleEffects.Add(enemyAttacks[getAttackSeed].particleEffect[i]);
				}
			}
			bullet.GetComponent<Renderer>().material.color = enemyAttacks[getAttackSeed].bulletColor;


			isAttackReady = false;
			StartCoroutine(Cooldown(enemyAttacks[getAttackSeed].attackSpeed));
		}
	}

	private void NoticeBehavior()
	{
		//
	}

	IEnumerator Cooldown(float cooldownTime)
	{
		yield return new WaitForSeconds(cooldownTime);
		isAttackReady = true;
	}

	[SerializeField]
	private float noticeRange;
	[SerializeField]
	private float bufferRange;
	[SerializeField]
	private float attackRange;

	
	private void GetEnemyState()
	{
		switch (objectState)
		{
			case (enemyState.idle):
				PlayerUndetectedState();
				break;
			case (enemyState.notice):
				PlayerDetectedState();
				break;
			case (enemyState.attack):
				DetectionStateAttack();
				break;
			default:
				Debug.LogError("Unknown Detection State Entered " + objectState);
				break;
		}
	}

	private void PlayerUndetectedState()
	{

		Vector3 dir = player.position - transform.position;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;
		Debug.DrawRay(transform.position, dir);
		if (Physics.Raycast(ray, out hit, noticeRange))
		{
			if (hit.transform.tag == "Player")
			{
				objectState = enemyState.notice;
			}
		}
	}

	private void PlayerDetectedState()
	{
		Vector3 dir = player.position - transform.position;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, attackRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
			}
			else
			{
				objectState = enemyState.attack;
			}
		}
		else if (Physics.Raycast(ray, out hit, noticeRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
			}
			else
			{
				objectState = enemyState.notice;
			}
		}
		else
		{
			objectState = enemyState.idle;
		}
	}
	private void DetectionStateAttack()
	{
		Vector3 dir = player.position - transform.position;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, bufferRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
			}
		}
		else if (Physics.Raycast(ray, out hit, noticeRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
			}
			else
			{
				objectState = enemyState.notice;
			}
		}
		else
		{
			objectState = enemyState.idle;
		}
	}
}