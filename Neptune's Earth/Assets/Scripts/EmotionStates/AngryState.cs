using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryState : IEmotions
{
	public Boss boss;

	public List<Enemy> minions;


	public void Enter (Boss boss)
	{
		this.boss = boss;
		minions = boss.listOfSpawnedMinions;
	
		boss.earth.on = true;
		ResetDpsChecker ();
	}

	public void Action ()
	{
		EffectMagnitude ();
		EmotionCheck ();
	}

	public void End ()
	{
		boss.MyAnimator.ResetTrigger("AngryTrigger");

	}

	public void EffectMagnitude()
	{
		boss.MyAnimator.SetTrigger("AngryTrigger");

		boss.magnitudeRate = 10.5f;
		boss.repeatRate = 2f;
		boss.earth.shakeAmount = 0.4f;
		boss.earth.shakeRate = 1f;

		foreach (Enemy minion in minions)
		{
			minion.ChangeSpeed (1f);
		}

	}

	public void EmotionCheck()
	{
		if (boss.hunger.CurrentVal >= boss.hunger.MaxVal)
		{
			End ();
			boss.phaseSounds.Play ();
			boss.ChangeEmotion (new SleepState());
		}
	}

	void ResetDpsChecker()
	{
		boss.DpsMask.SetActive (false);
		boss.dpsBar.CurrentVal = 0;
		boss.dpsChecker.fillAmount = 0;
		boss.currentAmount = 0;

	}

		
}
