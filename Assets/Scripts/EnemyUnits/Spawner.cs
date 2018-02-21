using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class Spawner :EnemyUnit {

		static int activeCount;
		public static bool ActiveUnits { get { return activeCount > 0; } }

		protected virtual void OnEnable()
		{
			++activeCount;
		}

		protected void OnDisable()
		{
			--activeCount;
		}
	}
}

