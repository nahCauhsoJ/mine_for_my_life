using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimBarPickaxe : AimBarToolBase
{
	[Header("Pre-defined ranges")]
	public float ok_aim_nomatch_range;
	public float ok_aim_match_range;
	public float good_aim_nomatch_range;
	public float good_aim_match_range;

    public override void UpdateAim()
	{
		if (AimBarCore.main.current_block == "stone")
		{
			ok_aims[0].range_length = ok_aim_match_range;
			good_aims[0].range_length = good_aim_match_range;
		} else {
			ok_aims[0].range_length = ok_aim_nomatch_range;
			good_aims[0].range_length = good_aim_nomatch_range;
		}
		UpdateAimData();
	}
	
	public override void BarRestart()
	{
		AimBarCore.main.aim_pin_animator.SetFloat("speed_multiplier",Random.Range(0.7f,1.3f));
	}
	
	public override void CheckAim(float stopped_range)
	{
		if (good_aims[0].range_value[0] <= stopped_range && stopped_range <= good_aims[0].range_value[1])
		{
			print("good aim");
		} else if (ok_aims[0].range_value[0] <= stopped_range && stopped_range <= ok_aims[0].range_value[1])
		{
			print("ok aim");
		} else {
			print("bad aim");
		}
	}
}
