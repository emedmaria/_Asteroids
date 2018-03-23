using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class HudView : BaseView
	{
		[SerializeField]
		[Header ("Score Display")]
		private LabelView scoreLabel;
		[SerializeField]
		[Header("Remaining Lives")]
		private LabelView livesLabel;

		#region MonoBehaviour methods
		void Awake()
		{
			scoreLabel.Hide();
			livesLabel.Hide();
		}

		#endregion

		private void Init() {

			//	TODO: Fill with Texts form Cfg
			//	Reset Score
			//scoreLabel.Text = "000";
			//	Restart Lives - // TODO: Get From CFg
			//livesLabel.Text = "3"; 

			//StartCoroutine(DisplayRound());
		}

		public void UpdateScore(int newScore) {
			scoreLabel.Text = "POINTS: "+newScore.ToString("D4");
		}
		public void UpdateLives(int lives) {
			livesLabel.Text = "LIVES: "+lives.ToString("D3");
			//livesLabel.TriggerFailColorAnimation();
		}

		public void DisplayHud()
		{
			scoreLabel.Show();
			livesLabel.Show();
		}
	}
}

