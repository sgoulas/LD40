using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformHorizontal : MonoBehaviour
{

	public float amplitude;
	private Vector3 startPosition;

	private void Start()
	{
		startPosition = transform.position;
	}

	private void Update()
	{
		transform.position = startPosition + new Vector3(amplitude * Mathf.Sin(Time.time), 0.0f, 0.0f);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.transform.SetParent(transform);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			collision.gameObject.transform.parent = null;
		}
	}
}
