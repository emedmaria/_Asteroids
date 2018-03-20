using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AsteroidsClone
{
	public class PlayerInputHandler : MonoBehaviourSingletonPersistent<PlayerInputHandler>
	{
		private Dictionary<PlayerInputControls.ActionType, EventHandler<PlayerInputEventArgs>> m_playerInputEvents;
		private PlayerInputControls m_inputControls;

		private void Init()
		{
			m_playerInputEvents = new Dictionary<PlayerInputControls.ActionType, EventHandler<PlayerInputEventArgs>>();

			//	Add bindings to keys and/or buttons
			m_inputControls = new PlayerInputControls();
			m_inputControls.BindButtonInputAction(PlayerInputControls.ActionType.Fire, KeyCode.Space);
			m_inputControls.BindAxisInputAction(PlayerInputControls.ActionType.Thurst, "Vertical");
			m_inputControls.BindAxisInputAction(PlayerInputControls.ActionType.Turn, "Horizontal");
		}

		public static void StartListening(PlayerInputControls.ActionType actionType, EventHandler<PlayerInputEventArgs> listener)
		{
			EventHandler<PlayerInputEventArgs> currentEvent;

			if (Instance.m_playerInputEvents.TryGetValue(actionType, out currentEvent))
			{
				//	Add one event to the existing one
				currentEvent += listener;

				//	Update the Dictionary
				Instance.m_playerInputEvents[actionType] = currentEvent;
			}
			else
			{
				//	Add event to the Dictionary first time
				currentEvent += listener;
				Instance.m_playerInputEvents.Add(actionType, currentEvent);
			}
		}

		public static void StopListening(PlayerInputControls.ActionType actionType, EventHandler<PlayerInputEventArgs> listener)
		{
			if (Instance == null) return;

			EventHandler<PlayerInputEventArgs> currentEvent;
			if (Instance.m_playerInputEvents.TryGetValue(actionType, out currentEvent))
			{
				// Remove event from the existing one
				currentEvent -= listener;

				// Update the Dictionary
				Instance.m_playerInputEvents[actionType] = currentEvent;
			}
		}

		public static void TriggerEvent(PlayerInputControls.ActionType actionType)
		{
			EventHandler<PlayerInputEventArgs> currentEvent = null;
			var playerInputEvents = Instance.m_playerInputEvents;

			if (playerInputEvents.TryGetValue(actionType, out currentEvent))
			{
				var e = new PlayerInputEventArgs();
				e.ActionType = actionType;
				e.SourceInputControl = Instance.m_inputControls.GetInputControl(actionType);
				playerInputEvents[actionType](Instance, e);
			}
		}

		#region MOnoBehaviour methods
		public override void Awake()
		{
			base.Awake();
			Init();
		}
	
		void Update () {

			if (!m_inputControls.ActiveActionMaps()) return;

			//	We can loop through the Input Controls instead and Trigger the suitable Event in case is held

			//	Fire Action
			if (m_inputControls.Fire.IsHeld)
				TriggerEvent(PlayerInputControls.ActionType.Fire);
			

			//	Move Action
			if (m_inputControls.Thurst.IsHeld)
				TriggerEvent(PlayerInputControls.ActionType.Thurst);
			

			//	Turn Action
			if (m_inputControls.Turn.IsHeld)
				TriggerEvent(PlayerInputControls.ActionType.Turn);
		}
		#endregion
	}
}
