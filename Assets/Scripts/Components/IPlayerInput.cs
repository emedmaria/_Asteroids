using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public interface IPlayerInput
	{
        //Create Keycodes  associated with each of our commands.
         KeyCode Turn { get; set; }
         KeyCode Thrust { get; set; }
         KeyCode Fire { get; set; }


        float GetAxis(string axisName);
		float GetKeyDown(KeyCode key);
	}
}

