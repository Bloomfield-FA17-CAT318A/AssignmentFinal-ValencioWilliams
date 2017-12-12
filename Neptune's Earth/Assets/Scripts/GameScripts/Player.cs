using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public delegate void DeadEventHandler ();


public class Player : Characters
{

	public static int playerNum = 1;

	#region PlayerID

	[Space]
	[Header ("Player ID Info")]

	[SerializeField]
	private int playerID = 0;

	[SerializeField]
	private Text playerText;

	public GameObject playerIdentifierObject;

	public Vector3 offset;


	#endregion

	#region Events

	public event DeadEventHandler Dead; //Event that tells the player is dead

	#endregion

	#region Properties

	public override bool IsDead //Boolean using the Dead event will be true if the player dies
	{
		get
		{
			if (health.CurrentVal <= 0)
			{
				OnDead ();
			}

			return health.CurrentVal <= 0;
		}
	}

	public Rigidbody2D MyRigidbody { get; set; }

	private bool OnGround { get; set; }

	#endregion

	[Space]

	#region Variables

	[SerializeField]
	private Collider2D col1; //Player's first collider

	[SerializeField]
	private Collider2D col2; //Player's second collider

	[Space]

	[Header ("UI Stats")]

	public Stat health; //Player's health stats

	public Stat ammo; //Player's ammo stats

	[Space]

	[Header ("Projectile Physics")]

	[SerializeField]
	private GameObject projectile; //Player's food prefab

	[SerializeField]
	private int throwingSpeed = 0; //The speed the player will throw the food (set in inspector)

	[Space]

	[Header ("Ground Checks")]

	[SerializeField]
	private Transform[] groundPoints; //Transforms under the player to detect the ground

	[SerializeField]
	private float groundRadius; //The size of the groundpoint's check

	[SerializeField]
	private LayerMask whatIsGround; //Label the ground the check is going for

	[Space]

	[Header ("Jump Control")]

	[SerializeField]
	private bool airControl; //Allow the player to move in the air?

	[SerializeField]
	private float jumpForce; //How strong will the player jump in the air?

	[SerializeField]
	private float bounceForce; //How strong the player will bounce off of enemies

	public bool jump; //Checks if the player jumped

	public bool dropDown; //Checks if the player dropped down from a platform

	public float dropDownTimer; //Amount of timer before dropdown is set back to false

	[Space]

	[Header ("Damage Control")]

	[SerializeField]
	private float knockBack; //The amount of force to push back the player after getting damaged

	[SerializeField]
	private Vector2 startPos; //A position on the scene where the player will spawn

	private bool bounceTriggered = false; //Determined whether the player bounced on an enemy

	private bool immortal = false; //Boolean for player immunity

	[SerializeField]
	private float immortalTime; //Float for how long the player will be immune

	private SpriteRenderer mySprite; //Current player sprite (used for immunity)

	[Space]
			
	#endregion

	#region Audio Variables

	[Space]

	[Header ("Audio Control")]

	public AudioSource playerSound;

	public AudioClip jumpSound;


	#endregion

	#region Analytics Variables
	public int deathCount;
	#endregion

	public float jumpTemp;

	public void Awake ()
	{
		playerNum = 1;
		playerID = 0;

		health.Initialize ();
		ammo.Initialize ();
	
		SetPlayerValues ();
	}

	public override void Start ()
	{

		playerID = playerNum++;

		playerText = playerIdentifierObject.GetComponentInChildren<Text> ();
		playerIdentifierObject = (GameObject)Instantiate (playerIdentifierObject, Vector3.zero, Quaternion.identity);
					
		base.Start ();
	
		mySprite = GetComponent<SpriteRenderer> ();
		MyRigidbody = GetComponent<Rigidbody2D> ();
		projectile.GetComponent<GameObject> ();

		playerText.text = "P" + playerID;

		gameObject.name = "Player " + playerID;

		//Debug.Log ("ENTER PLAYER: " + playerID);


	}

