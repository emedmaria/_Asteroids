using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsClone
{
	public class MessagesView : BaseView
	{
		[SerializeField]
		private LabelView messageLabel;

		private float m_appearanceTs = 32f;
		private WaitForSeconds m_appearance;

		//void Awake() { UpdateContent(string.Empty); }
		void OnEnable()
		{
			if(m_appearance == null)
				m_appearance = new WaitForSeconds(m_appearanceTs);
		}

		public void UpdateContent(string sms) { messageLabel.Text = sms; }

		public IEnumerator DisplayMessage(string sms)
		{
			UpdateContent(sms);
			messageLabel.Show();
			yield return new WaitForSeconds(m_appearanceTs);
			messageLabel.Hide();
		}
	}

}

