using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Engine.Physics.Collisions.interfaces
{
	/// <summary>
	/// Represents a collision area.
	/// </summary>
	public interface IAmACollisionArea
	{
		/// <summary>
		/// Gets the collision area.
		/// </summary>
		IAmAArea Area { get; }

		/// <summary>
		/// Gets the movement terrain types.
		/// </summary>
		IEnumerable MovementTerrainTypes { get; }

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external collision source.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <param name="collisionInformation">The collision information.</param>
		/// <returns>The collision information.</returns>
		CollisionInformation GetCollisionInformation(IHaveCollision external, Vector2 candidatePosition, CollisionInformation collisionInformation);
	}
}
