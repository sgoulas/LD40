using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {

	private Rigidbody2D arrowRB;
	private float arrowSpeed = -5f;
	private SpriteRenderer shooterSprite;
	private SpriteRenderer arrowSprite;

	void Start()
	{
		if(transform.parent.gameObject != null)
		{
			shooterSprite = transform.parent.gameObject.GetComponentInChildren<SpriteRenderer>(); ;
			transform.parent = null;
		}
		arrowSprite = gameObject.GetComponent<SpriteRenderer>();

		arrowRB = gameObject.GetComponent<Rigidbody2D>();
		if(shooterSprite.flipX)
		{
			arrowSpeed = -arrowSpeed;
		}
	}

	void Update()
	{
		arrowRB.velocity = new Vector2(arrowSpeed, 0);
		if(!arrowSprite.isVisible)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			Destroy(gameObject);
		}
	}
}
