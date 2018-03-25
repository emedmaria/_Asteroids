using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AsteroidsClone
{
	public abstract class EnemySharedData : ScriptableObject
	{
		[SerializeField]
		[Range(0, 800)]
		protected int destructionScore = 100;               //	Points obtained when is hit
		public int DestructionScore { get { return destructionScore; } }

		[SerializeField]
		protected int health = 1;                           //	Number of hits to permanent destroy it
		public int Health { get { return health; } }

		[SerializeField]
		protected int damage = 1;                           //	Damage caused by its collision
		public int Damage { get { return damage; } }

	}

	[CreateAssetMenu(fileName = "Asteroid Template", menuName = "__Asteroids/Shared Data/Asteroids Template")]
	public class AsteroidSharedData : EnemySharedData
	{
		[SerializeField]
		[Header("Num fragments after collision")]
		[Range(0, 3)]
		private int numBits;
		public int NumBits { get { return numBits; } }

		[SerializeField]
		[Range(0, 3)]
		private float minScale = 0.3f;
		public float MinScale { get { return minScale; } }

		[SerializeField]
		[Range(0, 5)]
		private float maxScale = 0.3f;
		public float MaxScale { get { return maxScale; } }

		[SerializeField]
		private float initForce = 800f;
		public float InitForce { get { return initForce; } }

		[SerializeField]
		private float initRotation = 80f;
		public float InitRotation { get { return initRotation; } }
	}
}

