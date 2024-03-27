using Engine.Controls.Base.enums;
using Microsoft.Xna.Framework.Input;
using System;

namespace Engine.Controls.Base
{
	/// <summary>
	/// Represents a ControlActionTypeMap.
	/// </summary>
	public static class ControlActionTypeMap
	{
		/// <summary>
		/// Maps the enumeration value to a control action type.
		/// </summary>
		/// <typeparam name="T">The enumeration type.</typeparam>
		/// <param name="value">The value.</param>
		/// <returns>The control action type.</returns>
		public static ControlActionTypes Map<T>(T value) where T : struct, Enum
		{
			if (value is Keys key)
			{
				return ControlActionTypeMap.Map(key);
			}
			else if (value is MouseButtons mouseButton)
			{ 
				return ControlActionTypeMap.Map(mouseButton);
			}

			return ControlActionTypes.None;
		}

		/// <summary>
		/// Maps the key to a control action type.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>The control action type.</returns>
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

		/// <summary>
		/// Maps the mouse button to the control action type.
		/// </summary>
		/// <param name="button">The mouse button.</param>
		/// <returns>The control action type.</returns>
		public static ControlActionTypes Map(MouseButtons button)
		{
			switch (button)
			{
				case MouseButtons.Left: return ControlActionTypes.LeftClick;

				case MouseButtons.Middle: return ControlActionTypes.MiddleClick;

				case MouseButtons.Right: return ControlActionTypes.RightClick;

				default: return ControlActionTypes.None;
			}
		}
	}
}
