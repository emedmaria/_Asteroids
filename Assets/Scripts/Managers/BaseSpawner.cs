using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class BaseSpawner
	{
		private int m_nObjectsToSpwan;
		private List<EntityBehaviour> m_spawnObjects;
		private GameObject m_prefabToSpawn;
		private GameObject m_childPrefabToSpawn;

		#region events to suscribe with
		public event EventHandler<CollisionDetectedEventArgs> Shot;
		protected void OnShot(object sender, CollisionDetectedEventArgs e)
		{
			if (Shot != null)
				Shot(sender, e);
		}
		#endregion

		public int EntityRemaining { get { return m_spawnObjects.Count; } }

		/// <summary>
		/// Constructor. Save references of parameters passed
		/// </summary>
		/// <param name="objectsToSpwan"></param>
		/// <param name="prefab"></param>
		public BaseSpawner(int objectsToSpwan, GameObject prefab, GameObject childPrefab = null)
		{
			m_prefabToSpawn = prefab;
			m_childPrefabToSpawn = childPrefab; 
			m_nObjectsToSpwan = objectsToSpwan;

			m_spawnObjects = new List<EntityBehaviour>(m_nObjectsToSpwan);
		}

		/// <summary>
		/// Spawn a set of instances retrieved from the pool  
		/// </summary>
		public void SpawnSet() { 

			for (int i = 0; i < m_nObjectsToSpwan; i++)
			{
				var clone = PoolManager.SpawnObject(m_prefabToSpawn);
				var entity = clone.GetComponent<EntityBehaviour>();
				entity.Spawn();

				BasicCollisionDectection collisionDetection = entity.GetComponentInChildren<BasicCollisionDectection>();
				if (collisionDetection != null)
					collisionDetection.UnitShot +=(s,e)=> OnShot(s,e); 
				
				//	Suscribe to collision events
				entity.Collided += (s, e) => { m_spawnObjects.Remove(entity); };

				//	Refactor!
				var passiveEnemy = clone.GetComponent<PassiveEnemy>();
				if (passiveEnemy != null)
					passiveEnemy.AsteroidShatter += OnAsteroidShatter; 
				
				m_spawnObjects.Add(entity); 
			}
		}

		/// <summary>
		/// Reset the list which track the active Units
		/// Recycle the instances back to the pool 
		/// </summary>
		public void Reset()
		{
			if (m_spawnObjects != null)
			{
				int nItems = m_spawnObjects.Count; 

				for (int i = m_spawnObjects.Count-1; i >= 0; i--)
				{
					EntityBehaviour entity = m_spawnObjects[i]; 
					BasicCollisionDectection collisionDetection = entity.GetComponentInChildren<BasicCollisionDectection>();

					//	Unsuscribe to events
					if (collisionDetection != null)
						collisionDetection.UnitShot -= (s, e) => OnShot(s, e);

					entity.Collided -= (s, e) => { m_spawnObjects.Remove(entity); };

					//	Destroy
					entity.Destruction();
				}
			}
			//	Clear the active list 
			m_spawnObjects = new List<EntityBehaviour>();
		}

		private void OnAsteroidShatter(object sender, EventArgs e)
		{
			PassiveEnemy passiveEnemy = sender as PassiveEnemy;
			passiveEnemy.AsteroidShatter -= OnAsteroidShatter;

			for (int i=0; i<passiveEnemy.NumBits; i++)
			{
				var clone = PoolManager.SpawnObject(m_childPrefabToSpawn);
				var entity = clone.GetComponent<EntityBehaviour>();
				entity.SpawnAt(passiveEnemy.transform.position); 
				m_spawnObjects.Add(entity);

				BasicCollisionDectection collisionDetection = entity.GetComponentInChildren<BasicCollisionDectection>();
				if (collisionDetection != null)
					collisionDetection.UnitShot += (s, evt) => OnShot(s, evt);

				//	Suscribe to collision events
				entity.Collided += (s, evt) => { m_spawnObjects.Remove(entity); };
				
			}
		}
	}
}

