using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimBarShovel : AimBarToolBase
{
	[Header("Good Aim Data")]
	public List<AimData> good_aims;

	[Header("Pre-defined ranges")]
	public float good_aim_nomatch_range;
	public float good_aim_match_range;

	[Header("Shovel Aim Presets")]
	public float aim_left_offset; // Values will be flipped if the pin is going left instead.
	public float aim_right_offset;
	public float aim_min_distance; // Since there's multiple bars, this decides at least how far they should be apart.
	public Color target_hide_color;
	public Color target_show_color;
	public Color target_hit_color;
	public Color target_miss_color;

	[Header("Shovel Damage Presets")]
	public float[] shovel_combo; // This is the damage according to combo count. Must match the size of good_aim.

	bool?[] target_hits; // BarRestart() will initialize it.
	int cur_target_ix = 0;

	void Awake() { target_hits = new bool?[good_aims.Count]; UpdateAimData(good_aims); }

    public override void UpdateAimRange()
	{
		foreach (var i in good_aims)
		{
			if (AimBarCore.main.current_block == "dirt") i.range_length = good_aim_match_range;
			else i.range_length = good_aim_nomatch_range;
		}
	}
	
	public override void BarRestart()
	{
		round_ended = false;
		target_hits = new bool?[good_aims.Count];
		foreach (var i in good_aims) i.img.color = target_hide_color;
		cur_target_ix = -1;
		NextTarget();

		float max_offset = aim_right_offset;
		float cur_offset = aim_left_offset;
		float rng_offset = cur_offset;
		float step_offset = (max_offset - cur_offset) / good_aims.Count;
		
		// Make it so that it spreads. max_offset is chopped into length pieces, and each is assigned that range.
		foreach (var i in good_aims)
		{
			cur_offset += step_offset;
			float old = rng_offset;
			rng_offset = Random.Range(rng_offset, cur_offset);
			i.range_offset = PinAnimator.main.move_side ? 1000f - rng_offset : rng_offset;
			rng_offset += aim_min_distance;
		}
		UpdateAimRange();
		UpdateAimData(good_aims);
	}
	
	public override void CheckAim(float pin_min, float pin_max)
	{
		if (cur_target_ix >= target_hits.Length) return;
		if (target_hits[cur_target_ix] != null) return;

		if (good_aims[cur_target_ix].range_value[0] <= pin_max && good_aims[cur_target_ix].range_value[1] >= pin_min)
		{ target_hits[cur_target_ix] = true; good_aims[cur_target_ix].img.color = target_hit_color; }
		else
		{ target_hits[cur_target_ix] = false; good_aims[cur_target_ix].img.color = target_miss_color; }
	}

	// Using this instead of Update() becuz we want Update() in AimBarCore to run first, and by execution order, 
	//	 AimBarShovel runs first then AimBarCore. Just in case Script Execution Order in Project Settings causes
	//	 complications and more bugs, let's just define the order here.
	public override void ManualUpdate()
	{
		if (cur_target_ix < target_hits.Length)
		{
			if ( (!PinAnimator.main.move_side && good_aims[cur_target_ix].range_value[1] < AimBarCore.main.aim_pin_range[0]) ||
			(PinAnimator.main.move_side && good_aims[cur_target_ix].range_value[0] > AimBarCore.main.aim_pin_range[1]) )
			{
				if (target_hits[cur_target_ix] == null)
				{
					target_hits[cur_target_ix] = false;
					good_aims[cur_target_ix].img.color = target_miss_color;
				}
				NextTarget();
			}
		} else if (!round_ended) TotalTargetHits();
	}

	void NextTarget()
	{
		cur_target_ix++;
		if (cur_target_ix < target_hits.Length) good_aims[cur_target_ix].img.color = target_show_color;
	}

	void TotalTargetHits()
	{
		round_ended = true;
		int combo_count = 0;
		foreach (var i in target_hits) if (i == true) combo_count++;
		if (combo_count != 0) 
		{
			AimBarCore.main.DamageBlock(shovel_combo[combo_count-1]);
		}
	}
}
