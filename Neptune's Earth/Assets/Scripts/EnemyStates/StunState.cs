using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : IEnemyState {
	private Enemy enemy;

	private float stunTimer;

	private float stunDuration = 3f;

	public void Enter(Enemy enemy)
	{
		this.enemy = enemy;
	}

	public void Starting ()
	{
		Debug.Log ("STUNNED ENEMY");
		Stun ();

	}
	public void Ending()
	{

	}
	public void OnTriggerEnter2D (Collider2D other)
	{

	}

	private void Stun()
	{
		stunTimer += Time.deltaTime;

		if (stunTimer >= stunDuration)
		{
			enemy.ChangeState (new WalkState ());
		} else
		{
			Stop ();
		}
	}

	private void Stop()
	{
		enemy.myRigidBody.velocity = new Vector2(0,0);
	}
}
