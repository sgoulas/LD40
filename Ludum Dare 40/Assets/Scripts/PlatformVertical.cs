using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformVertical : MonoBehaviour
{

	public float amplitude;
	private Vector3 startPosition;

	private void Start()
	{
		startPosition = transform.position;
	}

	private void Update()
	{
		transform.position = startPosition + new Vector3(0.0f, amplitude * Mathf.Sin(Time.time), 0.0f);
	}
}
