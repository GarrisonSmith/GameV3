using Engine.UI.Base.enums;
using System.Collections.Generic;

namespace Engine.UI.Base
{
	/// <summary>
	/// Represents a row number.
	/// </summary>
	public class UiStack : BaseUiElement
	{
		/// <summary>
		/// Gets or sets the UI element type. 
		/// </summary>
		public UiStackDirection UiStackDirection { get; set; }

		/// <summary>
		/// Gets or sets the stack elements
		/// </summary>
		public List<BaseUiElement> StackElements { get; set; }

		/// <summary>
		/// Initializes a new UiStack of the UiElement class.
		/// </summary>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="stackElements">The stack elements.</param>
		/// <param name="uiStackDirection">The UI stack direction.</param>
		public UiStack(bool drawingActivated, ushort drawOrder, List<BaseUiElement> stackElements, UiStackDirection uiStackDirection)
			: base(UiElementTypes.UiStack, drawingActivated, drawOrder)
		{
			this.UiStackDirection = uiStackDirection;
			this.StackElements = stackElements;
		}

		/// <summary>
		/// Adds the UI element.
		/// </summary>
		/// <param name="drawableUiElement">The drawable UI element.</param>
		/// <returns>A value indicating whether the UI element was added.</returns>
		public void AddUiElement(DrawableUiElement drawableUiElement)
		{
			this.StackElements.Add(drawableUiElement);

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
		/// Draws the UI element.
		/// </summary>
		public override void Draw()
		{

		}
	}
}
