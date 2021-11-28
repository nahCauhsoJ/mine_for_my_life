using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBarHand : AimBarToolBase
{
    [Header("Target Aim Data")]
    public List<AimData> target_aims;

    [Header("Pre-defined ranges")]
    public float max_aim_range;
    public float min_aim_range;

    [Header("Hand Aim Presets")]
	public Color target_good_color;
	public Color target_ok_color;
	public Color target_bad_color;
    public Color target_swipe_tint;
    public Color target_miss_tint;
    public float out_of_range_offset; // If it's going to right, then it's from pin right to bar right, vice versa.

    [Header("Hand Damage Presets")]
    // These numbers are the percentage swiped on the target. 0_30 means deal this damage if (0,30]% of the target is swiped.
    // Remember ranges, () is exclusive and [] is inclusive.
    public float damage_0_30;
    public float damage_30_60;
    public float damage_60_80;
    public float damage_80_100;
    float dmg_mplyr; // Short for damage multiplier, decided by UpdateAimRange().

    bool touched_target; // Bar only resets if player deals damage with the swipe.
    bool missed_target;
    float swipe_start_range; // This marks where the player starts holding key. 0 means unused, no way players can start at 0.

    void OnEnable() { touched_target = true; }

    public override void UpdateAimRange()
	{
        switch (AimBarCore.main.current_block)
        {
            case "web":
                target_aims[0].img.color = target_good_color;
                dmg_mplyr = 1f;
                break;
            case "stone":
                target_aims[0].img.color = target_bad_color;
                dmg_mplyr = 0f;
                break;
            default:
                target_aims[0].img.color = target_ok_color;
                dmg_mplyr = 0.5f;
                break;
        }
	}

    public override void BarRestart()
	{
        if (!touched_target) return; // It only restarts on next round if break attempted, hence this is needed.

        touched_target = false;
        missed_target = false;
        round_ended = false;
        swipe_start_range = 0;
        UpdateAimRange();
        
        if (AimBarCore.main.current_block == "stone") { MissTarget(); target_aims[0].range_length = max_aim_range; }
        else target_aims[0].range_length = Random.Range(min_aim_range, max_aim_range);
		target_aims[0].range_offset = (1000f - target_aims[0].range_length) / 2 * AimBarCore.main.aim_range_modifier;
        target_aims[1].range_length = 0f;
        
        UpdateAimData(target_aims);
    }

    public override void CheckAim(float pin_min, float pin_max)
	{   
        if (round_ended) return;

        // It marks pin's left side if going left, vice versa.
        if (!PinAnimator.main.move_side)
        {
            swipe_start_range = pin_max;
            if (target_aims[0].range_value[0] > pin_max || target_aims[0].range_value[1] < pin_max) MissTarget();
        } else {
            swipe_start_range = pin_min;
            if (target_aims[0].range_value[0] > pin_min || target_aims[0].range_value[1] < pin_min) MissTarget();
        }

        touched_target = true;
    }

    public override void CheckAimRelease(float pin_min, float pin_max)
	{   
        if (round_ended || swipe_start_range == 0) return;

        // Players can hold all the way till stop point, making this false upon restart, hence this extra.
        // Also when block breaks via SwipeTotal(), touched_target must be true beforehand to trigger
        //      BarRestart(), hence it needs to be put before SwipeTotal.
        touched_target = true;
        if (!missed_target)
            if (!PinAnimator.main.move_side)
            {
                if (target_aims[0].range_value[1] >= pin_max) SwipeTotal((pin_max - swipe_start_range) / target_aims[0].range_length);
                else MissTarget();
            } else {
                if (target_aims[0].range_value[0] <= pin_min) SwipeTotal((swipe_start_range - pin_min) / target_aims[0].range_length);
                else MissTarget();
            }

        swipe_start_range = 0;
        round_ended = true;
    }

    public override void ManualUpdate()
    {
        if (swipe_start_range != 0)
        {
            float swipe_length = 0f;
            if (!PinAnimator.main.move_side)
            {
                swipe_length = AimBarCore.main.aim_pin_range[1] / AimBarCore.main.aim_range_modifier - swipe_start_range;
                target_aims[1].range_offset = swipe_start_range;
                target_aims[1].range_length = swipe_length;
                if (AimBarCore.main.aim_pin_range[1] >= 1000f - out_of_range_offset)
                    CheckAimRelease(AimBarCore.main.aim_pin_range[0], AimBarCore.main.aim_pin_range[1]);
            } else {
                swipe_length = swipe_start_range - AimBarCore.main.aim_pin_range[0] / AimBarCore.main.aim_range_modifier;
                target_aims[1].range_offset = AimBarCore.main.aim_pin_range[0];
                target_aims[1].range_length = swipe_length;
                if (AimBarCore.main.aim_pin_range[1] <= out_of_range_offset)
                    CheckAimRelease(AimBarCore.main.aim_pin_range[0], AimBarCore.main.aim_pin_range[1]);
            }
            if (!missed_target) target_aims[1].img.color = Color.Lerp(
                Color.clear, target_swipe_tint, swipe_length / target_aims[0].range_length);
            UpdateAimData(target_aims.GetRange(1,1));
        }
    }

    // Am just lazy.
    void MissTarget()
    {
        target_aims[1].img.color = target_miss_tint;
        missed_target = true;
    }

    // percent: 0f to 1f, it should be the percentage of swipe length / target length.
    void SwipeTotal(float percent)
    {
        switch (percent)
        {
            case > 0.8f: AimBarCore.main.DamageBlock(damage_80_100 * dmg_mplyr); break;
            case > 0.6f: AimBarCore.main.DamageBlock(damage_60_80 * dmg_mplyr);  break;
            case > 0.3f: AimBarCore.main.DamageBlock(damage_30_60 * dmg_mplyr);  break;
            case > 0f:  AimBarCore.main.DamageBlock(damage_0_30 * dmg_mplyr);   break;
        }
    }
}
