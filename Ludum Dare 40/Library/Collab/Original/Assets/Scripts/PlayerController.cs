using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	private Rigidbody2D playerRigidBody;
	private Animator playerAnimator;
	private SpriteRenderer playerSprite;
	private float jumpforce = 14f;
	private float speed = 5f;
	private bool isAlive = true;
	private int hp = 5;
	private bool magicMode;


	void Start () 
	{
		playerRigidBody = gameObject.GetComponent<Rigidbody2D> ();
		//playerAnimator = gameObject.GetComponent<Animator> ();
		playerSprite = gameObject.GetComponent<SpriteRenderer> ();
		playerRigidBody.gravityScale = 5f;

	}

	// Update is called once per frame
	void Update () 
	{
		if (isAlive) 
		{
			if (Input.GetKeyDown (KeyCode.E)) 
			{
				magicMode = !magicMode;
				Debug.Log ("magic mode set to " + magicMode);
			}

			if (Input.GetKeyDown ("space"))
				Jump();
			if(Input.GetKeyDown(KeyCode.S))
				Attack();
			if (Input.GetKeyDown (KeyCode.D) && magicMode)
				slowTime ();
		}

	}

	private void FixedUpdate()
	{
		if(isAlive)
		{
			Move();
		}
	}


	void Move()
	{
		float translation = Input.GetAxis ("Horizontal") * speed;
		translation *= Time.deltaTime;//wste oso 8elw na kinoumai se 1 sec na ginetai swsta scale down gia kinisi se 1 tick.
		Debug.Log ("translation: " + translation);

		if (translation < 0)
			playerSprite.flipX = true;
			//Debug.Log ("translation: negative");
		else
			playerSprite.flipX = false;
			//Debug.Log ("translation: positive");
		transform.Translate (translation, 0, 0);
	}
	void Jump()
	{
		playerRigidBody.velocity = new Vector2 (0, jumpforce);
		//playerAnimator.SetTrigger ("jump");
	}

	void Attack ()
	{
		if (magicMode)
		{
			Debug.Log ("magic missile");
			//playerAnimator.SetTrigger ("missle");
		}
		else
		{
			Debug.Log ("attack");
			//playerAnimator.SetTrigger ("attack");
		}
	}


	void slowTime()
	{
		Debug.Log ("slooooooow time");
	}
}
