using Engine.Controls.Base.enums;
using Microsoft.Xna.Framework.Input;

namespace Engine.Controls.Base
{
	/// <summary>
	/// Represents a ControlActionTypeMap.
	/// </summary>
	public static class ControlActionTypeMap
	{
		/// <summary>
		/// Maps the key to a control action type.
		/// </summary>
		/// <param name="key">The key.</param>
		public static ControlActionTypes Map(Keys key)
		{ 
			switch (key)
			{
				case Keys.W: return ControlActionTypes.Up;
				
				case Keys.A: return ControlActionTypes.Left;

				case Keys.S: return ControlActionTypes.Down;

				case Keys.D: return ControlActionTypes.Right;

				case Keys.OemPlus: return ControlActionTypes.ZoomIn;

				case Keys.OemMinus: return ControlActionTypes.ZoomOut;

				default: return ControlActionTypes.None;
			}
		}
	}
}
