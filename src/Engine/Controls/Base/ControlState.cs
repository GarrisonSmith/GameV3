using Engine.Controls.Base.enums;
using Engine.View;
using System;
using System.Collections.Generic;

namespace Engine.Controls.Base
{
	/// <summary>
	/// Represents a control state.
	/// </summary>
	public class ControlState
	{
		/// <summary>
		/// Gets or sets the direction radians. 
		/// </summary>
		public float? DirectionRadians { get; set; }

		/// <summary>
		/// Gets or sets the active control actions.
		/// </summary>
		public Dictionary<ControlActionTypes, ControlAction> ActiveControlActions { get; set; }

		/// <summary>
		/// Initializes a new instance of the ControlState class.
		/// </summary>
		public ControlState()
		{
			DirectionRadians = null;
			this.ActiveControlActions = new Dictionary<ControlActionTypes, ControlAction>();
		}

		/// <summary>
		/// Updates the control state.
		/// </summary>
		public void UpdateControlState()
		{
			this.UpdateDirectionalMovement();

			if (this.ActiveControlActions.ContainsKey(ControlActionTypes.ZoomIn))
			{ 
				Camera.GetCamera().SmoothZoomIn();
			}

			if (this.ActiveControlActions.ContainsKey(ControlActionTypes.ZoomOut))
			{
				Camera.GetCamera().SmoothZoomOut();
			}
		}

		/// <summary>
		/// Updates the directional movement.
		/// </summary>
		private void UpdateDirectionalMovement()
		{
			bool upMovement = this.ActiveControlActions.ContainsKey(ControlActionTypes.Up);
			bool downMovement = this.ActiveControlActions.ContainsKey(ControlActionTypes.Down);
			bool leftMovement = this.ActiveControlActions.ContainsKey(ControlActionTypes.Left);
			bool rightMovement = this.ActiveControlActions.ContainsKey(ControlActionTypes.Right);

			if (upMovement && downMovement)
			{
				upMovement = false;
				downMovement = false;
			}

			if (leftMovement && rightMovement)
			{
				leftMovement = false;
				rightMovement = false;
			}

			if (upMovement)
			{
				if (leftMovement)
				{
					this.DirectionRadians = (float)(3 * Math.PI) / 4f;
				}
				else if (rightMovement)
				{
					this.DirectionRadians = (float)Math.PI / 4f;
				}
				else
				{
					this.DirectionRadians = (float)Math.PI / 2f;
				}
			}
			else if (downMovement)
			{
				if (leftMovement)
				{
					this.DirectionRadians = (float)(5 * Math.PI) / 4f;
				}
				else if (rightMovement)
				{
					this.DirectionRadians = (float)(7 * Math.PI) / 4f;
				}
				else
				{
					this.DirectionRadians = (float)(3 * Math.PI) / 2f;
				}
			}
			else if (leftMovement)
			{
				this.DirectionRadians = (float)Math.PI;
			}
			else if (rightMovement)
			{
				this.DirectionRadians = 0f;
			}
			else
			{
				this.DirectionRadians = null;
			}
		}
	}
}
