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
	public Animator aim_bar_animator;
	public Animator aim_pin_animator;
	public Slider block_hp_bar;
	public Image block_hp_bar_fill;
	
	RectTransform aim_bar_rect;
	RectTransform aim_pin_rect;
	RectTransform aim_pin_stop_rect;
	
	public float aim_range_max{get; private set;} // assigned depending on AimBar's and aim_pin_img's width
	public float aim_range_modifier{get; private set;}
	// Conversion formula:
	// v = pin_x + pin_width * ( pin_x / (bar_width - pin_width) )
	// Simplify:
	// v = pin_x + pin_x * ( pin_width / (bar_width - pin_width) )
	// v = pin_x * ( 1 + pin_width / (bar_width - pin_width) )
	// Hence, modifier = 1 + pin_width / (bar_width - pin_width)
	[HideInInspector] public float aim_pin_value = 0f; // Scaled to 1-1000, instead of centered, it's the left side of the pin.
	[HideInInspector] public float[] aim_pin_range;

	public List<AimBarToolBase> tool_scripts = new List<AimBarToolBase>();
	public Color stone_hp_color;
	public Color dirt_hp_color;
	public Color web_hp_color;

	public AimBarToolBase current_tool{get; private set;}
	public string tool_mode = "pickaxe"; // Available tools: pickaxe, shovel, hand
	public string current_block = "stone"; // Available blocks: stone, dirt, web
	public float block_max_hp = 100f;
	public float block_hp = 100f;
	
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
		aim_range_modifier = 1 + aim_pin_rect.sizeDelta.x / (aim_bar_rect.sizeDelta.x - aim_pin_rect.sizeDelta.x);
		aim_pin_value = 0;
		aim_pin_range = new float[]{aim_pin_value, aim_pin_value + aim_pin_rect.sizeDelta.x};
		
		// These 3 are necessary to temporary initialize the block-breaking mechanic
		current_tool = tool_scripts[0];
		current_tool.gameObject.SetActive(true);
		NewBlock(current_block,100);

		OnStartBar();
    }

    // Update is called once per frame
    void Update()
    {
		aim_pin_value = aim_pin_rect.anchoredPosition.x * aim_range_modifier;
		aim_pin_range[0] = aim_pin_value;
		aim_pin_range[1] = aim_pin_value + aim_pin_rect.sizeDelta.x;
		if (pin_moving) { aim_pin_stop_rect.anchoredPosition = aim_pin_rect.anchoredPosition; }
		current_tool.ManualUpdate();
	}
	
	void NewBlock(string block, float hp)
	{
		current_block = block;
		block_max_hp = hp;
		block_hp = hp;

		switch (block)
		{
			case "stone": block_hp_bar_fill.color = stone_hp_color; break;
			case "dirt": block_hp_bar_fill.color = dirt_hp_color; break;
			case "web": block_hp_bar_fill.color = web_hp_color; break;
		}
		block_hp_bar.value = 1f;

		current_tool.BarRestart();
	}
	
	// If not empty, it'll force that tool.
	public void SwitchToolManual(string tool_mode = null)
	{
		if (string.IsNullOrEmpty(tool_mode))
		{
			switch (this.tool_mode)
			{
				case "pickaxe": tool_mode = "shovel"; break;
				case "shovel": tool_mode = "hand"; break;
				case "hand": tool_mode = "pickaxe"; break;
			}
		}
		StartCoroutine(SwitchToolSequence(tool_mode));
	}

	IEnumerator SwitchToolSequence(string tool_mode)
	{
		aim_bar_animator.SetTrigger("switch");
		aim_pin_animator.SetInteger("mode",-1);
		aim_pin_animator.SetTrigger("move");
		yield return new WaitForSeconds(0.125f);

		aim_pin_animator.SetFloat("speed_multiplier",1f);
		if (current_tool != null) current_tool.gameObject.SetActive(false);
		switch (tool_mode)
		{
			case "pickaxe": aim_pin_animator.SetInteger("mode",0); current_tool = tool_scripts[0]; break;
			case "shovel": aim_pin_animator.SetInteger("mode",1); current_tool = tool_scripts[1]; break;
			case "hand": aim_pin_animator.SetInteger("mode",2); current_tool = tool_scripts[2]; break;
		}
		this.tool_mode = tool_mode;
		current_tool.gameObject.SetActive(true);
		current_tool.BarRestart();
	}

	// press: true = press down, false = release
	public void BreakBlockManual(bool press)
	{
		if (!pin_moving) return;
		switch (tool_mode)
		{
			case "pickaxe":
				if (press) current_tool.CheckAim(aim_pin_range[0], aim_pin_range[1]);
				break;
			case "shovel":
				if (press) current_tool.CheckAim(aim_pin_range[0], aim_pin_range[1]);
				break;
			case "hand":
				if (press) current_tool.CheckAim(aim_pin_range[0], aim_pin_range[1]);
				if (!press) current_tool.CheckAimRelease(aim_pin_range[0], aim_pin_range[1]);
				break;
		}
	}

	public void DamageBlock(float dmg)
	{
		block_hp -= dmg;
		block_hp_bar.value = block_hp / block_max_hp;
		if (block_hp <= 0)
		{
			OnStopBar();
			StartCoroutine(RestartBar());
		}
	}
	
	public void OnStopBar()
	{
		aim_pin_animator.SetTrigger("stop");
		pin_moving = false;
	}
	
	public void OnStartBar()
	{
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
		NewBlock("stone",100f);
		OnStartBar();
	}
	
	
	// Used by Input System
	public void BreakBlock(InputAction.CallbackContext ctx)
	{
		if (!pin_moving) return;
		if (ctx.performed) BreakBlockManual(true);
		else if (ctx.canceled) BreakBlockManual(false);
	}

	public void SwitchTool(InputAction.CallbackContext ctx)
	{
		if (!ctx.performed) return;
		SwitchToolManual();
	}
}
