using System;
using UnityEngine;

namespace AsteroidsClone
{
	public class Player : EntityBehaviour
	{
		private Rigidbody m_rb;
		private BaseShipMovement m_baseShipMovement;
		private BaseShooter m_baseShooter;

		#region - Events to suscribe with
		public event EventHandler LivedDecreased;
		protected virtual void OnLivedDecreased(object sender, EventArgs e)
		{
			if (LivedDecreased != null)
				LivedDecreased(this, EventArgs.Empty);
		}

		#endregion

		#region - MonoBehaviour methods
		void Awake()
		{
			//	Save references
			m_rb = GetComponent<Rigidbody>();
			m_baseShipMovement = GetComponent<BaseShipMovement>();
			m_baseShooter = GetComponent<BaseShooter>();

			//	Reset components
			ResetTransform();
			ResetRigidbody();
		}

		void OnDestroy()
		{
			//m_collisionDetection.PlayerCollided -= OnPlayerCollided;
			//m_collisionDetection.PlayerGetsShot -= OnPlayerGetsShot;
		}

		#endregion

		/// <summary>
		/// Creates an instance of itself given a prefab
		/// </summary>
		/// <param name="prefab"></param>
		/// <returns></returns>
		/// 
		public static Player Spawn(GameObject prefab, Transform parent = null)
		{
			GameObject playerClone = Instantiate(prefab, parent);
			Player player = playerClone.GetComponent<Player>();
			return player ? player : playerClone.AddComponent<Player>();
		}

		public override GameObject CreateClone(GameObject prefab, Transform parent = null)
		{
			GameObject clone = base.CreateClone(prefab, parent);
			Player clonePlayer = clone.GetComponent<Player>();
			if (clonePlayer == null)  clone.AddComponent<Player>();
			return clone;
		}

		public override void Spawn() { }

		public override void Recover()
		{
			if (!IsAlive)
			{
				ResetTransform();
				gameObject.SetActive(true);
				ResetRigidbody();
			}
		}

		public void ResetTransform()
		{
			transform.position = Vector3.zero;
			transform.rotation = Quaternion.identity;
		}

		public void ResetRigidbody() {

			m_rb.position = Vector3.zero;
			m_rb.rotation = Quaternion.identity;
			m_rb.velocity = Vector3.zero;
			m_rb.angularVelocity = Vector3.zero;
		}

		public override void EnableControls()
		{
			m_baseShipMovement.enabled = true;
			m_baseShooter.enabled = true; 
		}

		public override void DisableControls()
		{
			m_baseShipMovement.enabled = true;
			m_baseShooter.enabled = true;
		}

		public override void Destruction()
		{
			DisableControls();
			gameObject.SetActive(false);
		}

		public override void Collide(int damage =1)
		{
			Detonator.Instance.Explode(ExplosionType.Player, gameObject.transform.position);
			Destruction();
		
			OnLivedDecreased(this, EventArgs.Empty); 
		}
	}

}

