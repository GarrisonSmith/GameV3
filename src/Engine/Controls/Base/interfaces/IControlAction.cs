using Engine.Controls.Base.enums;

namespace Engine.Controls.Base.interfaces
{
	/// <summary>
	/// Represents a control action.
	/// </summary>
	public interface IControlAction
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
		/// Gets or sets the control action type.
		/// </summary>
		public ControlActionTypes ControlActionType { get; set; }
	}
}
