using Engine.Drawing.Base;
using Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.UI.Base.enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.Base
{
	public class Button : DrawableUiElement
	{
		/// <summary>
		/// Gets or sets the button text.
		/// </summary>
		public string ButtonText { get; set; }

		/// <summary>
		/// Gets or sets the button text color.
		/// </summary>
		public Color ButtonTextColor { get; set; }

		/// <summary>
		/// Gets or sets the button text coordinates.
		/// </summary>
		public Vector2 ButtonTextCoordinates { get; set; }

		/// <summary>
		/// Gets or sets the button font.
		/// </summary>
		public SpriteFont ButtonFont { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public OffsetArea ClickableArea { get; set; }

		/// <summary>
		/// Initializes a new instance of the Button class.
		/// </summary>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="drawData">The draw data.</param>
		/// <param name="area">The area.</param>
		/// <param name="buttonBorderSize">The button border size.</param>
		/// <param name="buttonText">The button text.</param>
		/// <param name="buttonTextColor">The button text color.</param>
		/// <param name="buttonFont">The button font.</param>
		public Button(bool drawingActivated, ushort drawOrder, DrawData drawData, IAmAArea area, int buttonBorderSize, string buttonText, Color buttonTextColor, SpriteFont buttonFont)
			: base(UiElementTypes.Button, drawingActivated, drawOrder, drawData, area)
		{
			this.ButtonText = buttonText;
			this.ButtonTextColor = buttonTextColor;
			this.ButtonFont = buttonFont;
			this.ClickableArea = new OffsetArea(area.Position, buttonBorderSize, buttonBorderSize, area.Width - buttonBorderSize * 2, area.Height - buttonBorderSize * 2);
			this.ButtonTextCoordinates = this.GetButtonTextCoordinates(buttonText, area);
			Managers.UiManager.UiElementsByHoverArea.Add(this.ClickableArea, this);
		}

		/// <summary>
		/// When the UI element is first hovered.
		/// </summary>
		public override void OnHover()
		{

		}

		/// <summary>
		/// When the UI element is being hovered.
		/// </summary>
		public override void OnHovering()
		{

		}

		/// <summary>
		/// Process button clicks.
		/// </summary>
		public void OnClick()
		{

		}

		/// <summary>
		/// Draws the UI element.
		/// </summary>
		public override void Draw()
		{
			base.Draw();
			Managers.DrawManager.Write(this.ButtonFont, this.ButtonText, this.ButtonTextCoordinates, this.ButtonTextColor);
		}

		/// <summary>
		/// Gets the button text coordinates.
		/// </summary>
		/// <param name="buttonText">The button text.</param>
		/// <param name="area">The button area.</param>
		/// <returns>The coordinates of the button text.</returns>
		private Vector2 GetButtonTextCoordinates(string buttonText, IAmAArea area)
		{
			var textSize = this.ButtonFont.MeasureString(buttonText);
			float verticalOffset = (area.Height - textSize.Y) / 2;
			float horizontalOffset = (area.Width - textSize.X) / 2 + 1;
			return new Vector2(area.Position.X + horizontalOffset, area.Position.Y + verticalOffset);
		}
	}
}
