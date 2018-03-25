using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class EnemySpawner{

		private GameObject[] m_enemyPrefabToSpawn;
		private List<ActiveEnemyUnit> m_spawnEnemies;

		private float startWaitTs = 3f;
		private float minSpawnWaitTs = 2f;
		private float maxSpawnWaitTs = 5f;

		private float spawnWaitTs;
		private int maxActiveEnemies = 2;

		private bool stopSpawn = true; 
		public bool StopSpawn { set { stopSpawn = value; } }

		private int m_currentEnemy; 

		#region events to suscribe with
		//	Enemy has been shot
		public event EventHandler<CollisionDetectedEventArgs> Shot;
		protected void OnShot(object sender, CollisionDetectedEventArgs e)
		{
			if (Shot != null)
				Shot(sender, e);
		}
		#endregion

		public EnemySpawner(GameObject[] enemyPrefabs)
		{
			m_enemyPrefabToSpawn = enemyPrefabs;
			m_spawnEnemies = new List<ActiveEnemyUnit>(); 
		}

		public void InitSpawn(Transform target)
		{
		}

		public IEnumerator StartSpawner(Transform target)
		{
			if (stopSpawn) yield return null;

			//	Wait to Start Spawning
			yield return new WaitForSeconds(startWaitTs);

			m_currentEnemy = UnityEngine.Random.Range(0, m_enemyPrefabToSpawn.Length);
			m_currentEnemy = 0;
			var enemyClone = PoolManager.SpawnObject(m_enemyPrefabToSpawn[m_currentEnemy]);
			var activeEnemyUnity = enemyClone.GetComponent<ActiveEnemyUnit>();

			//	Suscribe to Enemy Collision Events - When is Shot
			BasicCollisionDectection collisionDetection = activeEnemyUnity.GetComponentInChildren<BasicCollisionDectection>();
			if (collisionDetection != null)
				collisionDetection.UnitShot += (s, e) => OnShot(s, e);

			//	Suscribe to Enemy Collision Events - When is Collided
			activeEnemyUnity.Collided += (s, e) => { m_spawnEnemies.Remove(activeEnemyUnity); };

			if (m_spawnEnemies == null) m_spawnEnemies = new List<ActiveEnemyUnit>(); 
			m_spawnEnemies.Add(activeEnemyUnity);


			//	Spawn at free position
			activeEnemyUnity.Spawn();

			//	Start movement
			activeEnemyUnity.Move(target);
		}

		public void Reset()
		{
			StopSpawn = true;

			EnemyUnit currentEnemy; 
			for(int i=0; i< m_spawnEnemies.Count; i++)
			{
				currentEnemy = m_spawnEnemies[i];
				BasicCollisionDectection collisionDetection = currentEnemy.GetComponentInChildren<BasicCollisionDectection>();
				if (collisionDetection != null)
					collisionDetection.UnitShot -= (s, e) => OnShot(s, e);

				currentEnemy.Destruction();
			}
				
			m_spawnEnemies = new List<ActiveEnemyUnit>(); 
		}
	}
}
