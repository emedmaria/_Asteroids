using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace AsteroidsClone
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseShipMovement : MonoBehaviour
	{
		public BaseShipSettings ShipSettings;

		// Refence to components
		private Rigidbody m_rb;

		// Input control
		private float m_thrustInput;
		private float m_turnInput;

        #region - MonoBehaviour methods
        private void OnValidate()
        {
            Assert.IsNotNull(ShipSettings, this + "BaseShipMovement requires BaseShipSettings referenced via inspector! ");
        }

        void Awake() {
			m_rb = GetComponent<Rigidbody>();
			//PlayerInputHandler.StartListening(PlayerInputControls.ActionType.Thurst, OnHandleMoveThurstAction);
		}

		void OnEnable() {
            Assert.IsNotNull(ShipSettings);
            Reset();
			PlayerInputHandler.StartListening(PlayerInputControls.ActionType.Thurst, OnHandleThurstAction);
			PlayerInputHandler.StartListening(PlayerInputControls.ActionType.Turn, OnHandleTurnAction);
		}

		void OnDisable()
		{
			PlayerInputHandler.StopListening(PlayerInputControls.ActionType.Thurst, OnHandleThurstAction);
			PlayerInputHandler.StopListening(PlayerInputControls.ActionType.Turn, OnHandleTurnAction);
		}

		void Destroy()
		{
			PlayerInputHandler.StopListening(PlayerInputControls.ActionType.Thurst, OnHandleThurstAction);
			PlayerInputHandler.StopListening(PlayerInputControls.ActionType.Turn, OnHandleTurnAction);
		}

		private void OnHandleThurstAction(object sender, PlayerInputEventArgs e)
		{
			var inputControl = e.SourceInputControl as AxisInputControl;
			if (inputControl == null) return; 
			m_thrustInput = Mathf.Clamp01(inputControl.AxisValue);
		}

		private void OnHandleTurnAction(object sender, PlayerInputEventArgs e)
		{
			var inputControl = e.SourceInputControl as AxisInputControl;
			if (inputControl == null) return;
			m_turnInput = inputControl.AxisValue;
		}

		/*void Update()
		{
			//	Update values according with the inputs - TODO; External inputs!!
			//m_turnInput = Input.GetAxis("Horizontal");
			//m_thrustInput = Mathf.Clamp01(Input.GetAxis("Vertical"));

		}*/

		void FixedUpdate() {
			Move();
			Turn();
			ClampSpeed();
		}
        #endregion

        #region Behaviour methods
        void Reset()
		{
			m_thrustInput = 0f;
			m_turnInput = 0f;
		}

		void ClampSpeed()
		{
            // Clamp down the rb velocity by the MaxSpeed given by the settings
			m_rb.velocity = Vector3.ClampMagnitude(m_rb.velocity, ShipSettings.MaxSpeed);
		}

		void Move()
		{
			// Vector force f(Input, speed, Frame Time )
			Vector3 thrustForce = m_thrustInput * ShipSettings.Rotation * Time.deltaTime * transform.up;
			m_rb.AddForce(thrustForce);
		}

		void Turn()
		{
			// Torque f(input, MaxRotation, Frame Time)
			float turn = m_turnInput * ShipSettings.Rotation * Time.deltaTime;
			Vector3 zTorque = -transform.forward * turn;
			m_rb.AddTorque(zTorque);
		}
        #endregion
    }
}

