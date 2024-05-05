using Engine.Physics.Areas.interfaces;
using System;

namespace Engine.Drawing.Base.interfaces
{
    /// <summary>
    /// Represents something that can be drawn.
    /// </summary>
	public interface ICanBeDrawn : IHaveArea, IDisposable
	{
        /// <summary>
        /// Gets or sets the draw data.
        /// </summary>
        DrawData DrawData { get; set; }
    }
}
