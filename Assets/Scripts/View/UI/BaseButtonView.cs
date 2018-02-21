using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsClone
{
	[RequireComponent(typeof(Button))]
	public class BaseButtonView : BaseView {

		private Button Button;
		private Text ButtonLabel;

		#region - MonoBehaviour methods
		void Awake()
		{
			// Save references of the components needed (in case not added via Inspector)
			Button = GetComponentInChildren<Button>();
			ButtonLabel = Button.GetComponentInChildren<Text>();

			WireUIEvents();
		}
		#endregion

		#region Properties/Fields
		public string Text
		{
			get { return ButtonLabel != null ? ButtonLabel.text : string.Empty; }
			set
			{
				if (ButtonLabel == null)
					return;

				ButtonLabel.text = value;
			}
		}
		/// <summary>
		/// To keep track the status of the button in order to disable
		/// the events if the button is disabled
		/// </summary>
		public bool IsEnabled
		{
			get;
			private set;
		}

		public void Enable()
		{
			IsEnabled = true;
		}

		public void Disable()
		{
			IsEnabled = false;
		}
		#endregion

		#region Events to suscribe with
		public event EventHandler Clicked;
		protected virtual void OnClicked()
		{
			//Debug.Log("Button " + gameObject.name + " Clicked");
			// Do not propage event when Disabled
			if (!IsEnabled)
				return;

			if (Clicked != null)
				Clicked(this, new EventArgs());
		}
		public void AddListener(EventHandler listener)
		{
			Clicked += listener;
		}
		public void RemoveListener(EventHandler listener)
		{
			Clicked -= listener;
		}

		protected virtual void WireUIEvents()
		{
			// Programatically add the onClick handler if it is not set
			if (Button.onClick.GetPersistentEventCount() <= 0)
				Button.onClick.AddListener(OnClicked);
        }

		protected virtual void UnWireUIEvents()
		{
			Button.onClick.RemoveAllListeners();
		}
		#endregion
	}
}
