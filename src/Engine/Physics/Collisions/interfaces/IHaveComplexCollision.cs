namespace Engine.Physics.Collisions.interfaces
{
	/// <summary>
	/// Represents something that has complex collision.
	/// </summary>
	public interface IHaveComplexCollision : IHaveSimpleCollision
	{
		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		new public ComplexCollisionArea CollisionArea { get; set; }
	}
}