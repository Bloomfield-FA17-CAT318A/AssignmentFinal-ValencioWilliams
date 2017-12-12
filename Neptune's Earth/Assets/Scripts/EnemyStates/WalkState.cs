using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : IEnemyState
{

	private Enemy enemy;

	private float patrolTimer;

	private float patrolDuration;

	private float randomlyChangeDir;

	public void Enter (Enemy enemy)
	{
		this.enemy = enemy;
		patrolDuration = Random.Range (3, 8);
		randomlyChangeDir = Random.Range (2, 6);

	}

	public void Starting ()
	{
		Walk ();
		enemy.Move ();
	}

	public void Ending ()
	{

	}

	public void OnTriggerEnter2D (Collider2D other)
	{
		
	}

	private void Walk ()
	{
		patrolTimer += Time.deltaTime;

		if (patrolTimer >= patrolDuration)
		{
			enemy.ChangeState (new IdleState ());
		}

		if (patrolTimer >= randomlyChangeDir)
		{
			enemy.ChangeDirection ();
			patrolTimer = 0;
		}
	}


}
