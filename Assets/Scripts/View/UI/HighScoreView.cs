using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
	public class HighScoreView : BaseView
	{
		const string SPACE_STR = " ";
		const string DASH_STR = "-";

		[SerializeField]
		private LabelView titleLabel;

		[SerializeField]
		private LabelView namesLabel;
		[SerializeField]
		private LabelView namesContentLabel;

		[SerializeField]
		private LabelView scoresLabel;
		[SerializeField]
		private LabelView scoresContentLabel;

		[SerializeField]
		private LabelView messagesLabel;

		[SerializeField]
		private BaseButtonView backBtn;

		private List<GameState.PlayerState> top10;
		public List<GameState.PlayerState> Top10 { private get; set; }

		// Events to suscribe with
		public event EventHandler BackRaised;
		protected virtual void OnBackRaised(object sender, EventArgs e)
		{
            AudioManager.Instance.PlaySFX(SoundFXType.MenuButton);

            if (BackRaised != null)
				BackRaised(this, EventArgs.Empty);
		}

		void Awake()
		{
			//	Ensure the Components are referenced in the inspector
			Assert.IsNotNull(titleLabel, "[HighScoreView] titleLabel must be referenced in the inspector!");
			Assert.IsNotNull(namesLabel, "[HighScoreView] namesLabel must be referenced in the inspector!");
			Assert.IsNotNull(namesContentLabel, "[HighScoreView] namesContentLabel must be referenced in the inspector!");
			Assert.IsNotNull(scoresLabel, "[HighScoreView] scoresLabel must be referenced in the inspector!");
			Assert.IsNotNull(scoresContentLabel, "[HighScoreView] scoresContentLabel must be referenced in the inspector!");
			Assert.IsNotNull(messagesLabel, "[HighScoreView] messagesLabel must be referenced in the inspector!");
			Assert.IsNotNull(messagesLabel, "[HighScoreView] backBtn must be referenced in the inspector!");

			//Init(); 
		}
		void OnEnable() { backBtn.AddListener(OnBackRaised); }
		void OnDisable() { backBtn.RemoveListener(OnBackRaised); }

		public void Init(List<GameState.PlayerState> top10)
		{
			Top10 = top10; 

			//	TODO: Get From CFg
			titleLabel.Text = " HIGH SCORES ";

			namesLabel.Text = "PLAYERS";
			namesContentLabel.Text = String.Empty; 
			scoresLabel.Text = "SCORES";
			scoresContentLabel.Text = String.Empty;

			messagesLabel.Text = "There aren't scores yet...";
			messagesLabel.Hide();

			backBtn.Enable();

			UpdateContent(); 
		}

		public override void Show()
		{
			UpdateContent(); 
			base.Show();
		}

		private void UpdateContent()
		{
			int nScores = (Top10 == null ? 0 : Top10.Count);

			if (nScores == 0)
			{
				namesLabel.Hide();
				scoresLabel.Hide();
				messagesLabel.Show();
			}
			else {
				namesLabel.Show();
				scoresLabel.Show();
				messagesLabel.Hide();
			}

			namesContentLabel.Text = String.Empty; 
			scoresContentLabel.Text = String.Empty;

			for (int i=0; i<nScores; i++)
			{
				GameState.PlayerState currentScore = Top10[i];
				AddPlayer((i + 1), currentScore.Name);
				AddPlayerScore(currentScore.HighScore);
			}
		}

		private void AddPlayer(int ranking, string name)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(ranking.ToString("D2"));
			sb.Append(DASH_STR);
			sb.Append(name);
			sb.Append(Environment.NewLine);
			namesContentLabel.Text += sb.ToString(); 
		}

		private void AddPlayerScore(int score)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(". . . . . . . . . . . . . . . .");
			sb.Append(score.ToString("D4"));
			sb.Append(Environment.NewLine);
			scoresContentLabel.Text += sb.ToString();
		}
	}
}

