using System.Linq;
using UnityEngine;

namespace AsteroidsClone
{
	/// <summary>
	/// Abstract class for making reload-proof singletons out of ScriptableObjects
	/// Returns the asset created on the editor o via script & saved, 
	/// otherwise will create on on demand
	/// </summary>
	/// <typeparam name="T">Singleton type</typeparam>

	public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		static T instance = null;
		public static T Instance
		{
			get
			{
				if (!instance)
					instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();

				if (!instance)
					instance = CreateInstance<T>();

				return instance;
			}
		}
	}
}
