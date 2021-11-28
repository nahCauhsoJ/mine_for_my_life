using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimBarPickaxe : AimBarToolBase
{
	[Header("OK Aim Data")]
	public List<AimData> ok_aims;
	
	[Header("Good Aim Data")]
	public List<AimData> good_aims;

	[Header("Pre-defined ranges")]
	public float ok_aim_nomatch_range;
	public float ok_aim_match_range;
	public float good_aim_nomatch_range;
	public float good_aim_match_range;

	[Header("Pickaxe Damage Presets")]
	public float bad_damage;
	public float ok_damage;
	public float good_damage;

    public override void UpdateAimRange()
	{
		if (AimBarCore.main.current_block == "stone")
		{
			ok_aims[0].range_length = ok_aim_match_range;
			good_aims[0].range_length = good_aim_match_range;
		} else {
			ok_aims[0].range_length = ok_aim_nomatch_range;
			good_aims[0].range_length = good_aim_nomatch_range;
		}
	}

	public override void BarRestart()
	{
		round_ended = false;
		AimBarCore.main.aim_pin_animator.SetFloat("speed_multiplier",Random.Range(0.7f,1.3f));
		UpdateAimRange();
		// Multiplied the modifier to nullify the one in UpdateAimData, making it dead center and ignore pin's width.
		ok_aims[0].range_offset = (1000f - ok_aims[0].range_length) / 2 * AimBarCore.main.aim_range_modifier;
		good_aims[0].range_offset = (1000f - good_aims[0].range_length) / 2 * AimBarCore.main.aim_range_modifier;
		
		UpdateAimData(ok_aims);
		UpdateAimData(good_aims);
	}
	
	public override void CheckAim(float pin_min, float pin_max)
	{
		if (round_ended) return;
		round_ended = true;
		if (good_aims[0].range_value[0] <= pin_max && good_aims[0].range_value[1] >= pin_min)
		{
			AimBarCore.main.DamageBlock(good_damage);
		} else if (ok_aims[0].range_value[0] <= pin_max && ok_aims[0].range_value[1] >= pin_min)
		{
			AimBarCore.main.DamageBlock(ok_damage);
		} else {
			AimBarCore.main.DamageBlock(bad_damage);
		}
	}
}
