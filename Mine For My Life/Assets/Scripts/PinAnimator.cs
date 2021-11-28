using UnityEngine;

public class PinAnimator : MonoBehaviour
{
	public static PinAnimator main;
	public bool move_side; // false: going from left to right, true: vice versa

	void Awake() {main = this;}
	public void ManualRestart()
	{
		AimBarCore.main.current_tool.BarRestart();
	}
}
