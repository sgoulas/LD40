using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinController : MonoBehaviour
{
	public GameObject bloodsplatter;

	private Transform playerTransform;
	private Animator monsterAnimator;
	private SpriteRenderer goblinSprite;
	private BoxCollider2D goblinCollider;
	private int health = 1;
	private float speed = 2f;
	private bool playerInAggroRange;
	private float minimumRange = 0.25f;
	private float timeBetweenAttacks = 1f;
	private float timeSinceLastAttack = 1f;

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
		}
	}

	void MoveToPoint(Vector2 targetPosition)
	{
		transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
	}

	IEnumerator AttackMelee(GameObject target)
	{
		FindObjectOfType<AudioManager> ().Play ("goblinAttack");
		speed = 0;
		monsterAnimator.SetTrigger("isAttacking");
		yield return new WaitForSecondsRealtime(0.25f);
		DamagePlayer(target);
		yield return new WaitForSecondsRealtime(0.75f);
		timeSinceLastAttack = 0;
		speed = 2f;
	}

	public void Damage(int damage)
	{
		health -= damage;
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}

	private void DamagePlayer(GameObject player)
	{
		PlayerHealthAndMagic stats = player.GetComponent<PlayerHealthAndMagic>();
		if (stats == null)
			return;

		stats.Damage(10);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			playerInAggroRange = true;
		}
		if (collision.IsTouching(goblinCollider))
		{
			if (collision.gameObject.CompareTag("PlayerProjectiles"))
			{
				Destroy(collision.gameObject);
				health -= 1;
				if (health <= 0)
					Destroy(gameObject);
			}
			else if (collision.gameObject.CompareTag("PlayerFailMissile"))
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

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("PlayerProjectiles"))
		{
			Destroy(gameObject);
		}
		if (collision.gameObject.CompareTag("Player"))
		{
			if(timeSinceLastAttack >= timeBetweenAttacks)
				StartCoroutine(AttackMelee(collision.gameObject));
		}
	}

	private void OnDestroy()
	{
		Vector2 deathEffectSpawn = new Vector2(transform.position.x, transform.position.y + 0.5f);
		Instantiate(bloodsplatter, deathEffectSpawn, Quaternion.identity);
	}
}
