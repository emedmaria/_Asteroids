using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	/// <summary>
	/// For keys without binding
	/// </summary>
	public class DoNothing : Command
	{
		public override void Execute(Transform boxTrans, Command command)
		{
			//	Do nothing
		}
	}
}
