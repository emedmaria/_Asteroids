using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
    [CreateAssetMenu(fileName = "New ScreenWrapper Settings", menuName = "__Asteroids/Settings/ScreenWrapperSettings")]
    public class ScreenWrapperSettings : ScriptableObject
    {
        public float PaddingTop = 0.1f;
        public float PaddingBottom = 0.1f;
        public float PaddingLeft = 0.1f;
        public float PaddingRight = 0.1f;

        public bool DynamicShrink = false;
        public bool EnableHorizontalWrap = true;
        public bool EnableVerticalWrap = true; 
    }
}

