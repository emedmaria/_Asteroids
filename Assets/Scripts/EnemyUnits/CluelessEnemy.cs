using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class CluelessEnemy : ActiveEnemyUnit, ICustomMove
	{
		private float m_wanderRadiusMin = 5f;
		private float m_wanderRadiusMax = 20f;


		override public void PerformMovement()
		{
			if (StopMove) return;
			m_rb.MovePosition(ChooseRandomDestination() * MoveSpeed * Time.fixedDeltaTime);
		}

		private Vector3 ChooseRandomDestination()
		{
			var wanderRadius = UnityEngine.Random.Range(m_wanderRadiusMin, m_wanderRadiusMax);
			Vector2 randomCirclePoint = UnityEngine.Random.insideUnitCircle * wanderRadius;
			Vector3 destination = target.position + new Vector3(randomCirclePoint.x, transform.position.y, 0);
			return destination;
		}
	}
}
