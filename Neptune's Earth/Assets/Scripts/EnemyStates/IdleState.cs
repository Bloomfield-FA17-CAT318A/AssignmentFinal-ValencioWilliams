using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{

	private Enemy enemy;

	private float idleTimer;

	private float idleDuration;

	public void Enter (Enemy enemy)
	{
		this.enemy = enemy;
		idleDuration = Random.Range (1, 3);
	}

	public void Starting ()
	{
		Idle ();
	}

	public void Ending ()
	{

	}
		
	public void OnTriggerEnter2D (Collider2D other)
	{
		//Enemy does not need to do anything in this state other than wai
	}

	private void Idle ()
	{
		enemy.MyAnimator.SetFloat ("speed", 0f);

		idleTimer += Time.deltaTime;

		if (idleTimer >= idleDuration)
		{
			enemy.ChangeState (new WalkState ());
		}
	}
}