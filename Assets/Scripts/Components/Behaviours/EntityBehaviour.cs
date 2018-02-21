using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public abstract class EntityBehaviour : MonoBehaviour
	{
		public event EventHandler Collided;
		protected void OnDestroyed(object sender, EventArgs e)
		{
			if (Collided != null)
				Collided(sender, e);
		}

		public virtual GameObject CreateClone(GameObject prefab, Transform parent = null)
		{
			GameObject clone = Instantiate(prefab, parent);
			return clone; 
		}

		public virtual bool IsAlive { get { return gameObject.activeSelf; } }
		public virtual void Spawn() { }
		public virtual void SpawnAt(Vector3 position) { transform.position = position; }
		public virtual void Collide(int damamge = 1) { }

		public abstract void EnableControls();
		public abstract void DisableControls();
		public abstract void Destruction();
		public abstract void Recover();
	}
}

