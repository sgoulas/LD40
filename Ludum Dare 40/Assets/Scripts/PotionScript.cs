using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionScript : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))
		{
			PlayerHealthAndMagic statManager = collision.gameObject.GetComponent<PlayerHealthAndMagic>();
			statManager.health = 100;
			statManager.magic = 0;
			Destroy(gameObject);
		}
	}
}
