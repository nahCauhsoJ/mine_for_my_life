using UnityEngine;

public class PinAnimator : MonoBehaviour
{
	public void ManualRestart()
	{
		AimBarCore.main.current_tool.BarRestart();
	}
}
