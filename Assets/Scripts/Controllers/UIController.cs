using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public enum UIState
	{
		MainMenu,
		InGame,
		GameOver, 
		HighScore
	}

	public class UIController : MonoBehaviour
	{
		//	References to the Views of subMenus (States)
		[SerializeField]
		private MainMenuView mainMenuView;

		//	Reference to the Hud
		[SerializeField]
		private HudView hudView;

		//	Reference to the HighScore
		[SerializeField]
		private HighScoreView highScoreView;

		//	Reference to Resume Screen
		[SerializeField]
        private GameOverView gameOverView;

		//	Reference to InGame Messages
		[SerializeField]
		private MessagesView messagesView;

        //	Background PS      
        private ParticleSystem m_starSystem; //   It would be better to have a centralized PS Sys (we just have one, tough - explosions aside)
        public GameObject StarSystemPrefab;

        #region Events to suscribe with
        public event EventHandler StateChanged;
		protected virtual void OnStateChanged()
		{
			if (StateChanged != null)
				OnStateChanged();
		}

		public event EventHandler<InputTextEventArgs> SaveData;
		protected virtual void OnSaveData(object sender, InputTextEventArgs e)
		{
			if (SaveData != null)
				SaveData(sender, e);
		}
		#endregion

		private void AddListenersFromView()
		{
			// Suscribe to MainMenu events
			mainMenuView.PlayRaised += (s, e) => GameState.Instance.InGame = true;
			mainMenuView.HighScoreRaised += (s, e) => CurrentState = UIState.HighScore;

			// Suscribe to GameOver events
			gameOverView.BackToMenu += (s, e) => CurrentState = UIState.MainMenu;
			gameOverView.SaveHighScore += (s, e) => OnSaveData(s, e);

			//	Suscribe to HighScoreView events
			highScoreView.BackRaised += (s, e) => CurrentState = UIState.MainMenu;

			//Suscribe to Hud events
			//..
		}

		private void RemoveListenersFromView()
		{
			//	Unsuscribe from MainMenu events
			mainMenuView.PlayRaised -= (s, e) => GameState.Instance.InGame = true;
			mainMenuView.HighScoreRaised -= (s, e) => CurrentState = UIState.HighScore;

			// Unsuscribe from GameOver events
			gameOverView.BackToMenu -= (s, e) => CurrentState = UIState.MainMenu;
			gameOverView.SaveHighScore -= (s, e) => OnSaveData(s, e);

			//	Unsuscribe from HighScore View events
			highScoreView.BackRaised -= (s, e) => CurrentState = UIState.MainMenu;
		}

		private UIState currentState;
		public UIState CurrentState {
			get { return currentState; }
			set
			{
				currentState = value;
				UpdateState();

				OnStateChanged();
			}
		}
		public void InitalizeViews(GameSettings.UITemplate uiTemplate, List<GameState.PlayerState> top10)
		{
			//	TODO: Initiaze content of the view
			mainMenuView.Init();

			//	Initialize TOP 10 View 
			highScoreView.Init(top10);

			//	Initialize GameOver View
			gameOverView.Init();

            CreateBackground(); 
        }

		public void UpdateHighScore(List<GameState.PlayerState> top10) { highScoreView.Top10 = top10; }

		#region - MonoBehaviour Methods
		void Awake()
		{
			//	To prevent if active in the hierachy
			mainMenuView.Hide();
			hudView.Hide();
			gameOverView.Hide();
			highScoreView.Hide();
		}

		void Start() { AddListenersFromView(); }
		void Destroy() { RemoveListenersFromView(); }
		void OnDisable() { RemoveListenersFromView();}
		#endregion

		private void UpdateState()
		{
			switch (currentState)
			{
				case UIState.MainMenu:
					mainMenuView.Show();
					hudView.Hide();
                    gameOverView.Hide();
					highScoreView.Hide();
					messagesView.Hide();
                    EnableBackground();
					break;
				case UIState.InGame:
					mainMenuView.Hide();
					hudView.Show();
                    gameOverView.Hide();
					highScoreView.Hide();
					DisableBackground();
					messagesView.Show();
					break;
                case UIState.GameOver:
                    mainMenuView.Hide();
                    hudView.Hide();
					EnableBackground();
					gameOverView.Show();
					highScoreView.Hide();
					messagesView.Hide();
					break;
				case UIState.HighScore:
					mainMenuView.Hide();
					hudView.Hide();
					gameOverView.Hide();
					EnableBackground();
					highScoreView.Show();
					messagesView.Hide();
					break;
			}
		}

		public void UpdateResume(int currentScore, int highscore)
		{
			gameOverView.SetPlayedScore(currentScore);
			gameOverView.SetHighScore(highscore); 
		}

		public void UpdateScore(int newScore){ hudView.UpdateScore(newScore); }
		public void UpdateLives(int lives) { hudView.UpdateLives(lives); }
		public void ShowMessage(string sms)
		{
			messagesView.UpdateContent(sms);
			messagesView.Show(); 
		}

		public void HideMessage()
		{
			messagesView.Hide();
			messagesView.UpdateContent(String.Empty);
		}

		public void DisplayHud() { hudView.DisplayHud(); }

        void CreateBackground()
        {
            Transform parent =  GameObject.Find("_Dynamic").transform;
            m_starSystem = Instantiate(StarSystemPrefab, parent).GetComponent<ParticleSystem>();
            DisableBackground();
        }

        private void EnableBackground()
        {
            if (m_starSystem != null)
                m_starSystem.Play();
        }

        private void DisableBackground()
        {
            if (m_starSystem != null)
                m_starSystem.Stop();
            
        }
    }
}

