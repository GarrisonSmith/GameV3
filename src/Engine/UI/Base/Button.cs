using Engine.Physics.Areas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI.Base
{
	public class Button : UiElement
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
		/// Initializes a new instance of the Button class.
		/// </summary>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="area">The area.</param>
		/// <param name="buttonBackgroundColor">The button background color.</param>
		/// <param name="buttonBorderColor">The button border color.</param>
		/// <param name="buttonBorderSize">The button border size.</param>
		/// <param name="buttonText">The button text.</param>
		/// <param name="buttonFont">The button font.</param>
		public Button(bool drawingActivated, ushort drawOrder, SimpleArea area, Color buttonBackgroundColor, Color buttonBorderColor, int buttonBorderSize, string buttonText, Color buttonTextColor, SpriteFont buttonFont)
			: base(UiElementTypes.Button, drawingActivated, drawOrder, area, 
				  new OffsetArea(area.Position, buttonBorderSize, buttonBorderSize, area.Width - buttonBorderSize * 2, area.Height - buttonBorderSize * 2))
		{
			this.ButtonText = buttonText;
			this.ButtonTextColor = buttonTextColor;
			this.ButtonFont = buttonFont;
			this.UiTexture = this.GetButtonTexture((int)area.Width, (int)area.Height, buttonBackgroundColor, buttonBorderColor, buttonBorderSize);
			this.ButtonTextCoordinates = this.GetButtonTextCoordinates(buttonText, area);
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
			this.UiTexture = this.GetButtonTexture((int)this.Area.Width, (int)this.Area.Height, Color.Orange, Color.DarkOrchid, 6);
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
		/// Gets the button texture.
		/// </summary>
		/// <param name="buttonTextureWidth">The button texture width.</param>
		/// <param name="buttonTextureHeight">The button texture height.</param>
		/// <param name="buttonBackgroundColor">The button background color.</param>
		/// <param name="buttonBorderColor">The button border color.</param>
		/// <param name="buttonBorderSize">The button border size.</param>
		/// <returns>The button texture.</returns>
		private Texture2D GetButtonTexture(int buttonTextureWidth, int buttonTextureHeight, Color buttonBackgroundColor, Color? buttonBorderColor = null, int? buttonBorderSize = null)
		{
			Color[] colorData = new Color[buttonTextureWidth * buttonTextureHeight];
			for (var i = 0; i < buttonTextureHeight; i++)
			{
				for (var j = 0; j < buttonTextureWidth; j++)
				{
					var arrayIndex = i * buttonTextureWidth + j;
					if (buttonBorderColor.HasValue && buttonBorderSize.HasValue
						&& (i <= buttonBorderSize || i >= buttonTextureHeight - buttonBorderSize || j <= buttonBorderSize || j >= buttonTextureWidth - buttonBorderSize))
					{
						colorData[arrayIndex] = buttonBorderColor.Value;
					}
					else
					{
						colorData[arrayIndex] = buttonBackgroundColor;
					}
				}
			}

			Texture2D buttonTexture = new Texture2D(Managers.Graphics.GraphicsDevice, buttonTextureWidth, buttonTextureHeight);
			buttonTexture.SetData(colorData);
			return buttonTexture;
		}

		/// <summary>
		/// Gets the button text coordinates.
		/// </summary>
		/// <param name="buttonText">The button text.</param>
		/// <param name="area">The button area.</param>
		/// <returns>The coordinates of the button text.</returns>
		private Vector2 GetButtonTextCoordinates(string buttonText, SimpleArea area)
		{
			var textSize = this.ButtonFont.MeasureString(buttonText);
			float verticalOffset = (area.Height - textSize.Y) / 2;
			float horizontalOffset = (area.Width - textSize.X) / 2 + 1;
			return new Vector2(area.Position.X + horizontalOffset, area.Position.Y + verticalOffset);
		}
	}
}
