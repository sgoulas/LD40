using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookScript : MonoBehaviour {

	private Animator bookAnimator;

	private void Start()
	{
		bookAnimator = GetComponent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			bookAnimator.SetBool("isOpen", true);
			PlayerController player = collision.gameObject.GetComponent<PlayerController>();
			player.winImage.enabled = true;
			player.restart = true;
			player.enabled = false;
		}
	}
}
