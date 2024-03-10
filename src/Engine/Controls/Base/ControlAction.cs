using Engine.Controls.Base.enums;
using Microsoft.Xna.Framework.Input;

namespace Engine.Controls.Base
{
	/// <summary>
	/// Represents a ControlAction.
	/// </summary>
	public class ControlAction
	{
		/// <summary>
		/// Gets or sets a value indicating whether this control action just started.
		/// </summary>
		public bool JustStarted { get; set; }

		/// <summary>
		/// Gets or sets start time in milliseconds.
		/// </summary>
		public double StartTime { get; set; }

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		public Keys Key { get; set; }

		/// <summary>
		/// Gets or sets the control action type.
		/// </summary>
		public ControlActionTypes ControlActionType { get; set; }

		/// <summary>
		/// Initializes a new instance of the ControlAction class.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		/// <param name="key">The key.</param>
		/// <param name="controlActionType">The control action type.</param>
		public ControlAction(double startTime, Keys key, ControlActionTypes controlActionType)
		{ 
			this.StartTime = startTime;
			this.Key = key;
			this.ControlActionType = controlActionType;
		}
	}
}
