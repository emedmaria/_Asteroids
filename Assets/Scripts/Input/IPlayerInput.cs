using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public interface IPlayerInput
	{
        float GetAxis(string axisName);
		bool GetKeyDown(KeyCode key);
		bool GetButton(string buttonName);
	}
}

