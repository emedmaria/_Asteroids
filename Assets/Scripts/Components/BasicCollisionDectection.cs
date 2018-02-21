using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class CollisionDetectedEventArgs : EventArgs
	{
		public EnemyUnit EnemyUnit { get; set; }
		public EntityBehaviour SourceEntity { get; set; }
	}

	public class BasicCollisionDectection : MonoBehaviour
	{
		//	When other object collided with owner (GO with component attached) 
		public event EventHandler<CollisionDetectedEventArgs> Collided;
		protected virtual void OnCollided(object sender, CollisionDetectedEventArgs e)
		{
			if (Collided != null)
				Collided(sender, e);
		}
		//	Unit Shot
		public event EventHandler <CollisionDetectedEventArgs> UnitShot; 
		protected virtual void OnUnitShot(object sender, CollisionDetectedEventArgs e)
		{
			if (UnitShot != null)
				UnitShot(this, e);
		}

		void OnTriggerEnter(Collider trigger)
		{
			IShootable ammu = trigger.gameObject.GetComponent<IShootable>();

			//	Ignore if is not a shot
			if (ammu == null) return;

			Transform ammuSource = ammu.Source;
			EntityBehaviour sourceEntity = ammuSource.gameObject.GetComponentInParent<EntityBehaviour>(); 
			EnemyUnit targetUnit = gameObject.GetComponentInParent<EnemyUnit>();

			if (targetUnit != null && sourceEntity!=null)
			{
				// Unit Shot by Player/Enemy
				CollisionDetectedEventArgs colisionEvent = new CollisionDetectedEventArgs();
				colisionEvent.SourceEntity = sourceEntity;
				colisionEvent.EnemyUnit = targetUnit;

				//	Debug.Log(this.name + " -Trigger in " + trigger.gameObject.name);

				OnUnitShot(this, colisionEvent);
			}
		}


		void OnCollisionEnter(Collision colision)
		{
			//Debug.Log(this.name + " -Collided with: " + colision.gameObject.name);

			CollisionDetectedEventArgs e = new CollisionDetectedEventArgs();
			e.SourceEntity = gameObject.GetComponent<EntityBehaviour>();
			e.EnemyUnit = colision.gameObject.GetComponentInParent<EnemyUnit>(); 
			OnCollided(this, e); 
		}
	}

}

