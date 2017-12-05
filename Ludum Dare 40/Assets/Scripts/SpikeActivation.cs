using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeActivation : MonoBehaviour {

	private Animator trapAnimator;

	private void Start()
	{
		trapAnimator = GetComponentInParent<Animator>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{

		if (collision.gameObject.CompareTag("Player"))
		{
			trapAnimator.SetTrigger("activationTrigger");
			PlayerHealthAndMagic statController = collision.gameObject.GetComponent<PlayerHealthAndMagic>();
			statController.Damage(30);
		}
	}

}
