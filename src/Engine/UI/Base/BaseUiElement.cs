using Engine.Core.Base.interfaces;
using Engine.UI.Base.enums;
using System;

namespace Engine.UI.Base
{
	/// <summary>
	/// Represents a UI element.
	/// </summary>
	public abstract class BaseUiElement : IDrawableContent
	{
		private bool drawingActivated;
		private bool beingHovered;
		private ushort drawOrder;

		/// <summary>
		/// Gets or sets the guid.
		/// </summary>
		public Guid Guid { get; set; }

		/// <summary>
		/// Gets or sets the UI type.
		/// </summary>
		public UiElementTypes UiElementType { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether updating is activated.
		/// </summary>
		public bool DrawingActivated
		{
			get => this.drawingActivated;

			set
			{
				if (this.drawingActivated != value)
				{
					this.drawingActivated = value;
					Managers.ContentManager.ChangeOverlayDrawActivation(this);
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the UI element is being hovered.
		/// </summary>
		public bool BeingHovered
		{
			get => this.beingHovered;

			set
			{
				if (value)
				{
					if (this.beingHovered)
					{
						this.OnHover();
					}
					else
					{
						this.OnHovering();
					}
				}

				this.beingHovered = value;
			}
		}

		/// <summary>
		/// Gets or sets the draw order.
		/// </summary>
		public ushort DrawOrder
		{
			get => this.drawOrder;

			set
			{
				if (this.drawOrder != value)
				{
					this.drawOrder = value;
					Managers.ContentManager.ChangeOverlayDrawOrder(drawOrder, this);
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the UiElement class.
		/// </summary>
		/// <param name="uiElementType">The UI element type.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		protected BaseUiElement(UiElementTypes uiElementType, bool drawingActivated, ushort drawOrder)
		{
			this.Guid = Guid.NewGuid();
			this.UiElementType = uiElementType;
			this.drawingActivated = drawingActivated;
			this.drawOrder = drawOrder;
			Managers.ContentManager.AddOverlayDrawable(this);
			Managers.UiManager.UiElements.Add(this.Guid, this);
		}

		/// <summary>
		/// When the UI element is first hovered.
		/// </summary>
		public virtual void OnHover()
		{

		}

		/// <summary>
		/// When the UI element is being hovered.
		/// </summary>
		public virtual void OnHovering()
		{

		}

		/// <summary>
		/// Draws the UI element.
		/// </summary>
		public virtual void Draw()
		{

		}

		public void Dispose()
		{
			this.DrawingActivated = false;
		}
	}
}
