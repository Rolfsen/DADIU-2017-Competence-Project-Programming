using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolOpen : EnemyStates
{
	public override void IdleBehavior()
	{
		if (patrolRoute.patrolRoute.Count < 1)
		{
			Debug.LogError("No assigned waypoints for patrol - create a patrol route or pick Turrent instead");
		}
		else if (isPathBlocked == false)
		{
			/*
			if (patrolRoute.myPatrolType == PatrolRoute.patrolTypes.openLoop)
			{
				OpenLoopMovement();
			}
			else if (patrolRoute.myPatrolType == PatrolRoute.patrolTypes.closedLoop)
			{
				ClosedLoopMovement();
			}*/

			Patrol();
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
							//thisType = enemyType.turrent;
							Debug.Log("No possible paths found");
						}
					}
				}
				else
				{
					transform.position = Vector3.MoveTowards(transform.position, startPosition, patrolRoute.speed * Time.deltaTime);
					if (transform.position == startPosition)
					{
						//thisType = enemyType.turrent;
					}
				}
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, startPosition, patrolRoute.speed * Time.deltaTime);
				if (transform.position == startPosition)
				{
					//thisType = enemyType.turrent;
				}
			}
		}
	}

	public virtual void Patrol ()
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
