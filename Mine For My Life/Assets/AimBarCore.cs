using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AimBarCore : MonoBehaviour
{
	public static AimBarCore main;
	
	public Image bad_aim_img;
	public Image aim_pin_img;
	public Image aim_pin_stop_img;
	public Animator aim_pin_animator;
	
	RectTransform aim_bar_rect;
	RectTransform aim_pin_rect;
	RectTransform aim_pin_stop_rect;
	
	public string tool_mode = "pickaxe"; // Available tools: pickaxe, shovel, hand
	public List<AimBarToolBase> tool_scripts = new List<AimBarToolBase>();
	public AimBarToolBase current_tool{get; private set;} 
	public float aim_range_current = 0f; // Scaled to 1-1000.
	public float aim_range_max{get; private set;} // assigned depending on AimBar's and aim_pin_img's width
	float aim_range_modifier;
	
	public string current_block = "stone"; // Available blocks: stone, dirt, web
	public int block_hp = 100;
	
	bool pin_moving;
	
	void Awake()
	{
		main = this;
		aim_bar_rect = GetComponent<RectTransform>();
		aim_pin_rect = aim_pin_img.GetComponent<RectTransform>();
		aim_pin_stop_rect = aim_pin_stop_img.GetComponent<RectTransform>();
		
	}

    // Start is called before the first frame update
    void Start()
    {
		
        aim_range_max = aim_bar_rect.sizeDelta.x - aim_pin_rect.sizeDelta.x;
		aim_range_modifier = aim_range_max / 1000f;
		
		SwitchTool("shovel");
		NewBlock(current_block,100);
		current_tool.UpdateAimData();
		OnStartBar();
    }

    // Update is called once per frame
    void Update()
    {
		aim_range_current = aim_pin_rect.anchoredPosition.x * aim_range_modifier;
		if (pin_moving) { aim_pin_stop_rect.anchoredPosition = aim_pin_rect.anchoredPosition; }
	}
	
	void NewBlock(string block, int hp)
	{
		current_block = block;
		block_hp = hp;
		current_tool.UpdateAim();
	}
	
	public void SwitchTool(string tool_mode)
	{
		aim_pin_animator.SetFloat("speed_multiplier",1f);
		switch (tool_mode)
		{
			case "pickaxe": aim_pin_animator.SetInteger("mode",0); current_tool = tool_scripts[0]; break;
			case "shovel": aim_pin_animator.SetInteger("mode",1); current_tool = tool_scripts[1]; break;
			case "hand": aim_pin_animator.SetInteger("mode",2); current_tool = tool_scripts[2]; break;
		}
		aim_pin_animator.SetTrigger("move");
		this.tool_mode = tool_mode;
	}
	
	public void OnStopBar()
	{
		aim_pin_animator.SetTrigger("stop");
		pin_moving = false;
		StartCoroutine(RestartBar());
	}
	
	public void OnStartBar()
	{
		current_tool.BarRestart();
		aim_pin_animator.SetTrigger("move");
		pin_moving = true;
	}
	
	IEnumerator RestartBar()
	{
		aim_pin_img.enabled = false;
		aim_pin_stop_img.enabled = true;
		yield return new WaitForSeconds(1f);
		aim_pin_img.enabled = true;
		aim_pin_stop_img.enabled = false;
		OnStartBar();
	}
	
	
	// Used by Input System
	public void StopBar(InputAction.CallbackContext ctx)
	{
		if (!pin_moving) return;
		switch (tool_mode)
		{
			case "pickaxe":
				if (!ctx.performed) return;
				current_tool.CheckAim(aim_range_current);
				OnStopBar();
				break;
			case "shovel":
				if (!ctx.performed) return;
				current_tool.CheckAim(aim_range_current);
				break;
			case "hand":
				current_tool.CheckAim(aim_range_current);
				break;
		}
	}
}
