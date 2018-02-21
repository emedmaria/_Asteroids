using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class ObjectPoolHolder<T>
	{
		private T pooledItem;

		public bool InUse { get; private set; }
		public void Use() { InUse = true;}
		public T Item { get; set; }
		public void Recycle() { InUse = false; }
	}

	public class ObjectPool<T>
	{
		private List<ObjectPoolHolder<T>> itemsList;
		private Dictionary<T, ObjectPoolHolder<T>> storage; 
		private Func<T> builderFunction;

		private int m_lastIndex = 0;
		private int m_poolSize;
		private bool m_enableExpand = true;

		public int PoolSize { get { return itemsList.Count; } }
		public int CountUsedItems { get { return storage.Count; } }


		public ObjectPool(Func<T> builderFunc, int poolSize)
		{
			m_poolSize = poolSize;
			this.builderFunction = builderFunc;

			itemsList = new List<ObjectPoolHolder<T>>(m_poolSize);
			storage = new Dictionary<T, ObjectPoolHolder<T>>(m_poolSize);

			Build(m_poolSize);
		}

		private void Build(int capacity)
		{
			for (int i = 0; i < capacity; i++)
				CreatePoolHolder();
		}

		private ObjectPoolHolder<T> CreatePoolHolder()
		{
			var itemHolder = new ObjectPoolHolder<T>();
			itemHolder.Item = builderFunction();
			itemsList.Add(itemHolder);
			return itemHolder;
		}

		public T GetPooledItem()
		{
			ObjectPoolHolder<T> holder = null;

			int nItems = itemsList.Count; 
			for (int i = 0; i < nItems; i++)
			{
				m_lastIndex++;
				if (m_lastIndex > nItems - 1) m_lastIndex = 0;

				if (itemsList[m_lastIndex].InUse)
				{
					continue;
				}
				else
				{
					holder = itemsList[m_lastIndex];
					break;
				}
			}

			if (holder == null && m_enableExpand)
			{
				holder = CreatePoolHolder();
			}

			holder.Use();
			storage.Add(holder.Item, holder);
			return holder.Item;
		}

		public void Recycle(object item)
		{
			RecycleItem((T)item);
		}

		public void RecycleItem(T item)
		{
			if (storage.ContainsKey(item))
			{
				var container = storage[item];
				container.Recycle();
				storage.Remove(item);
			}
			else
			{
				Debug.LogWarning("This Pool does not contain the item: " + item);
			}
		}
	}
}


