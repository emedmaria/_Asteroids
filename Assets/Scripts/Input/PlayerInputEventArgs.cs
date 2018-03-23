using System;

namespace AsteroidsClone
{
	public class PlayerInputEventArgs : EventArgs
	{
		public PlayerInputControls.ActionType ActionType { get; set; }
		public InputControl SourceInputControl { get; set; }
	}
}
