using Engine.Physics.Base.interfaces;
using Engine.Physics.Collisions.enums;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Physics.Collisions.interfaces
{
	/// <summary>
	/// Represents a collision area.
	/// </summary>
	public interface IAmACollisionArea : IHavePosition
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		float Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		float Height { get; }

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
