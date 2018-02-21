using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class GameState: ScriptableObject
	{
		private const string DEFAULT_NAME = "Default_Player";

		public string CurrentName{ get; set;}
		public int CurrentLevel{ get; set; }
		public int CurrentScore{ get; set; }
		public int CurrentLive;
		private bool inGame;
		public bool InGame{
			get { return inGame; }
			set {
				inGame = value;
				if(inGame) AudioManager.Instance.PlayMusic(SoundFXType.SpaceAmbience);
				else AudioManager.Instance.StopMusic();
			}
		}

		private PlayerState playerState = new PlayerState();
		private List<PlayerState> topT10; 
		public List<PlayerState> Top10 { get { return topT10; } }

		/// <summary>
		/// This is meant to track the HighScore/Top10
		/// </summary>
		[Serializable]
		public class PlayerState
		{
			public string Name = DEFAULT_NAME;
			public int Level = 1;
			public int HighScore = 0;
		}

		#region - Singleton Reloadable
		private static GameState instance;
		public static GameState Instance
		{
			get
			{
				if (!instance)
					instance = Resources.FindObjectsOfTypeAll<GameState>().FirstOrDefault();

#if UNITY_EDITOR
				if (!instance)
					CreateFromScratch();
#endif
				return instance;
			}
		}

		private static void CreateFromScratch()
		{
			instance = CreateInstance<GameState>();
			instance.hideFlags = HideFlags.HideAndDontSave;
		}
		#endregion


		public void LoadTop10()
		{
			DataPersistence.LoadTop10FromJSON();
			topT10 = DataPersistence.dataContainer.scores;
			topT10 = topT10.OrderByDescending(o => o.HighScore).ToList();
		}

		public void StoreData()
		{
			playerState.Name = CurrentName;
			playerState.Level = CurrentLevel;
			playerState.HighScore = CurrentScore;
		}

		/// <summary>
		/// Stores relevant information for the HighScore 
		/// </summary>
		public void StoreHighScoreData()
		{
			//StoreData(); 
			DataPersistence.AddScoreData(playerState);

			DataPersistence.SaveToJSON(DataPersistence.SavedPlayerStatePath, DataPersistence.dataContainer);
			topT10 = DataPersistence.dataContainer.scores;
		}

#if UNITY_EDITOR
		[UnityEditor.MenuItem("Window/__Asteroids/Game State")]
		public static void ShowGameState()
		{
			UnityEditor.Selection.activeObject = Instance;
		}
#endif
	}
}
