using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	[RequireComponent (typeof(Rigidbody))]
	public class BaseShooter : MonoBehaviour
	{
		const int MAX_AMMUNITION = 4; 
		const float AMMU_SPEED = 45f;

		[SerializeField]
		protected float FireRate = 0.25f;

		[SerializeField]
		protected Transform m_bulletSpawnTransform;
		private Rigidbody m_rb;

		[SerializeField]
		private SoundFXType soundFXType = SoundFXType.Shot;

		private float m_nextFire = 0.0f;
		private float m_fireRate = 0.25f;

		[SerializeField]
		private GameObject ammunitionPrefab;

		#region MonoBehaviour methods
		void Awake()
		{
			m_rb = GetComponent<Rigidbody>();
			PoolManager.BuildPool(ammunitionPrefab, MAX_AMMUNITION);

			EnableInputControls();
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

		private void OnHandleFireEvent(object sender, PlayerInputEventArgs e)
		{
			// checks the firerate and fires laser from objectpool.
			if (Time.time > m_nextFire)
			{
				m_nextFire = Time.time + m_fireRate;

				var ammunitionClone = PoolManager.SpawnObject(ammunitionPrefab);
				var ammuTransform = ammunitionClone.transform;
				ammuTransform.position = m_bulletSpawnTransform.position;
				ammuTransform.rotation = gameObject.transform.rotation;
				IShootable shootable = ammunitionClone.GetComponent<IShootable>();
				shootable.Source = m_bulletSpawnTransform;
				AudioManager.Instance.PlaySFX(soundFXType);
				FireAmmunition(shootable);
			}
			else {
				m_nextFire = Time.time;
			}

		}

		/*void Update()
		{
			//return Input.GetButtonDown("Fire1");
			if (Input.GetKeyDown(KeyCode.Space))
			{
				// checks the firerate and fires laser from objectpool.
				if (Time.time > m_nextFire)
				{
					m_nextFire = Time.time + m_fireRate;

					var ammunitionClone = PoolManager.SpawnObject(ammunitionPrefab);
					var ammuTransform = ammunitionClone.transform;
					ammuTransform.position = m_bulletSpawnTransform.position;
					ammuTransform.rotation = gameObject.transform.rotation;
					IShootable shootable = ammunitionClone.GetComponent<IShootable>();
					shootable.Source = m_bulletSpawnTransform;
                    AudioManager.Instance.PlaySFX(soundFXType); 
					FireAmmunition(shootable); 
				}
				else {
					m_nextFire = Time.time;
				}
			}
		}*/

		void FireAmmunition(IShootable ammu, float speed = AMMU_SPEED)
		{
			Vector3 direction = m_bulletSpawnTransform.up;
			direction = (direction * speed) + m_rb.velocity;
			ammu.Fire(m_rb.position,m_rb.rotation,direction);
		}
	}
}



