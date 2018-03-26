using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public interface ICustomMove
	{
		void Move(Transform target = null);
		void PerformMovement(); 
	}

	public class ActiveEnemyUnit : EnemyUnit, ICustomMove
	{
		//	Shared Enemies Data - Customizable via SO
		[SerializeField]
		private SaucerSharedData sharedData;

		//	Shooting Handler
		public EnemyShooter EnemyShooter; 

		//	Common properties (Enemy)
		override public int Health { get { return sharedData.Health; } }
		override public int DestructionScore { get { return sharedData.DestructionScore; } }
		override public int Damage { get { return sharedData.Damage; } }

		//	Active Enemy properties
		public float FireRate { get { return sharedData.FireRate; } }
		public float ShootSpeed { get { return sharedData.ShootSpeed; } }
		public float MoveSpeed { get { return sharedData.MoveSpeed; } }

		// Movement variables
		protected Transform target;
		public bool StopMove = true;

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

		virtual public void FixedUpdate() { PerformMovement(); }

		#endregion
		#region Enemy Behaviours
		public virtual void PerformMovement()
		{
			throw new NotImplementedException("PerformMovement methods must be implemented by child classes");
		}

		public virtual void Move(Transform target = null)
		{
			this.target = target;
			StopMove = false;
			Aim(); 
		}

		protected virtual void Aim()
		{
			//	Set Up the Shooter 
			EnemyShooter.BulletSpawnPoint = transform.Find("BulletSpawnPoint");
			EnemyShooter.FireRate = FireRate;
			EnemyShooter.ShootSpeed = ShootSpeed;
			EnemyShooter.Target = target;

			//	Enable Shooting
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

			//	Disable Shooting
			EnemyShooter.StopShooting = true;

		}
		#endregion
	}
}

