using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimBarShovel : AimBarToolBase
{
	[Header("Pre-defined ranges")]
	public float good_aim_nomatch_range;
	public float good_aim_match_range;
	
	int round_hit; // In 1 swipe, this counts the numer of times the aim hit those green bars

    public override void UpdateAim()
	{
		foreach (var i in good_aims)
		{
			if (AimBarCore.main.current_block == "dirt") i.range_length = good_aim_match_range;
			else i.range_length = good_aim_nomatch_range;
		}
		BarRestart(); // Most function needed is in BarRestart(), so might as well use it.
	}
	
	public override void BarRestart()
	{
		round_hit = 0;
		float max_offset = AimBarCore.main.aim_range_max * 0.2f;
		foreach (var i in good_aims)
		{
			i.range_offset = Random.Range(-max_offset, max_offset); // This means the offset at max moves each bar 70% away from center to edge
		}
		UpdateAimData();
	}
	
	public override void CheckAim(float stopped_range)
	{
	
	}
}
