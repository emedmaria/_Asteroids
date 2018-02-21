using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	[RequireComponent(typeof(EntityBehaviour))]
	public class PlayerCollisionDetection : MonoBehaviour
	{
		//	Pasive Unit Collides with Player
		public event EventHandler<CollisionDetectedEventArgs> PlayerCollided;
		protected virtual void OnPlayerCollided(object sender, CollisionDetectedEventArgs e)
		{
			if (PlayerCollided != null)
				PlayerCollided(sender, e);
		}
		//	Active Unit Shoots Player
		public event EventHandler<CollisionDetectedEventArgs> PlayerGetsShot;
		protected virtual void OnPlayerGetsShot(object sender, CollisionDetectedEventArgs e)
		{
			if (PlayerGetsShot != null)
				PlayerGetsShot(this, e);
		}

		void OnTriggerEnter(Collider trigger)
		{
			if (trigger.gameObject.tag != Tags.Ammu) return;

			Debug.Log(this.name + " -Trigger in " + trigger.gameObject.name);

			CollisionDetectedEventArgs e = new CollisionDetectedEventArgs();
			e.EnemyUnit = gameObject.GetComponent<EnemyUnit>();
			OnPlayerGetsShot(this, e);
		}

		void OnCollisionEnter(Collision colision)
		{
			Debug.Log(this.name + " -Collided with: " + colision.gameObject.name);

			CollisionDetectedEventArgs e = new CollisionDetectedEventArgs();
			e.EnemyUnit = gameObject.GetComponent<EnemyUnit>();
			PlayerCollided(this, e);
		}
	}
}
