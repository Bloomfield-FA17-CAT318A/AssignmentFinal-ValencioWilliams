using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsTracking : MonoBehaviour {

	#region Singleton
	private static AnalyticsTracking instance;

	public static AnalyticsTracking Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<AnalyticsTracking> ();
			}
			return instance;

		}
	}

	#endregion

	public Player player;

	public void OnEnd()
	{
		Analytics.CustomEvent("Game Info", new Dictionary<string, object> 
		{
			{"Time", Mathf.Round(GameController.Instance.time)},
			{"Player Deaths", player.deathCount}
		});
	}
}