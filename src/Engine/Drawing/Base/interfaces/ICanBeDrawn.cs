using Engine.Physics.Areas.interfaces;

namespace Engine.Drawing.Base.interfaces
{
    /// <summary>
    /// Represents something that can be drawn.
    /// </summary>
	public interface ICanBeDrawn : IHaveArea
    {
        /// <summary>
        /// Gets or sets the draw data.
        /// </summary>
        DrawData DrawData { get; set; }
    }
}
