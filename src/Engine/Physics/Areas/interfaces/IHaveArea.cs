using Engine.Physics.Base.interfaces;

namespace Engine.Physics.Areas.interfaces
{
    /// <summary>
    /// Represents something with an area.
    /// </summary>
    public interface IHaveArea : IHavePosition
    {
        /// <summary>
        /// Gets the area.
        /// </summary>
        IAmAArea Area { get; }
    }
}
