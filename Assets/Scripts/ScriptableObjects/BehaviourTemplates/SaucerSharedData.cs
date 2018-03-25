using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	[CreateAssetMenu(fileName = "Saucer Template", menuName = "__Asteroids/Shared Data/Saucer Template")]
	public class SaucerSharedData : EnemySharedData
	{
		[SerializeField]
		[Header("Appearance Interval")]
		private float spawnIntervalTs = 4f;
		public float SpawnIntervalTs { get { return spawnIntervalTs; } }

		[SerializeField]
		[Header("Shooting Delay")]
		private float fireRate = 0.3f;
		public float FireRate { get { return fireRate; } }

		[SerializeField]
		private float initForce = 800f;
		public float InitForce { get { return initForce; } }

		[SerializeField]
		private float initRotation = 80f;
		public float InitRotation { get { return initRotation; } }
	}
}
