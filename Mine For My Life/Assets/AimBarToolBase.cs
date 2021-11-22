using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AimBarToolBase : MonoBehaviour
{
/*
	Notes:
	1. If good and ok ranges overlap, we check the good first, then ok.
	2. These are parallel lists. Make sure the lengths match.
*/

	[Header("OK Aim Data")]
	public List<AimData> ok_aims;
	
	[Header("Good Aim Data")]
	public List<AimData> good_aims;
	
	public virtual void UpdateAim() {}
	public virtual void BarRestart() {}
	
	public void UpdateAimData()
	{
		foreach (var i in ok_aims.Concat(good_aims))
		{
			print("A");
			RectTransform rect = i.img.GetComponent<RectTransform>();
			print("B");
			rect.anchoredPosition = new Vector2(i.range_offset,0f);
			print("C");
			rect.sizeDelta = new Vector2(i.range_length,rect.sizeDelta.y);
			print(i.range_offset);
			print(i.range_value);
			float[] a = new float[2];
			print(a[0]);
			print("meh");
			i.range_value[0] = 500 - i.range_length / 2 + i.range_offset;
			i.range_value[1] = 500 + i.range_length / 2 + i.range_offset;
			print("E");
		}
	}
	
	public virtual void CheckAim(float stopped_range)
	{
		
	}
	
	
	[System.Serializable]
	public class AimData
	{
		public Image img;
		[HideInInspector] public float range_length;
		[HideInInspector] public float range_offset; // From center
		[HideInInspector] public float[] range_value = new float[2]; // Auto-updates from UpdateAimData()
	}
}
