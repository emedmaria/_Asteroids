using System;
using UnityEngine;

namespace AsteroidsClone
{
	[RequireComponent(typeof(Rigidbody))]
	public abstract class EnemyUnit : EntityBehaviour {

		abstract public int Health { get;  }
		abstract public int DestructionScore { get; }
		abstract public int Damage { get; }

		//	TODO: Move this to the abstract SO
		/*[SerializeField]
		[Range(0, 200)]
		protected int destructionScore = 100;				//	Points obtained when is hit
		public int DestructionScore { get { return destructionScore; } }
		
		[SerializeField]
		protected int health = 1;							//	Number of hits to permanent destroy it
		public int Health { get { return health; } }
		[SerializeField]
		protected int damage = 1;							//	Damage caused by its collision
		public int Damage { get { return damage; } }
		*/

		//	State Variables
		protected int currentHealth;
		public int CurrentHealth { get { return currentHealth;  } }

		protected Rigidbody m_rb;

		[SerializeField]
		private ExplosionType explosionType;
		public virtual ExplosionType ExplosionType { get { return explosionType; }}
	
		public virtual void Awake() {
			m_rb = GetComponent<Rigidbody>();
			//currentHealth = health;
			currentHealth = Health;
		}

		#region Spawning
		public override void Spawn() { transform.position = FindFreePosition(); }

		protected virtual Vector3 FindFreePosition(int layerMask = ~0)
		{
			float posX = transform.localScale.x;
			float posY = transform.localScale.y;
			float collisionSphereRadius = posX > posY ? posX : posY;

			bool overlap = false;
			Vector3 freePosition;
			do
			{
				Vector3 randomXY = new Vector3(UnityEngine.Random.value, UnityEngine.Random.value);
                freePosition = Camera.main.ViewportToWorldPoint(randomXY);
                freePosition.z = 0; 
				overlap = Physics.CheckSphere(freePosition, collisionSphereRadius, layerMask);

			} while (overlap);
			return freePosition;
		}
        #endregion
        public override void EnableControls()
        {
            m_rb.detectCollisions = true;
            m_rb.isKinematic = false;
        }

        public override void DisableControls()
        {
            m_rb.detectCollisions = false;
            m_rb.isKinematic = true;
        }

        public override void Destruction()
		{
            ResetTransform();
            ResetRigidbody(); 

            PoolManager.RecycleObject(gameObject);
            OnDestroyed(this, EventArgs.Empty);
        }

		public override void Recover()
		{
            if (PoolManager.ExistClone(gameObject))
                gameObject.SetActive(true);

			//currentHealth = health;
			currentHealth = Health;

			EnableControls();
            SpawnAt(FindFreePosition());
			
        }

        public override void Collide(int damage =1)
		{
			//	Trigger VFX
			Detonator.Instance.Explode(ExplosionType, gameObject.transform.position);
            //  Reduce damage affected by collision
			currentHealth -= damage;
		}

        public void ResetTransform()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public void ResetRigidbody()
        {
            m_rb.position = Vector3.zero;
            m_rb.rotation = Quaternion.identity;
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
        }
    }
}