	void Update ()
	{
		playerIdentifierObject.transform.position = transform.position;

		if (!TakingDamage && !IsDead)
		{
			if (transform.position.y <= -14f)
			{
				Death ();
			}
		}

		PlayerInputs ();
	}

	void FixedUpdate ()
	{

		if (!IsDead)
		{
			if (!TakingDamage)
			{
				float move = Input.GetAxis ("J" + playerID + "Horizontal");

				OnGround = IsGrounded ();

				PlayerMovement (move);

				Flip (move);

				ChangeLayerWeight ();
			}
		}

	}

	private void SetPlayerValues ()
	{
		health.MaxVal = 100;
		health.CurrentVal = 100;

		ammo.MaxVal = 10;
		ammo.CurrentVal = 2;

		throwingSpeed = 10;
	}

	#region Movement/Input

	private void PlayerMovement (float move)
	{
		if (MyRigidbody.velocity.y < 0.1f)
		{
			MyAnimator.SetBool ("land", true);
			MyRigidbody.gravityScale = 20f;
		}

		if (OnGround || airControl)
		{
			MyRigidbody.velocity = new Vector2 (move * maxSpeed, MyRigidbody.velocity.y);
		}

		if (jump && MyRigidbody.velocity.y == 0)
		{
			MyAnimator.SetTrigger ("jump");
			playerSound.clip = jumpSound;
			playerSound.Play ();
			//MyRigidbody.AddForce (new Vector2 (0, jumpForce),ForceMode2D.Force);
			MyRigidbody.velocity = new Vector3 (0, jumpTemp, 0);
		}

		if (OnGround)
		{
			MyRigidbody.gravityScale = 11f;
			MyAnimator.SetBool ("land", false);
			MyAnimator.ResetTrigger ("jump");
			jump = false;
		}

		if (!jump)
		{
			MyAnimator.SetFloat ("speed", Mathf.Abs (move));
		}

		if (dropDown)
		{
			gameObject.layer = 14;
			StartCoroutine (DropDownAvail ());
		}

		if (!dropDown)
		{
			gameObject.layer = 8;
		}

	}

	void PlayerInputs ()
	{
		float moveY = Input.GetAxis ("J" + playerID + "Vertical");

		if (Input.GetButtonDown ("J" + playerID + "Jump") && moveY >= 0)
		{
			jump = true;
		}

		if (moveY < 0 && Input.GetButtonDown ("J" + playerID + "Jump"))
		{
			dropDown = true;
		}
			
		if (Input.GetButtonUp ("J" + playerID + "Jump"))
		{
			MyRigidbody.velocity = new Vector3 (0, -jumpTemp / 3, 0);
		}

		if (Input.GetButtonDown ("J" + playerID + "Throw"))
		{
			MyAnimator.SetTrigger ("throw");
			ThrowItem ();
		}

		if (Input.GetKeyDown (KeyCode.Q))
		{
			health.CurrentVal -= 10;
		}
//
//		if (Input.GetKeyDown (KeyCode.R))
//		{
//			ammo.CurrentVal += 10;
//		}
//
//		if (Input.GetKeyUp (KeyCode.E))
//		{
//			TakeDamage ();
//		}
//			
	}

	void ChangeLayerWeight ()
	{
		if (!OnGround)
		{
			MyAnimator.SetLayerWeight (1, 1);
		} else
		{
			MyAnimator.SetLayerWeight (1, 0);
		}
	}

	void Flip (float move)
	{
		if (move > 0 && !faceRight || move < 0 && faceRight)
		{
			ChangeDirection ();
		}


	}

