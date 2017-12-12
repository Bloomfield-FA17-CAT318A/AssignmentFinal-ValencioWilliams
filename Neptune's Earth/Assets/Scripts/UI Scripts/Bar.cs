using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar: MonoBehaviour
{

	private float fill;

	[SerializeField]
	private float lerpSpeed;

	[SerializeField]
	private float colorLerpSpeed;

	[SerializeField]
	private Image barImage;

	//private Color barBaseColor;

	[SerializeField]
	private Text barText;

	public float MaxValue { get; set; } 

	public float Value
	{
		set
		{
			string[] temp = barText.text.Split (':');

			if (gameObject.name != "DpsCheck")
			{
				barText.text = temp [0] + ":" + value;
			}

			fill = CalculateFillAmount (value, 0, MaxValue, 0, 1);
		}
				

	}

	void Start()
	{
		//barBaseColor = barImage.color;
	}

	void Update ()
	{
		UpdateBar ();

	}

	private void UpdateBar ()
	{
		if (fill != barImage.fillAmount)
		{
			barImage.fillAmount = Mathf.Lerp (barImage.fillAmount, fill, Time.deltaTime * lerpSpeed);
		}

//		if (gameObject.name == "P1 Healthbar" || gameObject.name == "P1 Food Ammo")
//		{
//			barImage.color = Color.Lerp (barBaseColor, Color.red, Time.deltaTime * colorLerpSpeed);
//		}
	}

	private float CalculateFillAmount (float val, float inMin, float inMax, float outMin, float outMax)
	{
		return ((val - inMin) * (outMax - outMin)) / ((inMax - inMin) + outMin);
	}
}
