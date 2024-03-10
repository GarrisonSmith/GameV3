using Engine.Controls.Base;
using Engine.Controls.Base.enums;
using Engine.Loading.Base.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

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
		public Dictionary<ControlActionTypes, ControlAction> HeldControlActions { get; set; }

		/// <summary>
		/// Gets or sets the new control actions.
		/// </summary>
		public Dictionary<ControlActionTypes, ControlAction> NewControlActions { get; set; }

		/// <summary>
		/// Initializes a new instance of the ControlManager class.
		/// </summary>
		/// <param name="game">The game.</param>
		private ControlManager(Game game) : base(game)
		{
			this.HeldControlActions = new Dictionary<ControlActionTypes, ControlAction>();
			this.NewControlActions = new Dictionary<ControlActionTypes, ControlAction>();
			this.ControlState = new ControlState();
		}

		/// <summary>
		/// Initializes the content.
		/// </summary>
		public override void Initialize()
		{

		}

		/// <summary>
		/// Updates the content.
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
				}

				this.NewControlActions.Clear();
			}

			var releasedHeldKeys = this.HeldControlActions.Keys.ToList();
			foreach (var key in Keyboard.GetState().GetPressedKeys())
			{
				var controlActionType = ControlActionTypeMap.Map(key);
				if (this.HeldControlActions.ContainsKey(controlActionType))
				{ 
					releasedHeldKeys.Remove(controlActionType);
				}
				else if (!this.NewControlActions.ContainsKey(controlActionType))
				{
					var controlAction = new ControlAction(gameTime.TotalGameTime.TotalMilliseconds, key, controlActionType);
					this.NewControlActions.Add(controlActionType, controlAction);
					this.ControlState.ActiveControlActions.Add(controlActionType, controlAction);
				}
			}

			foreach (var releasedHeldKey in releasedHeldKeys)
			{ 
				this.HeldControlActions.Remove(releasedHeldKey);
				this.ControlState.ActiveControlActions.Remove(releasedHeldKey);
			}

			this.ControlState.UpdateControlState();
		}

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			this.IsLoaded = true;
		}
	}
}
