using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimBarToolBase : MonoBehaviour
{
	protected bool round_ended;

	public virtual void ManualUpdate() {}
	public virtual void BarRestart() {}
	public virtual void UpdateAimRange() {}
	public virtual void CheckAim(float pin_min, float pin_max) {} // Runs on press.
	public virtual void CheckAimRelease(float pin_min, float pin_max) {} // Runs on release.

	protected void UpdateAimData(List<AimData> aims)
	{
		foreach (var i in aims)
		{
			RectTransform rect = i.img.rectTransform;
			rect.anchoredPosition = new Vector2(i.range_offset / AimBarCore.main.aim_range_modifier,0f);
			rect.sizeDelta = new Vector2(i.range_length,rect.sizeDelta.y);
			i.range_value = new float[]{i.range_offset, i.range_offset + i.range_length};
		}
	}

	[System.Serializable]
	public class AimData
	{
		public Image img;
		[HideInInspector] public float range_length;
		[HideInInspector] public float range_offset; // From left
		[HideInInspector] public float[] range_value; // Auto-updates from UpdateAimData()
	}
}
