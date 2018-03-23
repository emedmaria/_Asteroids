using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AsteroidsClone {

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

		private Dictionary<ActionType, List<InputControl>> m_actionMapInputControls;

		public PlayerInputControls()
		{
			//	Mapping of Actions with InputControls
			m_actionMapInputControls = new Dictionary<ActionType, List<InputControl>>(); 
		}

		public void BindButtonInputAction(ActionType actionType, KeyCode key, bool enabled = false)
		{
			var buttonInputControl = new ButtonInputControl(key);
			buttonInputControl.SetControlEnabled(enabled);

			//	Add inputControl to an Action Type
			List<InputControl> inputControls; 
			if(m_actionMapInputControls.TryGetValue(actionType, out inputControls)){

				if (inputControls == null) inputControls = new List<InputControl>();
				inputControls.Add(buttonInputControl);
			}
			else
			{
				inputControls = new List<InputControl>();
				inputControls.Add(buttonInputControl);
				m_actionMapInputControls[actionType] = inputControls;
			}
		}

		public void BindButtonInputAction(ActionType actionType, string buttonName, bool enabled = false)
		{
			var buttonInputControl = new ButtonInputControl(buttonName);
			buttonInputControl.SetControlEnabled(enabled);

			//	Add inputControl to an Action Type
			List<InputControl> inputControls;
			if (m_actionMapInputControls.TryGetValue(actionType, out inputControls))
			{
				if (inputControls == null) inputControls = new List<InputControl>();
				inputControls.Add(buttonInputControl);
			}
			else
			{
				inputControls = new List<InputControl>();
				inputControls.Add(buttonInputControl);
				m_actionMapInputControls[actionType] = inputControls;
			}
		}

		public void BindAxisInputAction(ActionType actionType, string axisName, bool enabled = false)
		{
			var axisNameInputControl = new AxisInputControl(axisName);
			axisNameInputControl.SetControlEnabled(enabled);

			//	Add inputControl to an Action Type
			List<InputControl> inputControls;
			if (m_actionMapInputControls.TryGetValue(actionType, out inputControls))
			{

				if (inputControls == null) inputControls = new List<InputControl>();
				inputControls.Add(axisNameInputControl);
			}
			else
			{
				inputControls = new List<InputControl>();
				inputControls.Add(axisNameInputControl);
				m_actionMapInputControls[actionType] = inputControls;
			}
		}

		public bool ActiveActionMaps()
		{
			return m_actionMapInputControls != null && m_actionMapInputControls.Count > 0;
		}

		public List<InputControl> GetInputControlList(ActionType actionType)
		{
			if (m_actionMapInputControls == null) return null;
			var inputControlList = m_actionMapInputControls[actionType];
			return inputControlList;
		}

		InputControl GetActionInputControlByName(ActionType actionType, string name)
		{
			List<InputControl> inputControlList = GetInputControlList(actionType);
			var result = (inputControlList != null ? inputControlList.FirstOrDefault(s => s.Name.Equals(name)):null);
			return result;
		}

		public void SetEnableActionType(ActionType actionType, bool enabled)
		{
			List<InputControl> inputControlList = GetInputControlList(actionType);
			if (inputControlList == null) return;

			int nControls = inputControlList.Count;

			for (int i = 0; i < nControls; i++)
				inputControlList[i].SetControlEnabled(enabled);
		}

		//	Player Button Input bindings 
		public bool IsActionActive(ActionType actionType)
		{
			List<InputControl> inputControlList = GetInputControlList(actionType);
			var result = (inputControlList != null ? inputControlList.FirstOrDefault(s => (s as IInputPress).IsPressed().Equals(true)) : null);
			return (result != null);
		}

		public InputControl GetActiveInputControl(ActionType actionType)
		{
			List<InputControl> inputControlList = GetInputControlList(actionType);
			var result = (inputControlList != null ? inputControlList.FirstOrDefault(s => (s as IInputPress).GetControlHeld() != null) : null);
			return (result != null ? result as InputControl : null);
		}

		#region - Current Player Input Actions
		public bool Fire {
			get { return IsActionActive(ActionType.Fire); }
		}

		public InputControl InputControlFire{
			get { return GetActiveInputControl(ActionType.Fire);}
		}

		public bool Thurst {
			get { return IsActionActive(ActionType.Thurst); }
		}

		public AxisInputControl InputControlThurst{
			get { return GetActiveInputControl(ActionType.Thurst) as AxisInputControl; }
		}

		public bool Turn
		{
			get { return IsActionActive(ActionType.Turn); }
		}

		public AxisInputControl InputControlTurn
		{
			get { return GetActiveInputControl(ActionType.Turn) as AxisInputControl; }
		}
		#endregion
	}
}
