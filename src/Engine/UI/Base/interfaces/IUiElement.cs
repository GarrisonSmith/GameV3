using Engine.Core.Base.interfaces;

namespace Engine.UI.Base.interfaces
{
	public interface IUiElement : IDrawableContent
	{
		/// <summary>
		/// Gets or sets a value indicating whether the UI element is being hovered.
		/// </summary>
		bool BeingHovered { get; set; }

		/// <summary>
		/// When the UI element is first hovered.
		/// </summary>
		void OnHover();

		/// <summary>
		/// When the UI element is being hovered.
		/// </summary>
		void OnHovering();
	}
}
