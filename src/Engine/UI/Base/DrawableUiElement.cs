using Engine.Drawing.Base;
using Engine.Drawing.Base.interfaces;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.UI.Base.enums;
using Microsoft.Xna.Framework;

namespace Engine.UI.Base
{
	/// <summary>
	/// Represents a drawable UI element.
	/// </summary>
	public abstract class DrawableUiElement : BaseUiElement, ICanBeDrawn
	{
		/// <summary>
		/// Get or sets the top left X value of the position.
		/// </summary>
		public float X { get => this.Position.X; set => this.Position.X = value; }

		/// <summary>
		/// Gets or sets the top left Y value of the position.
		/// </summary>
		public float Y { get => this.Position.Y; set => this.Position.Y = value; }

		/// <summary>
		/// Gets or sets the top right position of the position.
		/// </summary>
		public Vector2 TopLeft { get => this.Position.Coordinates; set => this.Position.Coordinates = value; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; set => this.Area.Position = value; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Gets or sets the draw data.
		/// </summary>
		public DrawData DrawData { get; set; }

		/// <summary>
		/// Initializes a new instance of the DrawableUiElement class.
		/// </summary>
		/// <param name="uiElementType">The UI element type.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="drawData">The draw data.</param>
		/// <param name="area">The area.</param>
		/// <param name="drawData">The draw data.</param>
		protected DrawableUiElement(UiElementTypes uiElementType, bool drawingActivated, ushort drawOrder, DrawData drawData, IAmAArea area)
			: base(uiElementType, drawingActivated, drawOrder)
		{
			this.Area = area;
			this.DrawData = drawData;
		}
	}
}
