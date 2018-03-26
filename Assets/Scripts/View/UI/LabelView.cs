using System;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsClone
{
	[RequireComponent(typeof(Text))]
	public class LabelView : BaseView
	{
		private const string SCALE_ANIMATION = "EnableScale";
		private const string FAIL_COLOR_ANIMATION = "EnableColorFail";

		private Text labelText;
		private Animator animator;

		#region Monobehaviour methods
		void Awake()
		{
			// Save references
			labelText = GetComponentInChildren<Text>();
			if (animator == null) animator = GetComponent<Animator>(); 
		}

		void OnDestroy()
		{
			// Null Event suscriptions
			LabelChanged = null;
		}
		#endregion

		/*public void TriggerScaleAnimation()
		{
			animator.SetBool(SCALE_ANIMATION, true);
		}

		public void TriggerFailColorAnimation()
		{
			animator.SetBool(FAIL_COLOR_ANIMATION, true);
		}*/

		#region Fields and properties
		public string Text
		{
			get
			{
				return labelText != null ? labelText.text : string.Empty;
			}
			set
			{
				if (labelText == null)
					labelText = GetComponentInChildren<Text>();

				labelText.text = value;
			}
		}

		public Color TextColor
		{
			get { return labelText.color; }
			set { labelText.color = value; }
		}
		#endregion

		#region Events to suscribe with
		public event EventHandler LabelChanged;
		protected virtual void OnLabelChanged()
		{
			if (LabelChanged != null)
				LabelChanged(this, EventArgs.Empty);
		}
		#endregion
	}
}
