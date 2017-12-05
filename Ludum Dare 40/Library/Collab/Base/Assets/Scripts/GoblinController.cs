using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{

	private Transform playerTransform;
	private Animator monsterAnimator;
	private SpriteRenderer goblinSprite;
	private BoxCollider2D goblinCollider;
	private float speed = 2f;
	private bool playerInAggroRange;
	private float minimumRange = 0.25f;
	private int damage = 1;
	private float timeBetweenAttacks = 1f;
	private float timeSinceLastAttack = 1f;

	//TODO fix flipping issues!

	void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		monsterAnimator = GetComponent<Animator>();
		goblinSprite = GetComponentInChildren<SpriteRenderer>();
		goblinCollider = GetComponent<BoxCollider2D>();
		playerInAggroRange = false;
	}


	void Update()
	{

		if (playerInAggroRange)
		{
			Vector2 targetPosition;
			if (transform.position.x - playerTransform.position.x < 0)
			{
				goblinSprite.flipX = true;
				targetPosition = new Vector2(playerTransform.position.x - minimumRange, transform.position.y);
				goblinCollider.offset = new Vector2(-0.2372746f, 0.5f);
			}
			else
			{
				goblinSprite.flipX = false;
				targetPosition = new Vector2(playerTransform.position.x + minimumRange, transform.position.y);
				goblinCollider.offset = new Vector2(0.2372746f, 0.5f);
			}
			MoveToPoint(targetPosition);

			timeSinceLastAttack += Time.deltaTime;
			if (Vector2.Distance(transform.position, playerTransform.position) <= minimumRange + 0.05f)
			{
				if (timeSinceLastAttack >= timeBetweenAttacks)
				{
					StartCoroutine(AttackMelee());
					timeSinceLastAttack = 0;
				}
			}
		}
	}

	void MoveToPoint(Vector2 targetPosition)
	{
		transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
	}

	IEnumerator AttackMelee()
	{
		speed = 0;
		monsterAnimator.SetTrigger("isAttacking");
		Debug.Log("Dealt damage to the target");
		yield return new WaitForSecondsRealtime(1);
		speed = 2f;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			playerInAggroRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			playerInAggroRange = false;
		}
	}
}
