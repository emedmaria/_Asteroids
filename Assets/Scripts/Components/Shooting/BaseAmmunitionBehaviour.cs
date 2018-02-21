using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public interface IShootable
	{
		void Fire(Vector3 position, Quaternion rotation, Vector3 velocity);
		Transform Source { get; set; }
	}

	public interface IRecycle
	{
		void Recycle(); 
	}
	
	[CreateAssetMenu(fileName ="Ammunition Settings",menuName = "__Asteroids/Shared Data/Ammunition Template")]
	[SerializeField]
	public class AmmunitionSettings:ScriptableObject
	{

		public float BulletLife = 1f;
		public float Damage = 1f;
	}

	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(ScreenWrapper))]
	public class BaseAmmunitionBehaviour : MonoBehaviour, IShootable,IRecycle {

		/*public float BulletLife
		{
			get { return ammuTemplate != null ? ammuTemplate.BulletLife : 1.0f; }
		}*/

		public float Damage {
			get { return ammuTemplate!=null? ammuTemplate.Damage:1.0f; }
		 }

		[SerializeField]
		private AmmunitionSettings ammuTemplate;

		private Rigidbody m_rb;
		private ScreenWrapper m_screenWrapper;
		private Transform sourceParent; 
		Transform IShootable.Source
		{
			get { return sourceParent; }
			set { sourceParent = value; }
		}

		#region - MonoBehaviour methods
		public virtual void Awake() {

			//Reference components to use
			m_rb = GetComponent<Rigidbody>();
			m_screenWrapper = GetComponent<ScreenWrapper>();
			m_screenWrapper.OffScreen += OnRemoveFromView;
		}

		public virtual void OnEnable() {
			//OnRemoveFromView(null, null);
		}

		void OnDestroy()
		{
			m_screenWrapper.OffScreen -= OnRemoveFromView;
		}

		#endregion

		

		public void OnRemoveFromView(object sender, EventArgs e)
		{
			if (PoolManager.ExistClone(gameObject)) Recycle(); 
			else Destroy(gameObject);
		}

        #region - Implementations
        public void Recycle() { PoolManager.RecycleObject(this.gameObject); }
        public virtual void Fire(Vector3 position, Quaternion rotation, Vector3 velocity)
        {
            transform.position = position;
            transform.rotation = rotation;
            m_rb.velocity = velocity;
        }
        #endregion
    }
}