	bool IsGrounded ()
	{
		if (MyRigidbody.velocity.y <= 0)
		{
			foreach (Transform point in groundPoints)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);
				for (int i = 0; i < colliders.Length; i++)
				{
					if (colliders [i].gameObject != gameObject)
					{
						return true;
					}

				}

			}
		}
		return false;
	}

	#endregion

	#region Player Abilities

	public void ThrowItem ()
	{
		if (ammo.CurrentVal > 0)
		{
			if (faceRight)
			{
				GameObject tmp = (GameObject)Instantiate (projectile, transform.position, Quaternion.identity);
				tmp.GetComponent<Projectile> ().Initialize (Vector2.right * throwingSpeed);

			} else
			{
				GameObject tmp = (GameObject)Instantiate (projectile, transform.position, Quaternion.identity);
				tmp.GetComponent<Projectile> ().Initialize (Vector2.left * throwingSpeed);
			}

			ammo.CurrentVal--;
		}
	}

	#endregion

	#region Trigger/Collision Methods

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Demons" && bounceTriggered == false)
		{
			MyRigidbody.velocity += new Vector2 (0, bounceForce);
		
			bounceTriggered = true;

			StartCoroutine (BounceTimer ());

		}

		if (other.gameObject.tag == "Item")
		{
			Item itemTemp = other.GetComponent<Item> ();

			if (itemTemp.isAmmo && !itemTemp.wasPickedUp)
			{
				itemTemp.wasPickedUp = true;
				ammo.CurrentVal += itemTemp.foodAmmo;
				Destroy (other.gameObject);
			}

			if (itemTemp.isHealing && !itemTemp.wasPickedUp)
			{
				itemTemp.wasPickedUp = true;
				health.CurrentVal += itemTemp.lifeHp;
				Destroy (other.gameObject);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		float xDir = transform.position.x - other.transform.position.x;

		if (other.gameObject.tag == "Demons")
		{
			MyRigidbody.AddForce (new Vector2 (knockBack * xDir, 0));
			StartCoroutine (TakeDamage ());
		}
			
	}

	void turnOffCollisions ()
	{
		col1.enabled = false;
		col2.enabled = false;
		MyRigidbody.velocity = Vector2.zero;
		MyRigidbody.isKinematic = true;

	}

	void turnOnCollisions ()
	{
		col1.enabled = true;
		col2.enabled = true;
		MyRigidbody.velocity = Vector2.zero;
		MyRigidbody.isKinematic = false;

	}

	#endregion

	#region Dead Methods

	public void OnDead ()
	{
		if (Dead != null)
		{
			Dead ();
		}
	}

	public override void Death ()
	{
		MyRigidbody.velocity = Vector2.zero;
		health.CurrentVal = 30;
		MyAnimator.SetTrigger ("idle");
		transform.position = startPos;
		turnOnCollisions ();
	}

	#endregion

	#region IEnumerator Methods

	private IEnumerator Immortality ()
	{
		while (immortal)
		{
			mySprite.enabled = false;	
			yield return new WaitForSeconds (.1f);
			mySprite.enabled = true;
			yield return new WaitForSeconds (.1f);

		}
	}

	public override IEnumerator TakeDamage ()
	{
		if (!immortal)
		{
			health.CurrentVal -= 10;

			if (!IsDead)
			{
				MyAnimator.SetTrigger ("damage");
				immortal = true;
				StartCoroutine (Immortality ());
				yield return new WaitForSeconds (immortalTime);
				immortal = false;
			} else
			{
				MyAnimator.SetLayerWeight (1, 0);
				turnOffCollisions ();
				MyAnimator.SetTrigger ("die");

				yield return new WaitForSeconds (0.2f);
				deathCount++;
				Debug.Log (deathCount);
			}	
		}
	}

	public IEnumerator BounceTimer ()
	{
		yield return new WaitForSeconds (0.1f);
		MyRigidbody.gravityScale = 11f;

		bounceTriggered = false;

	}

	public IEnumerator DropDownAvail ()
	{
		yield return new WaitForSeconds (dropDownTimer);
		dropDown = false;
	}

	#endregion


}
