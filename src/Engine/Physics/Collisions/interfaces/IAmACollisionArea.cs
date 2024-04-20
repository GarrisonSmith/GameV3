using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions.enums;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;

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
		/// Get a value indicating if the external collision area intersects this collision area. 
		/// </summary>
		/// <param name="external">The external collision area.</param>
		/// <param name="intersectedMovementTerrainTypes">The intersected movement terrain types.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating if the external collision area intersects this collision area.</returns>
		bool Intersects(IAmACollisionArea external, out IList<MovementTerrainTypes> intersectedMovementTerrainTypes, Vector2? candidatePosition = null);
	}
}
