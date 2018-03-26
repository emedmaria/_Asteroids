using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class ChaserEnemy : ActiveEnemyUnit
	{
		// Movement variables
		private Vector3 m_direction;

		override public void Move(Transform target = null)
		{
			base.target = target;
			StopMove = false;
			Aim();
		}

		override public void PerformMovement()
		{
			if (StopMove || target == null) return;

			//	Recalculate direction of movement
			m_direction = (target.position - transform.position).normalized;
			m_rb.MovePosition(m_rb.position + m_direction * MoveSpeed * Time.fixedDeltaTime);
		}
	}
}

