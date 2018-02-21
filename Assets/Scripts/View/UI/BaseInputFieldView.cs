using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidsClone
{
    [RequireComponent(typeof(InputField))]
    public class BaseInputFieldView : BaseView {

        private InputField inputField;
        private string inputText; 

        #region Events to suscribe with
        public event EventHandler<InputTextEventArgs> InputChanged;
        protected virtual void OnInputChanged(object sender, InputTextEventArgs e)
        {
            if (InputChanged != null)
                InputChanged(this, e);
        }

        protected virtual void WireUIEvents()
        {
            if (inputField.onValueChanged.GetPersistentEventCount() <= 0)
                inputField.onValueChanged.AddListener(OnInputFieldChanged);

            //	Add more handlers if needed
        }

        private void OnInputFieldChanged(string arg)
        {
			InputTextEventArgs e = new InputTextEventArgs();
			e.InputText = arg;
            OnInputChanged(this, e);
        }

        protected virtual void UnWireUIEvents() { inputField.onValueChanged.RemoveAllListeners(); }
        #endregion

        #region - MonoBehaviour methods
        void Awake()
        {
            // Save references of the components needed 
            inputField = GetComponentInChildren<InputField>();
			inputText = inputField.text;

            WireUIEvents();
        }
        #endregion

        #region Properties/Fields
        public string InputText { get { return inputText; } }
        #endregion
    }
}