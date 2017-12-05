using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	private PlayerHealthAndMagic statController;
	private Slider healthBar;
	private Slider magicBar;

	void Awake()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		statController = player.GetComponent<PlayerHealthAndMagic>();

		healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
		magicBar = GameObject.Find("MagicBar").GetComponent<Slider>();
	}

	void Start()
	{
		UpdateDisplay();
	}

	void Update()
	{
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		healthBar.value = (float)statController.health / 100;
		magicBar.value = statController.magic / 100;
	}
}
