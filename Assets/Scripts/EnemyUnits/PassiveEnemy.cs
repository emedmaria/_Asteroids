using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone { 

	/// <summary>
	/// Enemies that not attackt 
	/// </summary>
	public class PassiveEnemy : EnemyUnit {

		[SerializeField]
		private AsteroidSharedData sharedData;

		override public int Health { get { return sharedData.Health; } }
		override public int DestructionScore { get { return sharedData.DestructionScore; } }
		override public int Damage { get { return sharedData.Damage; } }

		public int NumBits { get { return sharedData.NumBits; } }
		protected float MinScale { get { return sharedData.MinScale; } }
		protected float MaxScale { get { return sharedData.MaxScale; } }
		protected float InitForce { get { return sharedData.InitForce; } }
		protected float InitRotation { get { return sharedData.InitRotation; } }

		#region - Events to suscribe with
		public event EventHandler AsteroidShatter;
		protected void OnAsteroidShatter(object sender, EventArgs e)
		{
			if (AsteroidShatter != null)
				AsteroidShatter(sender, e);
		}
        #endregion


        #region Spawning
        public override void Spawn()
		{
			ApplySpawnVariation();
            base.Spawn();
		}

        public override void SpawnAt(Vector3 position)
        {
            ApplySpawnVariation();
            base.SpawnAt(position); 
        }

        protected Vector3 CalculateRandomScale(float min, float max)
        {
            float scaleFactor = UnityEngine.Random.Range(min, max);
            return new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }

        protected virtual void ApplySpawnVariation()
        {
            transform.localScale = CalculateRandomScale(MinScale, MaxScale);

            if (m_rb != null)
            {
                SetRandomForce(m_rb, InitForce);
                SetRandomRotation(m_rb, InitRotation);
            }
        }

        private void SetRandomForce(Rigidbody rb, float maxForce)
        {
            Vector3 randomForce = maxForce * UnityEngine.Random.insideUnitSphere;
            rb.velocity = Vector3.zero;
            rb.AddForce(randomForce);
        }

        private void SetRandomRotation(Rigidbody rb, float maxTorque)
        {
            Vector3 randomTorque = maxTorque * UnityEngine.Random.insideUnitSphere;
            rb.angularVelocity = Vector3.zero;
            rb.AddTorque(randomTorque);
        }
        #endregion

        public override void Collide(int damage = 1)
        {
            //	Explosion
            base.Collide();

            if (currentHealth > 0)
                OnAsteroidShatter(this, EventArgs.Empty);

			currentHealth -= damage;

            Destruction();
        }
    }
}
