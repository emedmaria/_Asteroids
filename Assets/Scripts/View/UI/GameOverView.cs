using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class InputTextEventArgs : EventArgs
	{
		public string InputText { get; set; }
	}

	public class GameOverView : BaseView
    {
        [SerializeField]
        private LabelView playedScoreLabel; 

        [SerializeField]
        private LabelView highScoreLabel;

        [SerializeField]
        private LabelView playedScoreDisplay;

        [SerializeField]
        private LabelView highScoreDisplay;

        [SerializeField]
        private LabelView messageLabel;

        [SerializeField]
        private LabelView captionLabel;

        [SerializeField]
        private BaseInputFieldView inputField;

        [SerializeField]
        private BaseButtonView saveBtn;

        [SerializeField]
        private BaseButtonView backBtn;

        private string inputText;


        #region - Events To suscribe with
        /// <summary>
        /// Persistence Data
        /// </summary>
        public event EventHandler<InputTextEventArgs> SaveHighScore;
        protected void OnSaveHighScore(object sender, InputTextEventArgs e)
        {
            AudioManager.Instance.PlaySFX(SoundFXType.MenuButton);

            if (SaveHighScore != null)
                SaveHighScore(sender, e);
        }
        /// <summary>
        /// Ignore current score and go back to menu
        /// </summary>
        public event EventHandler BackToMenu;
        protected void OnBackToMenu(object sender, EventArgs e)
        {
            AudioManager.Instance.PlaySFX(SoundFXType.MenuButton);

            if (BackToMenu != null)
                BackToMenu(sender, e);
        }
        #endregion

        #region - MonoBehaviour methods
        private void Awake()
        {
            //	Ensure the Components are set 
            Assert.IsNotNull(playedScoreLabel, "[GameOverView] playedScoreLabel must be referenced in the inspector!");
            Assert.IsNotNull(highScoreLabel, "[GameOverView] highScoreLabel must be referenced in the inspector!");
            Assert.IsNotNull(playedScoreDisplay, "[GameOverView] playedScoreDisplay must be referenced in the inspector!");
            Assert.IsNotNull(highScoreDisplay, "[GameOverView] highScoreDisplay must be referenced in the inspector!");
            Assert.IsNotNull(messageLabel, "[GameOverView] messageLabel must be referenced in the inspector!");
            Assert.IsNotNull(inputField, "[GameOverView] inputField must be referenced in the inspector!");
            Assert.IsNotNull(saveBtn, "[GameOverView] saveBtn must be referenced in the inspector!");
            Assert.IsNotNull(backBtn, "[GameOverView] backBtn must be referenced in the inspector!");
        }

		private void OnEnable() { AddListeners(); }
		private void OnDisable() { Reset(); }
		private void OnDestroy() { Reset(); }
		private void Reset()
		{
			inputText = string.Empty;
			RemoveListeners();
		}
        #endregion

        public void Init()
        {
			playedScoreLabel.Text = "SCORED POINTS";
			highScoreLabel.Text = "HIGH SCORE ";

			inputText = String.Empty;
            captionLabel.Hide();
            captionLabel.Text = "Introduce a valid Name!!";

			//  TODO: Set Texts From Cfg

			//AddListeners();
			backBtn.Enable();
			saveBtn.Enable(); 
		}

		public void SetPlayedScore(int value) { playedScoreDisplay.Text = value.ToString("D4"); }
		public void SetHighScore(int value) { highScoreDisplay.Text = value.ToString("D4"); }

		private void AddListeners()
		{
			inputField.InputChanged += (s, e) => { inputText = e.InputText; };

			saveBtn.AddListener(CheckInputField);
			backBtn.AddListener(OnBackToMenu);
		}

		private void RemoveListeners()
		{
			inputField.InputChanged -= (s, e) => { inputText = e.InputText; };

			saveBtn.RemoveListener(CheckInputField);
			backBtn.RemoveListener(OnBackToMenu);
		}

		private void CheckInputField(object sender, EventArgs e)
        {
            InputTextEventArgs ie = new InputTextEventArgs();
			ie.InputText = inputText; 

			if (String.IsNullOrEmpty(inputText))
                captionLabel.Show();
			else
			{
				captionLabel.Hide();
				OnSaveHighScore(sender, ie);
			}
		}
    }
}

