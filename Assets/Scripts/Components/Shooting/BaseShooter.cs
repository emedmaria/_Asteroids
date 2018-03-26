using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	[RequireComponent (typeof(Rigidbody))]
	public class BaseShooter : MonoBehaviour
	{
		//	Default values in case Properties are not set
		const int DEFAULT_MAX_AMMUNITION = 4; 
		const float DEFAULT_AMMU_SPEED = 40f;
		const float DEFAULT_FIRERATE = 0.5f;

		[SerializeField]
		private SoundFXType soundFXType = SoundFXType.Shot;

		//	Shoot Frequency
		private float fireRate;
		public float FireRate {
			get { return (fireRate == 0) ? DEFAULT_FIRERATE : fireRate; }
			set { fireRate = value; }
		}

		//	Speed of the Bullet
		private float shootSpeed;
		public float ShootSpeed {
			get { return (shootSpeed == 0) ? DEFAULT_AMMU_SPEED : shootSpeed; }
			set { shootSpeed = value; }
		}

		private float m_nextFire = 0.0f;

		[SerializeField]
		private GameObject ammunitionPrefab;

		[SerializeField]
		protected Transform m_bulletSpawnTransform;
		private Rigidbody m_rb;

		#region MonoBehaviour methods
		void Awake()
		{
			m_rb = GetComponent<Rigidbody>();
			PoolManager.BuildPool(ammunitionPrefab, DEFAULT_MAX_AMMUNITION);
			//EnableInputControls();
		}

		void Destroy() { DisableInputControls();  }
		void OnEnable() { EnableInputControls(); }
		void OnDisable() { DisableInputControls(); }
		#endregion

		private void EnableInputControls()
		{
			//	Suscribe to Input Events
			PlayerInputHandler.EnableInputControl(PlayerInputControls.ActionType.Fire,true);
			PlayerInputHandler.StartListening(PlayerInputControls.ActionType.Fire, OnHandleFireEvent);

		}
		private void DisableInputControls()
		{
			//	Stop Listening Input Events
			PlayerInputHandler.EnableInputControl(PlayerInputControls.ActionType.Fire, false);
			PlayerInputHandler.StopListening(PlayerInputControls.ActionType.Fire, OnHandleFireEvent);
		}

		private void OnHandleFireEvent(object sender, PlayerInputEventArgs e) { Fire(); }

		private void Fire()
		{
			// checks the firerate and fires laser from objectpool.
			if (Time.time > m_nextFire)
			{
				m_nextFire = Time.time + FireRate;

				var ammunitionClone = PoolManager.SpawnObject(ammunitionPrefab);
				var ammuTransform = ammunitionClone.transform;
				ammuTransform.position = m_bulletSpawnTransform.position;
				ammuTransform.rotation = gameObject.transform.rotation;

				IShootable shootable = ammunitionClone.GetComponent<IShootable>();
				shootable.Source = m_bulletSpawnTransform;

				AudioManager.Instance.PlaySFX(soundFXType);
				FireAmmunition(shootable, ShootSpeed);
			}
		}

		void FireAmmunition(IShootable ammu, float speed = DEFAULT_AMMU_SPEED)
		{
			Vector3 direction = m_bulletSpawnTransform.up;
			direction = (direction * speed) + m_rb.velocity;
			ammu.Fire(m_rb.position,m_rb.rotation,direction);
		}
	}
}



