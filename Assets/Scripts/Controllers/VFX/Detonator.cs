using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class Detonator: Singleton<Detonator>
	{
		[SerializeField]
		public BaseExplosion [] explosions;

        public bool poolsBuilt = false;

		private Dictionary<BaseExplosion, ObjectPool<GameObject>> explosionCollection;

		public override void Awake()
		{
            //DontDestroyOnLoad(this.gameObject);
            base.Awake(); 

			if (explosionCollection == null)
				explosionCollection = new Dictionary<BaseExplosion, ObjectPool<GameObject>>();
		}

		public void BuildPools(int poolSize)
		{
			if (explosionCollection == null)
				explosionCollection = new Dictionary<BaseExplosion, ObjectPool<GameObject>>();

			int nExplosions = explosions.Length;
			for (int i = 0; i < nExplosions; i++)
			{
				BaseExplosion currentExplosion = explosions[i];
				PoolManager.BuildPool(currentExplosion.ParticleSysPrefab, poolSize);
				ObjectPool<GameObject> itemsPool = PoolManager.GetPool(currentExplosion.ParticleSysPrefab);
				explosionCollection[currentExplosion] = itemsPool;
			}
			//poolsBuilt = true;
		}

		public void Explode(ExplosionType type, Vector3 position)
		{

			for (int i=0; i< explosions.Length; i++)
			{
				BaseExplosion currentExplosion = explosions[i];

				if (currentExplosion.ExplosionType == type)
				{
					var clone = PoolManager.SpawnObject(currentExplosion.ParticleSysPrefab);
					if (clone == null) return;
					ParticleSystem ps = clone.GetComponent<ParticleSystem>();
					ps.transform.position = position;

					currentExplosion.ExplosionFinished += (s, e) => ExplosionDie(clone);
					currentExplosion.Explode(currentExplosion.SoundFX, ps, position);
					return; 
				}
			}
		}

		private void ExplosionDie(GameObject pooledObj)
		{
			StartCoroutine(Recycle(pooledObj));
		}

		IEnumerator Recycle(GameObject pooledObj)
		{
			yield return new WaitForSeconds(2f);

			//	Recycle explosion in the pool 
			PoolManager.RecycleObject(pooledObj);

			ParticleSystem ps = pooledObj.GetComponent<ParticleSystem>();
			ps.Stop();

			//	Reset position
			ps.transform.position = Vector3.zero;
		}
	}
}

