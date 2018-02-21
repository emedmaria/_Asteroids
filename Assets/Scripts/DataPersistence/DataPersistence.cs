using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace AsteroidsClone
{
	[Serializable]
	public class DataContainer
	{
		public const int  MAX_SCORES = 10;

		//	HighScore 
		public List<GameState.PlayerState> scores = new List<GameState.PlayerState>(MAX_SCORES);

		//	We could add more variables to save  data after session 
		//	...
	}

	public class DataPersistence
    {
		public static DataContainer dataContainer = new DataContainer();

		#region events to suscribe 
		public delegate void DataPersistenceEvent();
        public static event DataPersistenceEvent OnLoaded;
        public static event DataPersistenceEvent OnBeforeSave;
		#endregion

		public static string SavedPlayerStatePath {
            get { return System.IO.Path.Combine(Application.persistentDataPath, "highScore_Top10.json");}
        }

		public static void LoadTop10FromJSON()
		{
			LoadFromJSON(SavedPlayerStatePath);
			//	
			//ClearScoreList();
		}

		private static void LoadFromJSON(string path)
		{
			//	Creates file if does not exist
			if (!File.Exists(path))
			{
				FileStream file = File.Create(SavedPlayerStatePath);
				file.Close();
				return; 
			}

			string json = File.ReadAllText(path);
			dataContainer = String.IsNullOrEmpty(json)?new DataContainer():JsonUtility.FromJson<DataContainer>(json);

			// Ensure that are sorted and do not exeed the limit
			List<GameState.PlayerState> scores = dataContainer.scores;
			scores = scores.OrderByDescending(o => o.HighScore).ToList();
			int existingItems = scores.Count;
			if (existingItems > DataContainer.MAX_SCORES)
				scores.RemoveRange(DataContainer.MAX_SCORES-1, existingItems - DataContainer.MAX_SCORES);

			if (OnLoaded!=null)
				OnLoaded();

			//ClearScoreList();
		}

		public static void SaveTop10ToJSON(DataContainer scores)
		{
			SaveToJSON(SavedPlayerStatePath, scores);
		}

		public static void SaveToJSON(string path, DataContainer scores)
		{
			//	To make sure the PlayerState is updated with last round
			if(OnBeforeSave!=null)
				OnBeforeSave(); 

			Debug.LogFormat("Saving game state to {0}", path);
			File.WriteAllText(path, JsonUtility.ToJson(scores, true));

			//ClearScoreList(); 
		}

		public static void AddScoreData(GameState.PlayerState data)
		{
			dataContainer.scores.Add(data);

			// Ensure that are sorted and do not exeed the limit
			List<GameState.PlayerState> scores = dataContainer.scores;
			scores = scores.OrderByDescending(o => o.HighScore).ToList();

			int existingItems = scores.Count;
			if(existingItems > DataContainer.MAX_SCORES)
				scores.RemoveRange(DataContainer.MAX_SCORES - 1, existingItems - DataContainer.MAX_SCORES);
		}

		public static void ClearScoreList()
		{
			dataContainer.scores.Clear();
		}

		public static int HighestScore() {
			//	We just want 10 players listed
			List<GameState.PlayerState> scores = dataContainer.scores;
			return scores != null && scores.Count > 0 ? scores[0].HighScore : 0; 
		}
	}
}

