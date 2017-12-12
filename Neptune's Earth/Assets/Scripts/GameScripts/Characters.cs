using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Characters : MonoBehaviour
{

	[SerializeField]
	protected float maxSpeed;

	protected bool faceRight;

	protected bool attack;

	public bool TakingDamage { get; set; }

	public abstract bool IsDead { get; }

	public Animator MyAnimator{ get; private set; }

	/*	All characters must CHANGE DIRECTION
		All characters must DIE
		All characters must TAKEDAMAGE
	*/
	public virtual void Start ()
	{
		faceRight = true;
		MyAnimator = GetComponent<Animator> ();
	}

	public void ChangeDirection ()
	{
		faceRight = !faceRight;
		Vector3 thePlayerScale = transform.localScale;
		thePlayerScale.x *= -1;
		transform.localScale = thePlayerScale;
	
	}

	public abstract void Death ();

	public abstract IEnumerator TakeDamage ();

}