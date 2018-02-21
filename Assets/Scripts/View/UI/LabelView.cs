using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsClone
{
	[RequireComponent(typeof(Text))]
	public class LabelView : BaseView
	{
		private Text LabelText;

		#region Monobehaviour methods
		void Awake()
		{
			// Save references
			LabelText = GetComponentInChildren<Text>();
		}

		void OnDestroy()
		{
			// Null Event suscriptions
			LabelChanged = null;
		}
		#endregion

		#region Fields and properties
		public string Text
		{
			get
			{
				return LabelText != null ? LabelText.text : string.Empty;
			}
			set
			{
				if (LabelText == null)
					LabelText = GetComponentInChildren<Text>();

				LabelText.text = value;
			}
		}

		public Color TextColor
		{
			get { return LabelText.color; }
			set { LabelText.color = value; }
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
