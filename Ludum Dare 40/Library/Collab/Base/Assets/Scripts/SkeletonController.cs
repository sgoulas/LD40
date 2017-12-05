using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	public GameObject skeletonArrow;
	public Transform skeletonShootPoint;

	private Transform playerTransform;
	private Animator skeletonAnimator;
	private SpriteRenderer skeletonSprite;
	private float speed = 0.5f;
	private bool playerInAggroRange;
	private float attackRange = 4f;
	private int damage = 1;
	private float timeBetweenAttacks = 1f;
	private float timeSinceLastAttack = 1f;

	private void Start()
	{
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
		skeletonAnimator = GetComponent<Animator>();
		skeletonSprite = GetComponentInChildren<SpriteRenderer>();
		playerInAggroRange = false;
	}

	private void Update()
	{
		if(playerInAggroRange)
		{
			Vector2 targetPosition;
			if (transform.position.x - playerTransform.position.x < 0)
			{
				skeletonSprite.flipX = true;
				targetPosition = new Vector2(playerTransform.position.x - attackRange, transform.position.y);
				skeletonShootPoint.localPosition = new Vector3(0.411f, 0.482f, 0);
			}
			else
			{
				skeletonSprite.flipX = false;
				targetPosition = new Vector2(playerTransform.position.x + attackRange, transform.position.y);
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
		Debug.Log("Dealt damage to the target");
		yield return new WaitForSeconds(1);
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
