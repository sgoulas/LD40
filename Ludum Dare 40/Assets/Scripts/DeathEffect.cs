using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEffect : MonoBehaviour {

	private ParticleSystem deathEffect;

	private void Awake()
	{
		deathEffect = GetComponentInChildren<ParticleSystem>();
		deathEffect.Stop();
	}


	void Start () {
		StartCoroutine(PlayerDeathEffect());
	}

	IEnumerator PlayerDeathEffect()
	{
		deathEffect.Play();
		yield return new WaitForSeconds(0.7f);
		Destroy(deathEffect);
		Destroy(gameObject);
	}	
}
