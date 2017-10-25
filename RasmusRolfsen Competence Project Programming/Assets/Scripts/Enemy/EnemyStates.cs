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
	[SerializeField]
	private float noticeRange;
	[SerializeField]
	private float bufferRange;
	[SerializeField]
	private float attackRange;
	private Transform player;
	private bool isAttackReady = true;
	private bool isPathBlocked = false;
	private Vector3 startPosition;



	private void Awake()
	{
		startPosition = transform.position;
		isPathBlocked = false;
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
		if (Physics.Raycast(ray, out hit, noticeRange))
		{
			if (hit.transform.tag == "Player")
			{
				objectState = enemyState.notice;
				ChangeMaterialColor(unitColors[1]);
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
		else if (isPathBlocked == false)
		{
			if (patrolRoute.myPatrolType == PatrolRoute.patrolTypes.openLoop)
			{
				OpenLoopMovement();
			}
			else if (patrolRoute.myPatrolType == PatrolRoute.patrolTypes.closedLoop)
			{
				ClosedLoopMovement();
			}
		}
		else
		{
			Ray ray = new Ray(transform.position, startPosition - transform.position);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Vector3.Distance(startPosition, transform.position)))
			{
				if (hit.transform.tag == "Ground")
				{
					Debug.Log("I am here");
					for (int i = 0; i < patrolRoute.patrolRoute.Count; i++)
					{
						Ray newRay = new Ray(transform.position, patrolRoute.patrolRoute[i].position - transform.position);
						RaycastHit newHit;
						if (Physics.Raycast(newRay, out newHit, Vector3.Distance(patrolRoute.patrolRoute[i].position, transform.position)))
						{
							if (newHit.transform.tag == "Ground")
							{
								// Ignore
							}
							else
							{
								Debug.Log("Found target " + i);
								patrolRoute.patrolTarget = i;
								isPathBlocked = false;
								break;
							}
						}
						if (i == patrolRoute.patrolRoute.Count - 1)
						{
							thisType = enemyType.turrent;
							Debug.Log("No possible paths found");
						}
					}
				}
				else
				{
					transform.position = Vector3.MoveTowards(transform.position, startPosition, patrolRoute.speed*Time.deltaTime);
					if (transform.position == startPosition)
					{
						thisType = enemyType.turrent;
					}
				}
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, startPosition, patrolRoute.speed* Time.deltaTime);
				if (transform.position == startPosition)
				{
					thisType = enemyType.turrent;
				}
			}
		}
	}

	private void OpenLoopMovement()
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

	private void ClosedLoopMovement()
	{
		if (transform.position == patrolRoute.patrolRoute[patrolRoute.patrolTarget].position)
		{
			if (patrolRoute.patrolTarget == patrolRoute.patrolRoute.Count - 1)
			{
				patrolRoute.patrolTarget = 0;
			}
			else
			{
				patrolRoute.patrolTarget++;
			}
		}
		transform.position = Vector3.MoveTowards(transform.position, patrolRoute.patrolRoute[patrolRoute.patrolTarget].position, patrolRoute.speed * Time.deltaTime);
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

	}

	private void ChangeMaterialColor(Color col)
	{
		GetComponent<Renderer>().material.color = col;
	}

	IEnumerator Cooldown(float cooldownTime)
	{
		yield return new WaitForSeconds(cooldownTime);
		isAttackReady = true;
	}
}