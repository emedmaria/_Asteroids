using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
    public class ScreenWrapper : MonoBehaviour
    {
		// Events to suscribe with
		public event EventHandler OffScreen;
		protected virtual void OnOffScreeen()
		{
			if (OffScreen != null)
				OffScreen(this, EventArgs.Empty);
		}
		public ScreenWrapperSettings WrapperSettings;

        private Vector3 m_viewPortPosition; 
        private bool m_isOffScreen;
        private bool m_enableHorizontalWrap;
        private bool m_enableVerticalWrap; 

        private float m_top;
        private float m_bottom;
        private float m_left;
        private float m_right;

        //private Renderer m_renderer = null;

        #region MonoBehaviour methods
        void OnValidate()
        {
            //  TODO: Maybe is better to load the default one in case of null
            Assert.IsNotNull(WrapperSettings, this + " ScreenWrapper component requires WrapperSettings referenced via inspector! ");
        }

        private void Awake()
        {
           // m_renderer = GetComponent<Renderer>();
            m_enableHorizontalWrap = WrapperSettings.EnableHorizontalWrap;
            m_enableVerticalWrap = WrapperSettings.EnableVerticalWrap;

            //  Calculate boundaries
            CalculateBoundaries();
        }
        void Update()
        {
            CheckBoundaries();
            if (!m_isOffScreen) return;
            transform.position = Camera.main.ViewportToWorldPoint(m_viewPortPosition);
        }
        #endregion

        private void CalculateBoundaries()
        {
            m_top = 0.0f - WrapperSettings.PaddingTop;
            m_bottom = 1.0f + WrapperSettings.PaddingBottom;
            m_left = 0.0f - WrapperSettings.PaddingLeft;
            m_right = 1.0f + WrapperSettings.PaddingRight;
        }

        private void CheckBoundaries()
        {

#if UNITY_EDITOR
            CalculateBoundaries();
#endif
            // convert transform position to viewport position.
            m_viewPortPosition = Camera.main.WorldToViewportPoint(transform.position);

            m_isOffScreen = false;

            if (m_enableHorizontalWrap)
            {
                // LEFT/RIGHT
                if (m_viewPortPosition.x < m_left)
                {
                    m_viewPortPosition.x = m_right;
                    m_isOffScreen = true;
                }
                else if (m_viewPortPosition.x > m_right)
                {
                    m_viewPortPosition.x = m_left;
                    m_isOffScreen = true;
                }
            }

            if (m_enableVerticalWrap)
            {
                // TOP/BOTTOM
                if (m_viewPortPosition.y < m_top)
                {
                    m_viewPortPosition.y = m_bottom;
                    m_isOffScreen = true;
                }
                else if (m_viewPortPosition.y > m_bottom)
                {
                    m_viewPortPosition.y = m_top;
                    m_isOffScreen = true;
                }
            }

			if (m_isOffScreen &&  OffScreen!=null)
				OnOffScreeen();
        }
    }
}

