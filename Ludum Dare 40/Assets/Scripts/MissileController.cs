using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour {

	private Rigidbody2D missileRigidBody;
	private float missileSpeed = 8f;
	private GameObject player;
	private SpriteRenderer playerSprite;
	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player");
		playerSprite = player.GetComponentInChildren<SpriteRenderer> ();

		missileRigidBody = gameObject.GetComponent<Rigidbody2D> ();
		if (playerSprite.flipX)
		{
			missileSpeed = -missileSpeed;
			transform.localScale = new Vector3(-1, 1, 1);
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		missileRigidBody.velocity = new Vector2 (missileSpeed, 0);
		if (!isVisibleToCamera(transform))
			Destroy (gameObject);
	}

	public static bool isVisibleToCamera (Transform transform)
	{
		Vector3 viTest = Camera.main.WorldToViewportPoint (transform.position);
		return (viTest.x >= 0 && viTest.y >= 0) && (viTest.x <= 1 && viTest.y <= 1) && viTest.z >= 0;
	}
}
