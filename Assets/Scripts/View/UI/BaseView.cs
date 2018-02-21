using System;
using UnityEngine;

namespace AsteroidsClone
{
	public class BaseView: MonoBehaviour, IViewDisplay
	{
		#region Monobehaviour methods
		void OnDestroy()
		{
			// Null Suscriptions references
			ViewHidden = null;
			ViewShown = null;
		}
		#endregion 

		#region - Events to suscribe with
		public event EventHandler ViewHidden;
		//Event publisher method - OnRaise
		protected virtual void OnViewHidden()
		{
			//Check whether are suscriptors 
			if (ViewHidden != null)
				ViewHidden(this, EventArgs.Empty);
		}
		public event EventHandler ViewShown;
		//Event publisher method - OnRaise
		protected virtual void OnViewShown()
		{
			//Check whether are suscriptors 
			if (ViewShown != null)
				ViewShown(this, EventArgs.Empty);
		}
		#endregion

		public virtual Transform ViewRoot {
			get{ return gameObject.GetComponent<Transform>();}
		}

		#region - Implementation of IDisplay interface methods
		public virtual void Show()
		{
			ViewRoot.gameObject.SetActive(true);
			OnViewShown();
		}

		public virtual void Hide()
		{
			ViewRoot.gameObject.SetActive(false);
			OnViewHidden();
		}
		#endregion
	}
}
