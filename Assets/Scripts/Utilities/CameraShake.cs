using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	public bool start = false;
	public float duration = 1f;

	void Update()
	{
		if (start)
		{
			start = false;
			StartCoroutine(Shaking());
		}
	}

	public void StartShaking()
    {
		start = true;
    }

	IEnumerator Shaking()
	{
		Vector3 startPosition = transform.position;
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			elapsedTime += Time.deltaTime;
			transform.position = startPosition + Random.insideUnitSphere * 1.5f;
			yield return null;
		}

		transform.position = startPosition;
	}
}
