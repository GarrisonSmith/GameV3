using Engine.Physics.Areas.interfaces;

namespace Engine.Physics.Collisions.interfaces
{
	/// <summary>
	/// Represents something that has collision.
	/// </summary>
	public interface IHaveCollision : IHaveArea
    {
        /// <summary>
        /// Gets the collision area.
        /// </summary>
        IAmACollisionArea CollisionArea { get; }
    }
}
