using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{

	public abstract class Command
	{
		public abstract void Execute(Transform boxTrans, Command command);
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


}
