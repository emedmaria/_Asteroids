using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{

	public abstract class Command
	{
		public abstract void Execute(Transform boxTrans, Command command);
	}

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
	public class Turn : Command
	{
		public override void Execute(Transform boxTrans, Command command)
		{
			//	Do nothing
		}
	}

	public class Move : Command
	{
		public override void Execute(Transform boxTrans, Command command)
		{
			//	Do nothing
		}
	}

	public class Fire : Command
	{
		public override void Execute(Transform boxTrans, Command command)
		{
			//	Do nothing
		}
	}
}
