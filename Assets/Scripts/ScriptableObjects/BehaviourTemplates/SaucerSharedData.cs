using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	[CreateAssetMenu(fileName = "Saucer Template", menuName = "__Asteroids/Shared Data/Saucer Template")]
	public class SaucerSharedData : EnemySharedData
	{
		[SerializeField]
		[Header("Shooting Delay")]
		private float fireRate = 4f;
		public float FireRate { get { return fireRate; } }

		[SerializeField]
		[Header("Shooting Speed")]
		private float shootSpeed = 4f;
		public float ShootSpeed { get { return shootSpeed; } }

		[SerializeField]
		[Header("Movement Speed")]
		private float moveSpeed = 2f;
		public float MoveSpeed { get { return moveSpeed; } }

		[SerializeField]
		private float initForce = 800f;
		public float InitForce { get { return initForce; } }

		[SerializeField]
		private float initRotation = 80f;
		public float InitRotation { get { return initRotation; } }
	}
}
