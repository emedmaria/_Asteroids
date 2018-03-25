using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class ActiveEnemyUnit : EnemyUnit
	{
		//	Shared Enemies Data - Customizable via SO
		[SerializeField]
		private SaucerSharedData sharedData;

		public EnemyShooter EnemyShooter; 

		override public int Health { get { return sharedData.Health; } }
		override public int DestructionScore { get { return sharedData.DestructionScore; } }
		override public int Damage { get { return sharedData.Damage; } }

		public float FireRate { get { return sharedData.FireRate; } }
		protected float SpawnIntervalTs { get { return sharedData.SpawnIntervalTs; } }
		protected float InitForce { get { return sharedData.InitForce; } }
		protected float InitRotation { get { return sharedData.InitRotation; } }

		// Movement variables
		protected Vector3 direction;
		protected float speed = 3f;

		private Transform m_target;
		private bool m_stopMove = true; 

		#region Monobehaviour methods
		void OnValidate()
		{
			//	Ensure prefabs are set 
			Assert.IsNotNull(EnemyShooter, "[ActiveEnemyUnit] EnemyShooter must be referenced in the inspector!");
		}

		override public void Awake()
		{
			//	Ensure prefabs are set 
			Assert.IsNotNull(EnemyShooter, "[ActiveEnemyUnit] EnemyShooter must be referenced in the inspector!");
			base.Awake();
		}

		void FixedUpdate()
		{
			if (m_stopMove || m_target == null) return; 

			direction = (m_target.position - transform.position).normalized;
			m_rb.MovePosition(m_rb.position + direction * speed * Time.fixedDeltaTime);
		}

		#endregion
		#region Enemy Behaviours
		public virtual void Move(Transform target = null)
		{
			m_target = target;
			m_stopMove = false;

			Aim(); 
		}

		protected virtual void Aim()
		{
			//	Set Up the Shooter 
			EnemyShooter.BulletSpawnPoint = transform.Find("BulletSpawnPoint");
			EnemyShooter.FireRate = FireRate;
			EnemyShooter.Target = m_target;
			EnemyShooter.StopShooting = false;
		}
	
		public override void Collide(int damage = 1)
		{
			//	Explosion
			base.Collide();

			//	Apply Damage
			currentHealth -= damage;

			if(currentHealth<=0)
			Destruction();
		}
		#endregion
	}
}

