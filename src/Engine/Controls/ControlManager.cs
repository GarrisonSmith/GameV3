using Engine.Controls.Base;
using Engine.Controls.Base.enums;
using Engine.Controls.Base.interfaces;
using Engine.Loading.Base.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Engine.Controls
{
	/// <summary>
	/// Represents a control manager.
	/// </summary>
	public class ControlManager : GameComponent, ICanBeLoaded
	{
		/// <summary>
		/// Start the control manager.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The control manager.</returns>
		public static ControlManager StartControlManager(Game game)
		{
			return Managers.ControlManager ?? new ControlManager(game);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the control state.
		/// </summary>
		public ControlState ControlState { get; set; }

		/// <summary>
		/// Gets or sets the held control actions.
		/// </summary>
		public Dictionary<ControlActionTypes, IControlAction> HeldControlActions { get; set; }

		/// <summary>
		/// Gets or sets the new control actions.
		/// </summary>
		public Dictionary<ControlActionTypes, IControlAction> NewControlActions { get; set; }

		/// <summary>
		/// Initializes a new instance of the ControlManager class.
		/// </summary>
		/// <param name="game">The game.</param>
		private ControlManager(Game game) : base(game)
		{

		}

		/// <summary>
		/// Initializes the content.
		/// </summary>
		public override void Initialize()
		{
			this.HeldControlActions = new Dictionary<ControlActionTypes, IControlAction>();
			this.NewControlActions = new Dictionary<ControlActionTypes, IControlAction>();
			this.ControlState = new ControlState();
		}

		/// <summary>
		/// Updates the content. TODO this can be optimized. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			if (this.NewControlActions.Count > 0)
			{
				foreach (var pair in this.NewControlActions)
				{
					pair.Value.JustStarted = false;
					this.HeldControlActions.Add(pair.Key, pair.Value);
					this.NewControlActions.Remove(pair.Key);
				}

				this.NewControlActions.Clear();
			}

			var stillHeldControlActions = new List<IControlAction>();
			foreach (var key in Keyboard.GetState().GetPressedKeys())
			{
				var controlActionType = ControlActionTypeMap.Map(key);
				stillHeldControlActions.Add(this.CheckAddControlActionType(key, controlActionType, gameTime));
			}

			foreach (var mouseButton in this.GetPressedMouseButtonsFromMouseState())
			{
				var controlActionType = ControlActionTypeMap.Map(mouseButton);
				stillHeldControlActions.Add(this.CheckAddControlActionType(mouseButton, controlActionType, gameTime));
			}

			foreach (var controlAction in this.HeldControlActions)
			{
				if (!stillHeldControlActions.Contains(controlAction.Value))
				{
					this.HeldControlActions.Remove(controlAction.Key);
					this.ControlState.ActiveControlActions.Remove(controlAction.Key);
				}
			}

			this.ControlState.UpdateControlState();
		}

		/// <summary>
		/// Check adding the control action type.
		/// </summary>
		/// <typeparam name="T">The type of the button.</typeparam>
		/// <param name="button">The button.</param>
		/// <param name="controlActionType">The control action type.</param>
		/// <param name="gameTime">The game time.</param>
		/// <returns>The control action.</returns>
		private IControlAction CheckAddControlActionType<T>(T button, ControlActionTypes controlActionType, GameTime gameTime) where T : struct, Enum
		{
			if (!this.HeldControlActions.ContainsKey(controlActionType))
			{
				var controlAction = new ControlAction<T>(gameTime.TotalGameTime.TotalMilliseconds, button, true, controlActionType);
				this.NewControlActions.Add(controlActionType, controlAction);
				this.ControlState.ActiveControlActions.Add(controlActionType, controlAction);
				return controlAction;
			}

			return this.HeldControlActions[controlActionType];
		}

		/// <summary>
		/// Gets the pressed mouse buttons from the current mouse state.
		/// </summary>
		/// <returns>A list of pressed mouse buttons.</returns>
		private List<MouseButtons> GetPressedMouseButtonsFromMouseState()
		{ 
			var mouseButtons = new List<MouseButtons>();
			var mouseState = Mouse.GetState();

			if (mouseState.LeftButton == ButtonState.Pressed)
			{
				mouseButtons.Add(MouseButtons.Left);
			}
			else if (mouseState.MiddleButton == ButtonState.Pressed)
			{
				mouseButtons.Add(MouseButtons.Middle);
			}
			else if (mouseState.RightButton == ButtonState.Pressed)
			{
				mouseButtons.Add(MouseButtons.Right);
			}

			return mouseButtons;
		}

		/// <summary>
		/// Loads data.
		/// </summary>`
		public void Load()
		{
			this.IsLoaded = true;
		}
	}
}
