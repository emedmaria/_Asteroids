using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class CollisionDetectedEventArgs : EventArgs
	{
		public EnemyUnit EnemyUnit { get; set; }
		public EntityBehaviour SourceEntity { get; set; }
	}
}
