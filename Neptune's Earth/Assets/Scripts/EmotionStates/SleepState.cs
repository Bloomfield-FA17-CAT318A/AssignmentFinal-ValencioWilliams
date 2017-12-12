using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepState : IEmotions
{
	public Boss boss;

	public List<Enemy> minions;

	public float timer;

	public float sleepDuration = 6f;

	//Make another state for when he wakes up and deals damage based on DPS

	public void Enter (Boss boss)
	{
		this.boss = boss;
		minions = boss.listOfSpawnedMinions;
		timer = 0;

		ResetDpsChecker ();
		Crash ();
	}

	public void Action ()
	{
		EffectMagnitude ();
		EmotionCheck ();
	}

	public void End ()
	{
		boss.MyAnimator.ResetTrigger("SleepyTrigger");

	}

	public void EffectMagnitude()
	{
		timer += Time.deltaTime;

		boss.MyAnimator.SetTrigger("SleepyTrigger");

		foreach (Enemy minion in minions)
		{
			minion.ChangeSpeed (15f);
		}

		if (timer > 2f)
		{
			boss.magnitudeRate = 0f;
			boss.earth.shakeAmount = 0f;
			boss.repeatRate = 0f;
			GameController.Instance.spawnRate = 0.9f;
		}



	}

	public void EmotionCheck()
	{
		if(timer >= sleepDuration)
		{
			if (boss.dpsBar.CurrentVal < boss.dpsBar.MaxVal)
			{
				End ();
				boss.hunger.CurrentVal = 0;
				boss.magnitude.CurrentVal += 30;
				boss.phaseSounds.Play ();
				boss.ChangeEmotion (new AngryState ());
				Debug.Log ("WAKE UP ANGRY");
			} else
			{
				End ();
				boss.hunger.CurrentVal = 0;
				boss.magnitude.CurrentVal -= 30;
				boss.phaseSounds.Play ();
				boss.ChangeEmotion (new HappyState ());
				Debug.Log ("WAKE UP HAPPY");
			}
		}
			

	}

	void Crash()
	{
		boss.magnitudeRate = 10;
		boss.earth.shakeAmount = 4f;
		boss.repeatRate = 4f;

		boss.earth.TriggerQuake (4f);

	}

	void ResetDpsChecker()
	{
		boss.dpsBar.CurrentVal = 0;
		boss.DpsMask.SetActive (true);
		boss.dpsChecker.fillAmount = 0;
		boss.currentAmount = 0;

	}

}