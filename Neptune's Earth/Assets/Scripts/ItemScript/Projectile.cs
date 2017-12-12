using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour {

	#region Variables
	[SerializeField]
	private float speed;

	private Rigidbody2D myRigidbody;

	private Vector2 direction;

	[SerializeField]
	private GameObject stars;

	#endregion

	#region ItemValues
	public float magDmg;

	public float hungerDmg;

	public float dpsDmg;
	#endregion

	void Start () {
		myRigidbody = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate(){
		myRigidbody.velocity = direction * speed;

		if (tag == "Projectile")
		{
			Destroy (gameObject, 4f);
		}
	}

	public void Initialize(Vector2 direction){
		this.direction = direction;
	}

	void OnBecameInvisible()
	{
		Destroy (gameObject);
	}

	#region Collisions/Triggers

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Floor")
		{
			myRigidbody.velocity = Vector2.zero;
		}

		if (gameObject.tag == "Projectile" && (other.gameObject.name == "RightInvisWall") || other.gameObject.name == "LeftInvisWall")
		{
			Destroy (gameObject);
		}
			
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Boss")
		{
			Instantiate (stars,transform.position,Quaternion.identity);
			Destroy (gameObject);
		}
	}

	#endregion
}