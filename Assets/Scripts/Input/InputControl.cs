﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public bool IsControlEnabled() { return m_Enabled;}
	public void SetControlEnabled(bool enabled) { m_Enabled = enabled; }
}

public class ButtonInputControl: InputControl
{
	private const float BUTTONTRESHOLD = 0.5f;

	private bool isHeld;
	public bool IsHeld { get { return isHeld = IsPressed(); } }

	private KeyCode bindKey;
	private string bindButton = null;

	public ButtonInputControl(KeyCode bindKey){ this.bindKey = bindKey;}
	public ButtonInputControl(string bindButton) { this.bindButton = bindButton;}

	private bool IsPressed()
	{
		isHeld = ((bindButton!=null && Input.GetButtonDown(bindButton) )|| Input.GetKeyDown(bindKey));
		return isHeld;
	}
}

public class AxisInputControl : InputControl
{
	private bool isHeld;
	public bool IsHeld { get { return isHeld = IsPressed(); } }

	private float axisValue;
	public float AxisValue { get { { return axisValue = Input.GetAxis(axis); } } }

	private string axis;

	public AxisInputControl(string axis) { this.axis = axis; }

	private bool IsPressed()
	{
		//if (!IsControlEnabled()) return false;
		axisValue = Input.GetAxis(axis);
		isHeld = (axisValue != 0);
		return isHeld;
	}
}

