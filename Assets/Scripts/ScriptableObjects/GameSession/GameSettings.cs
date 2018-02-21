 using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AsteroidsClone
{
	[CreateAssetMenu(fileName = "New Game Settings", menuName = "__Asteroids/Settings/Game Settings", order = 1)]
	public class GameSettings : ScriptableObject
	{
		[Serializable]
		public class UITemplate
		{
			public string Title = "ASTEROIDS";
			public string MessageIntro= "LEVEL ";
			public string TextPlayBtn ="Play";
			public string TextHighScoreBtn = "HighScore"; 
		}

		[SerializeField]
		[Range(1,3)]
		[Header("Number of lives by round")]
		public int NumLives;

		//	Max num of levels 
		[SerializeField]
		[Range(1, 8)]
		private int numLevels = 4; 
		public int NumLevels { get { return numLevels; } }

		//	Initial num asteroids by level (increases with the level)
		[Header("Initial number of asteroids")]
		[Tooltip("Number of asteroids increases by level")]
		[Range(1,3)]
		public int NumAsteroidsByLevel;

		//	Delay transitions
		public float LevelStartDelay;
		public float LevelEndDelay; 

		//	
		#region Singleton Reload - Proof Singleton
		private static GameSettings instance;
		public static GameSettings Instance
		{
			get
			{
				if (!instance)
					instance = Resources.FindObjectsOfTypeAll<GameSettings>().FirstOrDefault();
					//GameSettings gs = Resources.Load<GameSettings>("GameSettings");
				#if UNITY_EDITOR
				if (!instance)
					InitializeFromDefault(AssetDatabase.LoadAssetAtPath<GameSettings>("Assets/Resources/GameSettingsTest.asset"));
				#endif
				return instance;
			}
		}
		#endregion

		/// <summary>
		/// Initialize from existing Asset
		/// </summary>
		/// <param name="settings"></param>
		public static void InitializeFromDefault(GameSettings settings)
		{
			if (instance) DestroyImmediate(instance);
			instance = Instantiate(settings);
			instance.hideFlags = HideFlags.HideAndDontSave;
		}

		/// <summary>
		/// To access to the active GameSettings in use from Windows Menu
		/// </summary>
#if UNITY_EDITOR
		[UnityEditor.MenuItem("Window/__Asteroids/Game Settings")]
		public static void ShowGameSettings()
		{
			UnityEditor.Selection.activeObject = Instance;
		}
#endif
	}
}
