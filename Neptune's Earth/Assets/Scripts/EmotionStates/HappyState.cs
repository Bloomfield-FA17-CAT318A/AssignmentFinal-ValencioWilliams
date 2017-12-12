using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyState : IEmotions
{

	public Boss boss;

	public List<Enemy> minions;

	public void Enter (Boss boss)
	{
		this.boss = boss;
		minions = boss.listOfSpawnedMinions;
		boss.earth.on = true;

		ResetVariables ();

		ResetDpsChecker ();
	}


	public void Action ()
	{
		EffectMagnitude ();
		EmotionCheck ();
	}

	public void End ()
	{
		boss.MyAnimator.ResetTrigger ("HappyTrigger");
	}

	public void EffectMagnitude ()
	{
		boss.MyAnimator.SetTrigger ("HappyTrigger");
		boss.magnitudeRate = 1.5f;
		boss.repeatRate = 2f;
		boss.earth.shakeAmount = 0.1f;
		boss.earth.shakeRate = 1f;

		foreach (Enemy minion in minions)
		{
			minion.ChangeSpeed (5f);
		}

	}

	public void EmotionCheck ()
	{
		if (boss.timeSinceLastFed > boss.enrageTime)
		{
			End ();
			boss.phaseSounds.Play ();
			boss.ChangeEmotion (new AngryState ());
		}

		if (boss.hunger.CurrentVal >= boss.hunger.MaxVal)
		{
			End ();
			boss.phaseSounds.Play ();
			boss.ChangeEmotion (new SleepState ());
		}
	}

	void ResetDpsChecker()
	{
		boss.DpsMask.SetActive (false);
		boss.dpsBar.CurrentVal = 0;
		boss.dpsChecker.fillAmount = 0;
		boss.currentAmount = 0;

	}

	void ResetVariables()
	{
		boss.timeSinceLastFed = 0;
	}
		

}
