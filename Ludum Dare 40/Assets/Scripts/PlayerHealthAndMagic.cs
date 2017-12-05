using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthAndMagic : MonoBehaviour {

	public float magicDegen = 0.1f;

	public int health = 100;
	public float magic = 0f;
	private bool immune;
	private bool vulnerable;
	
	// Use this for initialization
	void Start () {
		immune = false;
		vulnerable = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		DecreaseMagic(magicDegen);
	}

	public void Damage(int damage)
	{
		if (immune)
			return;
		else if (vulnerable)
			damage *= damage;
		health -= damage;
		if(health <= 0)
		{
			Debug.Log("Player Died");
		}
	}

	public void IncreaseMagic(float magic)
	{
		this.magic += magic;
		if(this.magic > 100)
		{
			this.magic = 100;
			StartCoroutine(Overload());
		}
	}

	public void DecreaseMagic(float magic)
	{
		this.magic -= magic;
		if(this.magic < 0)
		{
			this.magic = 0;
		}
	}

	public void SetImmune()
	{
		immune = true;
	}

	public void SetVulnerable()
	{
		vulnerable = true;
	}

	public void ResetFlags()
	{
		immune = false;
		vulnerable = false;
	}

	IEnumerator Overload()
	{
		magicDegen = 0;
		yield return new WaitForSecondsRealtime(3f);
		magicDegen = 0.1f;
	}
}
