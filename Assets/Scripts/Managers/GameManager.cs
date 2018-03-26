using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class GameManager : MonoBehaviour
	{
		private const float GAMEOVER_PAUSE = 2.5f; 
		private const float START_LEVEL_PAUSE = 2f;
		private const float LEVEL_COMPLETED_PAUSE = 1.5f;
		private const float PLAYER_READY_PAUSE = 1f;

		[SerializeField]
		private UIController uiController;			//	Reference to GUI
		[SerializeField]
		private LevelManager levelManager;          //	Reference to LevelManager (level handling)

		private GameSettings gameSettings;          //	Reference to GameSettings (default settings of the game)
		private GameState gameState;                //	Reference to State of the current game State
        private AudioManager audioManager; 

		private int m_currentNumAsteroidsInLevel; 

		[NonSerialized]
		public GameSettings.UITemplate UITemplate;

		#region - Monobehaviour methods
		void Awake()
		{
			Assert.IsNotNull(uiController, "[GameManager] UIController MUST be referenced in the inspector!");
			Assert.IsNotNull(levelManager, "[GameManager] LevelManager MUST be referenced in the inspector!");

			//	Save References
			gameSettings = GameSettings.Instance;
			gameState = GameState.Instance;
            audioManager = AudioManager.Instance; 
		}

		private void OnEnable() { AddListeners(); }
		private void OnDisable() { RemoveListeners(); }

		void Start()
		{
			Initialize();
			StartCoroutine(GameLoop());
		}
		#endregion

		void Initialize()
		{
			//	Load HighScore Data
			gameState.LoadTop10();

			//	 Init State
			uiController.InitalizeViews(UITemplate,gameState.Top10);

			//	Reset GameState
			ResetGame();

			//	Level Init
			levelManager.Initialize();
		
			// Create the delays so they only have to be made once.
			//m_StartWait = new WaitForSeconds(m_StartDelay);
			//m_EndWait = new WaitForSeconds(m_EndDelay);
			//	Prepare the objects to pool shown in the level (Asteroids)

		}

		void ResetGame()
		{
			//	Reset GameState
			gameState.CurrentLevel = 1;
			gameState.CurrentLive = gameSettings.NumLives;
			gameState.CurrentScore = 0;
			m_currentNumAsteroidsInLevel = gameSettings.NumAsteroidsByLevel; 

			//	Reset UI
			uiController.UpdateLives(gameState.CurrentLive);
			uiController.UpdateScore(gameState.CurrentScore);

		}

		void NewGame()
		{
			//	Set initial State
			gameState.InGame = false;

			// Initially in Main Menu
			uiController.CurrentState = UIState.MainMenu;

			ResetGame(); 
		}

		IEnumerator GameLoop()
		{
			NewGame();

			while (true)
			{
				// While User has not pressed Play 
				while (!gameState.InGame)
					yield return null;
				
				yield return StartCoroutine(LevelStart());
				yield return StartCoroutine(LevelPlaying());
				yield return StartCoroutine(LevelEnd());
				GC.Collect();
			}
		}

		IEnumerator LevelStart()
		{
			uiController.CurrentState = UIState.InGame;

            //	Display the current level and hide
            audioManager.PlaySFX(SoundFXType.LevelStart); 
            uiController.ShowMessage("WAVE " + gameState.CurrentLevel);
			yield return new WaitForSeconds(START_LEVEL_PAUSE);
			uiController.HideMessage();

			//	Show Hud
			uiController.DisplayHud();
			//yield return new WaitForSeconds(START_LEVEL_PAUSE);

			//	Player Init
			levelManager.RecoverPlayer();
			yield return new WaitForSeconds(PLAYER_READY_PAUSE);

			//	Prepare the level
			levelManager.StartLevel(gameState.CurrentLevel, m_currentNumAsteroidsInLevel);
            
        }

		IEnumerator LevelPlaying()
		{
			//	Check whether Player is Alive & there are Asteroids left
			while (gameState.CurrentLive > 0  && levelManager.AsteroidsAlive)
				yield return null;
		}

		IEnumerator LevelEnd()
		{
			bool gameover = gameState.CurrentLive ==0 ;
			if (gameover)
			{
				//	Spawn objects reset
				levelManager.ResetSpawners();

				//	Player controls not active
				levelManager.ResetPlayer();

                audioManager.StopMusic(); 

                //	Display message and hide
                audioManager.PlaySFX(SoundFXType.GameOver);
                uiController.ShowMessage("GAMEOVER");
				yield return new WaitForSeconds(GAMEOVER_PAUSE);
				uiController.HideMessage();

				gameState.InGame = false;

				//	Save data before Reset Game
				gameState.StoreData();

				uiController.UpdateResume(gameState.CurrentScore, DataPersistence.HighestScore());

				//	Show Resume Screen
				uiController.CurrentState = UIState.GameOver;

				//	Reset GameState to restart from scratch
				ResetGame(); 
			
				//NewGame();  
			}
			else{

				//	Player controls not active
				levelManager.ResetPlayer();
				//levelManager.ResetEnemies(); 

				//	Display message and hide
				uiController.ShowMessage("LEVEL COMPLETED!!");
				yield return new WaitForSeconds(LEVEL_COMPLETED_PAUSE);
				uiController.HideMessage();

				ProgressToNextLevel();
			}
		}

		private void ProgressToNextLevel()
		{
			gameState.CurrentLevel += 1;
			m_currentNumAsteroidsInLevel += gameState.CurrentLevel; 
		}

		#region Data Persistence
		void SaveData(object sender, InputTextEventArgs e)
		{
			gameState.CurrentName = e.InputText;
			//	Score should be set before  level
			gameState.StoreHighScoreData();
			uiController.UpdateHighScore(gameState.Top10);
			uiController.CurrentState = UIState.HighScore;
		}
		#endregion


		#region event Handlers and suscriptions
		private void AddListeners()
		{
			//	Suscribe to UI events
			uiController.SaveData += SaveData;

			//	Suscribe to level events
			levelManager.Died += OnDied;
			levelManager.Scored += OnScored;
		}

		private void RemoveListeners()
		{
			//	UnSuscribe to UI events
			uiController.SaveData -= SaveData;

			//	UnSuscribe to level events
			levelManager.Died -= OnDied;
			levelManager.Scored -= OnScored;
		}

		private void OnDied(object sender, EventArgs e)
		{
			gameState.CurrentLive = gameState.CurrentLive>0? gameState.CurrentLive -1:0;

			//	Update UI
			uiController.UpdateLives(gameState.CurrentLive);

			if (gameState.CurrentLive > 0)
				levelManager.RecoverPlayer();
		}
		
		private void OnScored(object sender, CollisionDetectedEventArgs e)
		{
			EnemyUnit unit = e.EnemyUnit;

			//	Todo check points obtained
			gameState.CurrentScore += e.EnemyUnit.DestructionScore;

			//	Update UI
			uiController.UpdateScore(gameState.CurrentScore);
		}
		#endregion
	}
}

