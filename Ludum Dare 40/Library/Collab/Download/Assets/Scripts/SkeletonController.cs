using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	public GameObject skeletonArrow;
	public Transform skeletonShootPoint;
	public GameObject boneChips;

	private Transform playerTransform;
	private Animator skeletonAnimator;
	private SpriteRenderer skeletonSprite;
	private Vector2 targetPosition;
	private BoxCollider2D bodyCollider;
	private float speed = 1f;
	private bool playerInAggroRange;
	private float attackRange = 4f;
	private float timeBetweenAttacks = 1f;
	private float timeSinceLastAttack = 1f;
	[SerializeField]
	private int health = 1;

	private void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		skeletonAnimator = GetComponent<Animator>();
		skeletonSprite = GetComponentInChildren<SpriteRenderer>();
		bodyCollider = GetComponent<BoxCollider2D>();
		playerInAggroRange = false;
		
	}

	private void Update()
	{
		if(playerInAggroRange)
		{
			if (transform.position.x - playerTransform.position.x < 0)
			{
				skeletonSprite.flipX = true;
				skeletonShootPoint.localPosition = new Vector3(0.411f, 0.482f, 0);
			}
			else
			{
				skeletonSprite.flipX = false;
				skeletonShootPoint.localPosition = new Vector3(-0.411f, 0.482f, 0);
			}

			MoveToPoint(targetPosition);
			

			timeSinceLastAttack += Time.deltaTime;
			if (Vector2.Distance(transform.position, playerTransform.position) <= attackRange + 0.05f)
			{
				if (timeSinceLastAttack >= timeBetweenAttacks)
				{
					StartCoroutine(AttackRanged());
					timeSinceLastAttack = 0;
				}
			}
		}
	}

	void MoveToPoint(Vector2 targetPosition)
	{
		transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
	}

	IEnumerator AttackRanged()
	{
		speed = 0;
		skeletonAnimator.SetTrigger("isAttacking");
		GameObject arrow =  Instantiate(skeletonArrow, skeletonShootPoint.position, Quaternion.identity) as GameObject;
		arrow.transform.SetParent(transform);
		yield return new WaitForSeconds(1);
		speed = 1f;
	}

	public void Damage(int damage)
	{
		health -= damage;
		Debug.Log("health is" + health);
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			playerInAggroRange = true;
			targetPosition = new Vector2(playerTransform.position.x + attackRange, transform.position.y);
		}
		if(collision.IsTouching(bodyCollider))
		{
			if(collision.gameObject.CompareTag("PlayerProjectiles"))
			{
				Destroy(collision.gameObject);
				health -= 1;
				if (health <= 0)
					Destroy(gameObject);
			}
			else if(collision.gameObject.CompareTag("PlayerFailMissile"))
			{
				Destroy(collision.gameObject);
				transform.localScale = new Vector3(2, 2, 0);
				health = 2;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			playerInAggroRange = false;
		}
	}

	private void OnDestroy()
	{
		Vector2 deathEffectSpawn = new Vector2(transform.position.x, transform.position.y + 0.5f);
		Instantiate(boneChips, deathEffectSpawn, Quaternion.identity);
	}

}
