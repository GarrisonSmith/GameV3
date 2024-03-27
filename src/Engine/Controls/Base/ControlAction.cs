using Engine.Controls.Base.enums;
using Engine.Controls.Base.interfaces;
using System;

namespace Engine.Controls.Base
{
	/// <summary>
	/// Represents a ControlAction.
	/// </summary>
	public class ControlAction<T>: IControlAction where T : struct, Enum
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
		/// Gets or sets the button.
		/// </summary>
		public T Button { get; set; }

		/// <summary>
		/// Gets or sets the control action type.
		/// </summary>
		public ControlActionTypes ControlActionType { get; set; }

		/// <summary>
		/// Initializes a new instance of the ControlAction class.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		/// <param name="button">The button.</param>
		/// <param name="justStarted">A value describing if the control action just started.</param>
		/// <param name="controlActionType">The control action type.</param>
		public ControlAction(double startTime, T button, bool justStarted, ControlActionTypes controlActionType)
		{
			this.StartTime = startTime;
			this.Button = button;
			this.JustStarted = justStarted;
			this.ControlActionType = controlActionType;
		}
	}
}
