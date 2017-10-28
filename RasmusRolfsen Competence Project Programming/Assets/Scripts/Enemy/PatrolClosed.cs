using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolClosed : PatrolOpen
{

	public override void Patrol()
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

}

