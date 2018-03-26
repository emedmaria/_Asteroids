using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class EnemyShooter : MonoBehaviour {

		//	Default values if properties are not set
		const int DEFAULT_AMMU_POOLED = 4;
		const float DEFAULT_AMMU_SPEED = 25f;
		const float DEFAULT_FIRERATE = 10f;

		#region Properties
		//	Pool Size
		private int ammuPoolCount;
		public int AmmuPoolCount { set { ammuPoolCount = value; }}

		//	Speed of the Bullet
		private float shootSpeed;
		public float ShootSpeed { set { shootSpeed = value; } }

		//	Shoot Frequency
		private float fireRate; 
		public float FireRate{ set { fireRate = value;  }}

		//	Enable/Disable Shooting
		private bool stopShooting = true; 
		public bool StopShooting { set { stopShooting = value; } }

		//	Target to Shoot
		private Transform target;
		public Transform Target{ set { target = value; }}

		[SerializeField]
		private Transform bulletSpawnPoint;
		public Transform BulletSpawnPoint { set { bulletSpawnPoint = value; } }
		#endregion

		private Rigidbody m_rb;
		private float m_nextFire = 0.0f;

		[SerializeField]
		private GameObject ammunitionPrefab;

		[SerializeField]
		private SoundFXType soundFXType = SoundFXType.Shot;

		#region MonoBehaviour methods
		void Awake()
		{
			//	Ensure prefabs are set 
			Assert.IsNotNull(ammunitionPrefab, "[EnemyShooter] ammunitionPrefab must be referenced in the inspector!");

			m_rb = GetComponent<Rigidbody>();
			PoolManager.BuildPool(ammunitionPrefab, DEFAULT_AMMU_POOLED);
		}

		void Update()
		{
			if (stopShooting) return;

			if (Time.time > m_nextFire)
			{
				m_nextFire = Time.time + fireRate;

				//	Check the direction between target and bulletSpawn point
				Vector3 direction = (target.position - bulletSpawnPoint.position).normalized;
				//	Calculate the angle determined by direction axis. Convert to Degrees
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
				//	Rotates angle degrees around Z axis
				Quaternion qRotation = Quaternion.AngleAxis(angle, Vector3.forward);

				//	Instance ammunition to shoot (from pool)
				var ammunitionClone = PoolManager.SpawnObject(ammunitionPrefab);
				var ammuTransform = ammunitionClone.transform;
				//	Set the ammu position/rotation accordingly
				ammuTransform.position = bulletSpawnPoint.position;
				ammuTransform.rotation = qRotation;

				//	Shoot and Play SFX
				IShootable shootable = ammunitionClone.GetComponent<IShootable>();
				shootable.Source = bulletSpawnPoint;
				
				AudioManager.Instance.PlaySFX(soundFXType);

				FireAmmunition(shootable, direction, shootSpeed==0?DEFAULT_AMMU_SPEED:shootSpeed);
			}
		}
		#endregion

		void FireAmmunition(IShootable ammu, Vector3 direction, float speed = DEFAULT_AMMU_SPEED)
		{
			direction = (direction * speed) + m_rb.velocity;
			ammu.Fire(m_rb.position, m_rb.rotation, direction);
		}
	}
}
