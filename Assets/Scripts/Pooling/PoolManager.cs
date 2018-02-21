using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class PoolManager : Singleton<PoolManager> {

		[SerializeField]
		private Transform parent;

		private Dictionary<GameObject, ObjectPool<GameObject>> prefabCollections;
		private Dictionary<GameObject, ObjectPool<GameObject>> instanceCollections;

		public override void Awake()
		{
            //DontDestroyOnLoad(this.gameObject);
            base.Awake();
			prefabCollections = new Dictionary<GameObject, ObjectPool<GameObject>>();
			instanceCollections = new Dictionary<GameObject, ObjectPool<GameObject>>();
		}

		public void buildPool(GameObject prefab, int poolSize)
		{
			if (prefabCollections.ContainsKey(prefab))
				throw new Exception("Pool for prefab " + prefab.name + " has already been created");
			
			var pool = new ObjectPool<GameObject>(() => { return Clone(prefab); }, poolSize);
			prefabCollections[prefab] = pool;

		}

		public ObjectPool<GameObject> getPool(GameObject prefab)
		{
			if (!prefabCollections.ContainsKey(prefab))
				throw new Exception("Pool for prefab " + prefab.name + " has not yet been created. Build the Pool first");

			return prefabCollections[prefab]; 
		}

		public GameObject Clone(GameObject prefab)
		{
			if(parent == null)
				parent = GameObject.FindGameObjectWithTag(Tags.Dynamic).transform;

			var clone = Instantiate(prefab, Vector3.zero, Quaternion.identity, parent) as GameObject;
			clone.SetActive(false);
			return clone;
		}

		public GameObject spawnObject(GameObject prefab)
		{
			return spawnObjectAt(prefab, Vector3.zero, Quaternion.identity);
		}

		public GameObject spawnObjectAt(GameObject prefab, Vector3 position, Quaternion rotation)
		{
			if (!prefabCollections.ContainsKey(prefab))
				buildPool(prefab, 1);
			

			var pool = prefabCollections[prefab];

			var clone = pool.GetPooledItem();
			clone.transform.position = position;
			clone.transform.rotation = rotation;
			clone.SetActive(true);

			instanceCollections.Add(clone, pool);
			return clone;
		}

		public void recycleObject(GameObject clone)
		{
			clone.SetActive(false);

			if (instanceCollections.ContainsKey(clone))
			{
				instanceCollections[clone].RecycleItem(clone);
				instanceCollections.Remove(clone);
			}
			/*else
			{
				Debug.LogWarning("The object does not exist in any pool: " + clone.name);
			}*/
		}

		public bool existClone(GameObject clone)
		{
			if (instanceCollections.ContainsKey(clone))
				return true; 
			else
				Debug.LogWarning("The Clone  does not exist in any pool: " + clone.name);

			return false; 
		}

		public void PrintStatus()
		{
			foreach (KeyValuePair<GameObject, ObjectPool<GameObject>> keyVal in prefabCollections)
			{
				Debug.Log(string.Format("Object Pool for Prefab: {0} In Use: {1} Total {2}", keyVal.Key.name, keyVal.Value.CountUsedItems, keyVal.Value.PoolSize));
			}
		}

		#region Static Methods
		public static void BuildPool(GameObject prefab, int poolSize) { Instance.buildPool(prefab, poolSize); }

		public static ObjectPool<GameObject> GetPool(GameObject prefab) { return Instance.getPool(prefab); }

		public static GameObject SpawnObject(GameObject prefab) { return Instance.spawnObject(prefab);}

		public static GameObject SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation) { return Instance.spawnObjectAt(prefab, position, rotation); }

		public static void RecycleObject(GameObject clone) { Instance.recycleObject(clone); }

		public static bool ExistClone(GameObject clone) { return Instance.existClone(clone); }

        #endregion
    }
}

