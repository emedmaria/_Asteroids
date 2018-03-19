using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player Input Action Map 
/// Mouse & Keyboard Schema
/// </summary>
public class PlayerInputControls{

	public enum ActionType
	{
		Fire,
		Thurst,
		Turn
	}

	Dictionary<ActionType, InputControl> buttonInputControls;
	Dictionary<ActionType, InputControl> buttonNameInputControls;

	public PlayerInputControls()
	{
		buttonInputControls = new Dictionary<ActionType, InputControl>();
		//	Bind Keys 
		BindButtonInputAction(ActionType.Fire, KeyCode.Space);
		buttonNameInputControls = new Dictionary<ActionType, InputControl>();
	}

	public void BindButtonInputAction(ActionType actionType, KeyCode key, bool enabled = true)
	{
		var buttonInputControl = new ButtonInputControl(key);
		buttonInputControl.SetControlEnabled(enabled);
		buttonInputControls[actionType] = buttonInputControl;
	}

	public void BindButtonInputAction(ActionType actionType, string buttonName, bool enabled = true)
	{
		var buttonInputControl = new ButtonInputControl(buttonName);
		buttonInputControl.SetControlEnabled(enabled);
		buttonNameInputControls[actionType] = new ButtonInputControl(buttonName);
	}


	//	Player Button Input bindings 
	public ButtonInputControl Fire {
		get { return (ButtonInputControl)(buttonInputControls.ContainsKey(ActionType.Fire)?buttonInputControls[ActionType.Fire]:null); }
	}

	public AxisInputControl Thurst {
		get { return (AxisInputControl)(buttonInputControls.ContainsKey(ActionType.Thurst)?buttonInputControls[ActionType.Thurst]:null); }
	}
	//public ButtonInputControl Turn { get { return buttonInputControls.ContainsKey(ActionType.Turn)?buttonInputControls[ActionType.Turn]:null; } }
}
