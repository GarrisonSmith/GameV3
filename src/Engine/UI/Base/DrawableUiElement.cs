using Engine.Drawing.Base;
using Engine.Drawing.Base.interfaces;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.UI.Base.enums;

namespace Engine.UI.Base
{
	/// <summary>
	/// Represents a drawable UI element.
	/// </summary>
	public abstract class DrawableUiElement : BaseUiElement, ICanBeDrawn
	{
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
