using Engine.Controls.Base.enums;
using Engine.Loading.Base.interfaces;
using Engine.Physics.Areas;
using Engine.UI.Base;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.UI
{
	/// <summary>
	/// Represents a UI manager. 
	/// </summary>
	public class UiManager : GameComponent, ICanBeLoaded
	{
		/// <summary>
		/// Starts the UI manager.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The UI manager.</returns>
		public static UiManager StartUiManager(Game game)
		{
			return Managers.UiManager ?? new UiManager(game);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the UI elements.
		/// </summary>
		public Dictionary<Guid, UiElement> UiElements { get; set; }

		/// <summary>
		/// Gets or sets the UI elements by the hover area.
		/// </summary>
		public Dictionary<OffsetArea, UiElement> UiElementsByHoverArea { get; set; }

		/// <summary>
		/// Gets the hovered UI element.
		/// </summary>
		public UiElement HoveredUiElement
		{
			get
			{
				foreach (var area in this.UiElementsByHoverArea.Keys)
				{
					if (area.Contains(Managers.ControlManager.ControlState.MousePosition))
					{
						return this.UiElementsByHoverArea[area];
					}
				}

				return null;
			}
		}

		/// <summary>
		/// Gets or sets the existing hovered UI element.
		/// </summary>
		private UiElement ExistingHoveredUiElement { get; set; }

		/// <summary>
		/// Initializes a new instance of the UiManager class.
		/// </summary>
		/// <param name="game">The game</param>
		private UiManager(Game game) : base(game)
		{
			this.UiElements = new Dictionary<Guid, UiElement>();
			this.UiElementsByHoverArea = new Dictionary<OffsetArea, UiElement>();
		}

		/// <summary>
		/// Updates the content.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			var hoveredUiElement = this.HoveredUiElement;
			if (this.ExistingHoveredUiElement != null && hoveredUiElement == null && this.ExistingHoveredUiElement != hoveredUiElement)
			{
				this.ExistingHoveredUiElement.BeingHovered = false;
			}
			else if (hoveredUiElement != null)
            {
                hoveredUiElement.BeingHovered = true;
				this.ExistingHoveredUiElement = hoveredUiElement;
            }

			if (hoveredUiElement is Button button && Managers.ControlManager.ControlState.ActiveControlActions.ContainsKey(ControlActionTypes.LeftClick))
			{
				button.OnClick();
			}

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
