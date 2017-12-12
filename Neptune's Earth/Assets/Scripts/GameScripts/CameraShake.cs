using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

	public Vector2 velocity;
	public float shake = 0f;
	public float shakeAmount = 0.7f;
	public float shakeRate = 1f;

	public bool on;

	[SerializeField]
	private Transform camTransform;

	[SerializeField]
	private float decrease = 1f;

	[SerializeField]
	private bool shaking = true;

	Vector3 originalpos;

	void Awake ()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent (typeof(Transform)) as Transform;
		}
	}

	void Start()
	{
		originalpos = camTransform.position;
	}

	void OnEnable ()
	{
		originalpos = camTransform.localPosition;
	}

	void Update ()
	{
		StartCoroutine (EarthQuake ());
	}

	IEnumerator EarthQuake ()
	{
		if (on)
		{
			if (shake > 0 && shaking)
			{
				camTransform.localPosition = originalpos + Random.insideUnitSphere * shakeAmount;
				shake -= Time.deltaTime * decrease;

				yield return new WaitForSeconds (shakeRate);
				shaking = false;
			} else
			{
				shake += 1;
				camTransform.localPosition = originalpos;

				yield return new WaitForSeconds (shakeRate);
				shaking = true;
			}
		}
	}

	public void TriggerQuake (float shakerAmount)
	{
		camTransform.localPosition = originalpos + Random.insideUnitSphere * shakerAmount;
		shake -= Time.deltaTime * decrease;

		//shaking = false;

	}

	public void EndQuake ()
	{
		shake += 1;
		camTransform.localPosition = originalpos;

		//shaking = true;
	}
}