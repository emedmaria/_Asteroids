using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AsteroidsClone
{
	public class MainMenuView : BaseView
	{
		// Components of the view (Set in the inspector)
		[Header("Title")]
		public LabelView labelView;
		public BaseButtonView playBtn;
		public BaseButtonView highScoreBtn;

		#region -Events to suscribe with
		public event EventHandler PlayRaised; 
		protected virtual void OnPlayRaised(object sender, EventArgs e)
		{
            AudioManager.Instance.PlaySFX(SoundFXType.MenuButton);
			if (PlayRaised != null)
				PlayRaised(this,EventArgs.Empty); 

		}

		public event EventHandler HighScoreRaised;
		protected virtual void OnHighScoreRaised(object sender, EventArgs e)
		{
            AudioManager.Instance.PlaySFX(SoundFXType.MenuButton);

            if (HighScoreRaised != null)
				HighScoreRaised(this, EventArgs.Empty);
		}
		#endregion

		#region - MonoBehaviour methods
		void Awake()
		{
			//	TODO: Refactor - Save references to widgets dynamically. Do not rely on strings
			// Save references - Set in the inspector instead
			/*labelView = GetComponentInChildren<LabelView>();
			playBtn = transform.Find("ButtonsPanel/PlayBtn").GetComponent<BaseButtonView>();
			highScoreBtn = transform.Find("ButtonsPanel/HighScoreBtn").GetComponent<BaseButtonView>();
			*/
		}
		void OnEnable() { AddListeners(); }
		void OnDisable() { RemoveListeners(); }
		#endregion

		public void Init()
		{
			playBtn.Enable();
			highScoreBtn.Enable();

			//	TODO: Load texts
		}

		private void AddListeners()
		{
			playBtn.AddListener(OnPlayRaised);
			highScoreBtn.AddListener(OnHighScoreRaised);
		}

		private void RemoveListeners()
		{
			playBtn.RemoveListener(OnPlayRaised);
			highScoreBtn.RemoveListener(OnHighScoreRaised);
		}
	}
}

