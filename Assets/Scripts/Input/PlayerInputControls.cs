using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AsteroidsClone {

	public class PlayerInputEventArgs : EventArgs
	{
		public PlayerInputControls.ActionType ActionType { get; set; }
		public InputControl SourceInputControl { get; set; }
	}

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
		Dictionary<ActionType, InputControl> axisNameInputControls;

		public PlayerInputControls()
		{
			//	Bind by KeyCode 
			buttonInputControls = new Dictionary<ActionType, InputControl>();
			
			//	Bind by ButtonName
			buttonNameInputControls = new Dictionary<ActionType, InputControl>();

			//	Bind by AxisName
			axisNameInputControls = new Dictionary<ActionType, InputControl>();
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

		public void BindAxisInputAction(ActionType actionType, string axisName, bool enabled = true)
		{
			var axisNameInputControl = new AxisInputControl(axisName);
			axisNameInputControl.SetControlEnabled(enabled);
			axisNameInputControls[actionType] = new AxisInputControl(axisName);
		}

		public bool ActiveActionMaps()
		{
			return ((buttonInputControls != null && buttonInputControls.Count > 0) ||
				(buttonNameInputControls != null && buttonNameInputControls.Count > 0)||
				(axisNameInputControls != null && axisNameInputControls.Count > 0));
		}

		public InputControl GetInputControl(ActionType actionType)
		{
			if (buttonInputControls.ContainsKey(actionType))
				return buttonInputControls[actionType];

			if (buttonNameInputControls.ContainsKey(actionType))
				return buttonNameInputControls[actionType];

			if (axisNameInputControls.ContainsKey(actionType))
				return axisNameInputControls[actionType];

			return null;
		}

		//	Player Button Input bindings 
		public ButtonInputControl Fire {
			get {
				return (ButtonInputControl)(buttonInputControls.ContainsKey(ActionType.Fire)?buttonInputControls[ActionType.Fire]:null);
			}
		}
		public AxisInputControl Thurst {
			get {
				return (AxisInputControl)(axisNameInputControls.ContainsKey(ActionType.Thurst)? axisNameInputControls[ActionType.Thurst]:null);
			}
		}
		public AxisInputControl Turn
		{
			get {
				return (AxisInputControl)(axisNameInputControls.ContainsKey(ActionType.Turn) ? axisNameInputControls[ActionType.Turn] : null);
			}
		}
	}
}
