using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

	[SerializeField]
	private Collider2D other;

	[SerializeField]
	private CircleCollider2D otherC;

	private void Awake()
	{
		Physics2D.IgnoreCollision (GetComponent<Collider2D> (), other, true);
//		Physics2D.IgnoreCollision (GetComponent<CircleCollider2D> (), otherC, true);
	}
}
