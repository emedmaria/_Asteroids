using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInputPress
{
	 bool IsPressed();
	 InputControl GetControlHeld();
}

public class InputControl {

	public enum InputControlType
	{
		Button,
		Axis
	}

	private bool m_Enabled;
	private InputControlType type; 
	public InputControlType Type
	{
		get { return type; }
		set { type = value; }
	}
	public string Name { get; set; }
	public bool IsControlEnabled() { return m_Enabled;}
	public void SetControlEnabled(bool enabled) { m_Enabled = enabled; }
}

public class ButtonInputControl: InputControl, IInputPress
{
	private const float BUTTONTRESHOLD = 0.5f;

	private bool isHeld;
	public bool IsHeld { get { return isHeld = IsPressed(); } }

	private KeyCode bindKey;
	private string bindButton = null;

	public ButtonInputControl(KeyCode bindKey){ this.bindKey = bindKey;}
	public ButtonInputControl(string bindButton) { this.bindButton = bindButton;}

	public bool IsPressed()
	{
		if (!IsControlEnabled()) return false; 
		isHeld = ((bindButton!=null && Input.GetButtonDown(bindButton) )|| Input.GetKeyDown(bindKey));
		return isHeld;
	}

	public InputControl GetControlHeld() { return isHeld ? this : null; }
}

public class AxisInputControl : InputControl, IInputPress
{
	private bool isHeld;
	public bool IsHeld { get { return isHeld = IsPressed(); } }

	private float axisValue;
	public float AxisValue { get { { return axisValue = Input.GetAxis(axis); } } }

	private string axis;

	public AxisInputControl(string axis) { this.axis = axis; }

	public bool IsPressed()
	{
		if (!IsControlEnabled()) return false;
		axisValue = Input.GetAxis(axis);
		isHeld = (axisValue != 0);
		return isHeld;
	}

	public InputControl GetControlHeld() { return isHeld ? this : null; }
}

