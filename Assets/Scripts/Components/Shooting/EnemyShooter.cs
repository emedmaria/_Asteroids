using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class EnemyShooter : MonoBehaviour {

		const int MAX_AMMUNITION = 4;
		const float AMMU_SPEED = 45f;
		const float DEFAULT_FIRERATE = 30f;

		private float m_nextFire = 0.0f;

		private float fireRate; 
		public float FireRate
		{
			set { fireRate = value;  }
		}

		private bool stopShooting = true; 
		public bool StopShooting
		{
			set { stopShooting = value; }
		}

		private Transform target;
		public Transform Target
		{
			set { target = value; }
		}

		[SerializeField]
		private Transform bulletSpawnPoint;
		public Transform BulletSpawnPoint
		{
			set { bulletSpawnPoint = value; }
		}

		private Rigidbody m_rb;

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
			PoolManager.BuildPool(ammunitionPrefab, MAX_AMMUNITION);
		}

		void Update()
		{
			if (stopShooting) return;

			if (Time.time > m_nextFire)
			{
				m_nextFire = Time.time + fireRate;

				Vector3 direction = (target.position - bulletSpawnPoint.position).normalized;
				float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
				Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

				var ammunitionClone = PoolManager.SpawnObject(ammunitionPrefab);
				var ammuTransform = ammunitionClone.transform;
				ammuTransform.position = bulletSpawnPoint.position;
				ammuTransform.rotation = q;


				IShootable shootable = ammunitionClone.GetComponent<IShootable>();
				shootable.Source = bulletSpawnPoint;
				AudioManager.Instance.PlaySFX(soundFXType);
				FireAmmunition(shootable, direction);
			}
			
		}
		#endregion

		void FireAmmunition(IShootable ammu, Vector3 direction, float speed = AMMU_SPEED)
		{
			//Vector3 direction = bulletSpawnPoint.up;
			direction = (direction * speed) + m_rb.velocity;
			ammu.Fire(m_rb.position, m_rb.rotation, direction);
		}
	}
}
