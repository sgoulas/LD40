using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	private Rigidbody2D playerRigidBody;
	private Animator playerAnimator;
	private SpriteRenderer playerSprite;
	private ParticleSystem magicModeEffect;
	private PlayerHealthAndMagic statManager;
	public GameObject missile;
	public GameObject failMissile;
	public GameObject shield;
	public GameObject shieldFail;
	public Transform missileSpawnPoint;
	public Image winImage;
	public Image lossImage;
	
	private float jumpforce = 14f;
	private float flyforce = 2f;
	private float speed = 3f;
	private bool magicMode = false;
	private GameObject missileToSpawn;
	private GameObject shieldToSpawn;

	private bool isJumping = false;
	private bool isFlying = false;

	private float timeBetweenAttacks = 1f;
	private float timeSinceLastAttack = 1f;
	private float shieldCooldown = 3f;
	private float timeSinceShield = 3f;
	private bool dealingDamage = false;
	public bool restart = false;

	//TODO maybe i dont need them
	const int goblinDamage = 1;
	const int skeletonDamage = 5;

	void Start () 
	{
		playerRigidBody = gameObject.GetComponent<Rigidbody2D> ();
		playerAnimator = gameObject.GetComponent<Animator> ();
		playerSprite = gameObject.GetComponentInChildren<SpriteRenderer>();//giati to sprite einai mesa sto adeio wizard body
		playerRigidBody.gravityScale = 5f;

		magicModeEffect = GetComponentInChildren<ParticleSystem>();
		magicModeEffect.Stop();

		winImage.enabled = false;
		lossImage.enabled = false;

		statManager = GetComponent<PlayerHealthAndMagic>();

	}

	// Update is called once per frame
	void Update() 
	{
		CheckHealth();

		timeSinceLastAttack += Time.deltaTime;
		timeSinceShield += Time.deltaTime;
		if (Input.GetKeyDown(KeyCode.E))
		{
			ToggleMagicMode();

			if (!magicMode)
			{//if magic mode was just turned off, stop all magical effects
				playerAnimator.SetBool("isFlying", false);
				playerAnimator.SetBool("isFalling", true);
				//stop slowing time
			}
		}

		if(Input.GetKeyDown(KeyCode.S))
			Attack();
		if (Input.GetKeyDown (KeyCode.D) && magicMode)
			StartCoroutine(Shield());

		if (Input.GetKeyDown(KeyCode.R))
			CheckRestart();
	}

	private void FixedUpdate()
	{
		Move ();//vertical move, applies to both modes
		if (!magicMode && Input.GetKeyDown ("space"))//you can only jump while not in magic mode
			Jump();
		if ((magicMode) && Input.GetKey (KeyCode.Space))
			Fly ();
		
		if (isJumping || isFlying)//isJumping is true while wizard is on air, not only when rising into the air
		verticalAnimation ();//TODO what happens if wizard stops chanelling fly while on air
	}


	void Move()
	{
		float translation = Input.GetAxis ("Horizontal") * speed;
		translation *= Time.deltaTime;//wste oso 8elw na kinoumai se 1 sec na ginetai swsta scale down gia kinisi se 1 tick.

		if (translation < 0) {
			playerSprite.flipX = true;
		} else if (translation > 0) {
			playerSprite.flipX = false;

		}

		//Debug.Log (translation);

		if (translation != 0)
			playerAnimator.SetBool ("isRunning", true);
		else
			playerAnimator.SetBool ("isRunning", false);
		
		transform.Translate (translation, 0, 0);
	}
	void Jump()
	{
		if (!isJumping) 
		{
			playerRigidBody.velocity = new Vector2 (0, jumpforce);
			FindObjectOfType<AudioManager> ().Play ("playerJump");
			isJumping = true;
		}

	}

	void Attack ()
	{
		if (timeSinceLastAttack < timeBetweenAttacks)
		{
			return;
		}

		if (magicMode)
		{
			statManager.IncreaseMagic(20f);
			playerAnimator.SetTrigger ("isAttacking");

			if(Random.Range(0.4f, 1f) <= statManager.magic * 0.008f)
			{
				FindObjectOfType<AudioManager>().Play("fireBallNerfed");
				missileToSpawn = failMissile;
			}
			else
			{
				FindObjectOfType<AudioManager>().Play("fireball");
				missileToSpawn = missile;
			}

			if (playerSprite.flipX) {
				double diff = transform.position.x - 0.526;
				Vector3 newSpawn = new Vector3 ((float)diff, missileSpawnPoint.position.y, 0);
				Instantiate (missileToSpawn, newSpawn, Quaternion.identity);
			} else {
				Instantiate (missileToSpawn, missileSpawnPoint.position, Quaternion.identity);
			}


		}
		else
		{
			playerAnimator.SetTrigger ("isAttacking");
			StartCoroutine(DealDamage());
			FindObjectOfType<AudioManager> ().Play ("playerMeleeAttack");
		}

		timeSinceLastAttack = 0;
	}


	IEnumerator Shield()
	{
		if (timeSinceShield >= shieldCooldown)
		{
			statManager.IncreaseMagic(50f);
			if (Random.Range(0.4f, 1f) <= statManager.magic * 0.008f)
			{
				shieldToSpawn = shieldFail;
				statManager.SetVulnerable();
			}
			else
			{
				shieldToSpawn = shield;
				statManager.SetImmune();
			}
			timeSinceShield = 0f;
			playerAnimator.SetTrigger("isAttacking");
			GameObject shieldObject = Instantiate(shieldToSpawn, transform.position, Quaternion.identity) as GameObject;
			shieldObject.transform.SetParent(transform);
			yield return new WaitForSecondsRealtime(3f);
			statManager.ResetFlags();
			Destroy(shieldObject);
		}
	}

	void ToggleMagicMode()
	{
		magicMode = !magicMode;

		if (magicMode) 
		{
			magicModeEffect.Play ();
			FindObjectOfType<AudioManager> ().Play ("toggleMagic");
		}
		else
			magicModeEffect.Stop();

	}
		
	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Ground")
		{
			isJumping = false;
			playerAnimator.SetBool ("isFalling", false);
			playerAnimator.SetBool ("isJumping", false);
			playerAnimator.SetBool ("isFlying", false);
		}
		else if((coll.gameObject.tag == "Enemy") && dealingDamage)
		{
			if (coll.gameObject.GetComponent<GoblinController>() != null)
			{
				coll.gameObject.GetComponent<GoblinController>().Damage(1);
			}
			else if (coll.gameObject.GetComponent<SkeletonController>() != null)
			{
				coll.gameObject.GetComponent<SkeletonController>().Damage(1);
			}
		}
			
	}

	void verticalAnimation ()
	{
		float verticalSpeed = playerRigidBody.velocity.y;

		if (magicMode)
		{
			if ((verticalSpeed > 0) && Input.GetKey (KeyCode.Space))//moving up or down while in magic mode and space bar is pressed == flying
				playerAnimator.SetBool ("isFlying", true);

			if (verticalSpeed < 0) {
				playerAnimator.SetBool ("isFlying", false);
				playerAnimator.SetBool ("isFalling", true);
			}
		}
		else
		{
			if (verticalSpeed > 0) {
				playerAnimator.SetBool ("isJumping", true);
				playerAnimator.SetBool ("isFalling", false);
			} else if (verticalSpeed < 0) {
				playerAnimator.SetBool ("isJumping", false);
				playerAnimator.SetBool ("isFalling", true);
			} 
		}

	}

	void Fly()
	{
		statManager.IncreaseMagic(0.5f);

		float rand = Random.Range(0.6f, 1f);
		if (rand <= statManager.magic * 0.008f)
		{
			float xEject = Random.Range(-1f, 1f) * 40;
			float yEject = Random.Range(-1f, 1f) * 40;

			playerRigidBody.velocity = new Vector2(xEject, yEject);
			ToggleMagicMode();
			return;
		}

		FindObjectOfType<AudioManager> ().Play ("playerFlying");//plays when spacebar is released, probably because it plays constantly while fly is called, so the music does not progress

		playerRigidBody.velocity = new Vector2 (0, flyforce);
		verticalAnimation ();
	}

	IEnumerator DealDamage()
	{
		dealingDamage = true;
		yield return new WaitForSeconds(0.5f);
		dealingDamage = false;
	}

	void CheckHealth()
	{
		if(statManager.health <= 0)
		{
			playerAnimator.SetTrigger("dead");
			lossImage.enabled = true;
			gameObject.GetComponent<PlayerController>().enabled = false;
			magicModeEffect.Stop();
			restart = true;
		}
	}

	void CheckRestart()
	{
		if(restart)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("EnemyProjectiles"))
		{
			statManager.Damage(skeletonDamage);
		}
	}
}
