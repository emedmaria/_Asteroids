using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	class DesktopInputKeys
	{
		KeyCode Turn { get; set; }
		KeyCode Thrust { get; set; }
		KeyCode Fire { get; set; }
	}

	class DesktopInput : IPlayerInput
	{
		//Create Keycodesassociated with each of our commands.
		KeyCode Turn { get; set; }
		KeyCode Thrust { get; set; }
		KeyCode Fire { get; set; }

		public float GetAxis(string axisName) { return Input.GetAxis(axisName); }

		public bool GetButton(string buttonName) { return Input.GetButton(buttonName); }

		public bool GetKeyDown(KeyCode key) { return Input.GetKeyDown(key); }
	}
}