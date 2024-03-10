namespace Engine.Core.Base.interfaces
{
	/// <summary>
	/// Represents drawable content.
	/// </summary>
	public interface IDrawableContent : IContent
    {
        /// <summary>
        /// Gets or sets a value indicating whether drawing is activated.
        /// </summary>
        bool DrawingActivated { get; set; }

        /// <summary>
        /// Gets or sets the draw order.
        /// </summary>
        ushort DrawOrder { get; set; }

        /// <summary>
        /// Draws the content.
        /// </summary>
        void Draw();
    }
}
