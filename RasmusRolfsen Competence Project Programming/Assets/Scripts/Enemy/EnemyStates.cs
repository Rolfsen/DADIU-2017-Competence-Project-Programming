using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public struct EnemyAttackTypes
{
	public float attackSpeed;
	public float bulletSpeed;
	public float damage;
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
}

public class EnemyStates : MonoBehaviour
{
	public Color[] unitColors = new Color[3];
	public enum enemyState { idle, notice, attack };
	public enemyState objectState;


	[SerializeField]
	public List<EnemyAttackTypes> enemyAttacks = null;
	[SerializeField]
	public PatrolRoute patrolRoute;
	[SerializeField]
	public float noticeRange;
	[SerializeField]
	public float bufferRange;
	[SerializeField]
	public float attackRange;
	public Transform player;
	public bool isAttackReady = true;
	public bool isPathBlocked = false;
	public Vector3 startPosition;



	public virtual void Awake()
	{
		startPosition = transform.position;
		isPathBlocked = false;
		patrolRoute.walkingDir = -1;
		isAttackReady = true;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		GetComponent<Renderer>().material.color = unitColors[0];
	}


	// Update is called once per frame
	void Update()
	{

		GetEnemyState();

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

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Ground")
		{
			isPathBlocked = true;
		}
	}

	public virtual void GetEnemyState()
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

	public virtual void PlayerUndetectedState()
	{

		Vector3 dir = player.position - transform.position;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, noticeRange))
		{
			if (hit.transform.tag == "Player")
			{
				objectState = enemyState.notice;
				ChangeMaterialColor(unitColors[1]);
			}
		}
	}

	public virtual void PlayerDetectedState()
	{
		Vector3 dir = player.position - transform.position;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, attackRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
				ChangeMaterialColor(unitColors[0]);

			}
			else
			{
				objectState = enemyState.attack;
				ChangeMaterialColor(unitColors[2]);

			}
		}
		else if (Physics.Raycast(ray, out hit, noticeRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
				ChangeMaterialColor(unitColors[0]);

			}
			else
			{
				objectState = enemyState.notice;
				ChangeMaterialColor(unitColors[1]);

			}
		}
		else
		{
			objectState = enemyState.idle;
			ChangeMaterialColor(unitColors[0]);

		}
	}

	public virtual void DetectionStateAttack()
	{
		Vector3 dir = player.position - transform.position;
		Ray ray = new Ray(transform.position, dir);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, bufferRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
				ChangeMaterialColor(unitColors[0]);

			}
		}
		else if (Physics.Raycast(ray, out hit, noticeRange))
		{
			if (hit.transform.tag != "Player")
			{
				objectState = enemyState.idle;
				ChangeMaterialColor(unitColors[0]);

			}
			else
			{
				objectState = enemyState.notice;
				ChangeMaterialColor(unitColors[1]);
			}
		}
		else
		{
			objectState = enemyState.idle;
			ChangeMaterialColor(unitColors[0]);
		}
	}

	public virtual void IdleBehavior()
	{
		// DO NOTHING
	}

	public virtual void IdlePatrol()
	{
	}

	public virtual void OpenLoopMovement()
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


	public virtual void AttackBehavior()
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

	public virtual void NoticeBehavior()
	{

	}

	public virtual void ChangeMaterialColor(Color col)
	{
		GetComponent<Renderer>().material.color = col;
	}

	public virtual IEnumerator Cooldown(float cooldownTime)
	{
		yield return new WaitForSeconds(cooldownTime);
		isAttackReady = true;
	}
}