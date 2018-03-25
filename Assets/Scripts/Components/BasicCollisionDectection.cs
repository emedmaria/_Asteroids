using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
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

		//	Player Shot
		/*public event EventHandler<CollisionDetectedEventArgs> PlayerShot;
		protected virtual void OnPlayerShot(object sender, CollisionDetectedEventArgs e)
		{
			if (PlayerShot != null)
				PlayerShot(this, e);
		}*/

		//	Active or Passive Enemy (Asteroids/Saucer) gets shot
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
				if (targetUnit.tag == "Saucer")
					Debug.Log("STOP");

				if (targetUnit.tag == "Player")
					Debug.Log("PLAYER SHOT");

				// Unit Shot by Player (Asteroid or Saucer)
				CollisionDetectedEventArgs colisionEvent = new CollisionDetectedEventArgs();
				colisionEvent.SourceEntity = sourceEntity;	//	Player
				colisionEvent.EnemyUnit = targetUnit;		//	Enemy Shot - Asteroid/Saucer

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

