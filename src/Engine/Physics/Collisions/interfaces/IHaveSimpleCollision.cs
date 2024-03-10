using Engine.Physics.Areas.interfaces;

namespace Engine.Physics.Collisions.interfaces
{
	/// <summary>
	/// Represents something that has simple collision.
	/// </summary>
	public interface IHaveSimpleCollision : IHaveCollision, IHaveSimpleArea
    {
		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		new public SimpleCollisionArea CollisionArea { get; set; }
	}
}
