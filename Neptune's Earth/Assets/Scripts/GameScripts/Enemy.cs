using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Characters
{

	public static int enemyCounter;

	public Player player;

	#region Variables
	[SerializeField]
	private GameObject boss;

	[SerializeField]
	private Collider2D col;

	[SerializeField]
	private Collider2D col2;

	public Rigidbody2D myRigidBody;

	[SerializeField]
	private int enemyHealth;

	private IEnemyState currentState;

	#endregion

	#region Properties
	public override bool IsDead
	{
		get
		{
			return enemyHealth <= 0;
		}
	}
	#endregion

	public override void Start ()
	{
		base.Start ();

		player = FindObjectOfType<Player> ();

		ChangeState (new WalkState ());

		enemyCounter++;

		if (transform.position.x > boss.transform.position.x)
		{
			ChangeDirection ();
			faceRight = false;
		}
	}

	void Update ()
	{ 
		if (!IsDead)
		{
			if (!TakingDamage)
			{
				currentState.Starting ();
			}
		}
					
	}

	public void ChangeState (IEnemyState newState)
	{
		if (currentState != null)
		{
			currentState.Ending ();
		}

		currentState = newState;

		currentState.Enter (this);
	}

	public void Move ()
	{
		MyAnimator.SetFloat ("speed", 1);

		transform.Translate (GetDirection () * (maxSpeed * Time.deltaTime));
	}

	public Vector2 GetDirection ()
	{
		return faceRight ? Vector2.right : Vector2.left;
	}

	#region Triggers/Collisions

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.tag == "Player" && player.MyRigidbody.velocity.y < 1f)
		{
			StartCoroutine (TakeDamage ());
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Projectile")
		{
			Destroy (other.gameObject);
			ChangeState (new StunState ());
		}
	}

	#endregion

	public override void Death ()
	{
		Destroy (gameObject, 1f);
	}

	public override IEnumerator TakeDamage ()
	{
		enemyHealth -= 10;

		if (!IsDead)
		{
			MyAnimator.SetTrigger ("damage");
		} 
		else
			
		{
			if (GameController.Instance != null)
			{
				Instantiate (GameController.Instance.ItemPrefab, new Vector3 (transform.position.x, transform.position.y), Quaternion.identity);
			}
					
			TurnOffCollisions ();

			MyAnimator.SetTrigger ("die");
			enemyCounter--;
			yield return null;
		}
	}

	public void ChangeSpeed(float sp)
	{
		maxSpeed = sp;
	}
		
	private void TurnOffCollisions ()
	{
		col.enabled = false;
		col2.enabled = false;
		myRigidBody.velocity = Vector2.zero;
		myRigidBody.isKinematic = true;

	}
}
