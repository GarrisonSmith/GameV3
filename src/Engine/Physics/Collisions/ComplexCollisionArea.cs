using Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions.enums;
using Engine.Physics.Collisions.interfaces;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Physics.Collisions
{
	/// <summary>
	/// Represents a complex collision area.
	/// </summary>
	public class ComplexCollisionArea : IAmACollisionArea
	{
		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		public ComplexArea Area { get; private set; }

		/// <summary>
		/// Gets the collision area.
		/// </summary>
		IAmAArea IAmACollisionArea.Area { get => this.Area; }

		/// <summary>
		/// Gets or sets the movement terrain types.
		/// </summary>
		public List<List<MovementTerrainTypes>> MovementTerrainTypes { get; set; }

		/// <summary>
		/// Gets the movement terrain types.
		/// </summary>
		IEnumerable IAmACollisionArea.MovementTerrainTypes { get => this.MovementTerrainTypes; }

		/// <summary>
		/// Initializes a new instance of the ComplexCollisionArea class.
		/// </summary>
		/// <param name="area">The area.</param>
		/// <param name="movementTerrainTypes">The movement terrain types.</param>
		public ComplexCollisionArea(ComplexArea area, List<List<MovementTerrainTypes>> movementTerrainTypes)
		{
			this.Area = area;
			this.MovementTerrainTypes = movementTerrainTypes;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external collision source.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <param name="collisionInformation">The collision information.</param>
		/// <returns>The collision information.</returns>
		public CollisionInformation GetCollisionInformation(IHaveCollision external, Vector2 candidatePosition, CollisionInformation collisionInformation)
		{
			if (external.CollisionArea is SimpleCollisionArea simpleArea)
			{
				return this.GetCollisionInformation(simpleArea, candidatePosition, collisionInformation);
			}
			else if (external.CollisionArea is OffsetCollisionArea offsetArea)
			{
				return this.GetCollisionInformation(offsetArea, candidatePosition, collisionInformation);
			}
			else if (external.CollisionArea is ComplexCollisionArea complexArea)
			{
				return this.GetCollisionInformation(complexArea, candidatePosition, collisionInformation);
			}

			return null;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external collision source.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <param name="collisionInformation">The collision information.</param>
		/// <returns>The collision information.</returns>
		private CollisionInformation GetCollisionInformation(SimpleCollisionArea external, Vector2 candidatePosition, CollisionInformation collisionInformation)
		{
			int i = 0;
			foreach (var externalSubArea in this.Area.SubAreas)
			{
				if (externalSubArea.Intersects(external.Area, candidatePosition))
				{
					collisionInformation.AddMovementTerrainTypesIfDistinct(this.MovementTerrainTypes[i]);
					var sharedTerrainTypes = external.MovementTerrainTypes.Intersect(this.MovementTerrainTypes[i]);
					if (!sharedTerrainTypes.Any())
					{
						collisionInformation.UpdateCollisionBounds(this.Area, candidatePosition);
						collisionInformation.TerrainCollision = true;
					}
				}

				i++;
			}

			return collisionInformation;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external collision source.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <param name="collisionInformation">The collision information.</param>
		/// <returns>The collision information.</returns>
		private CollisionInformation GetCollisionInformation(OffsetCollisionArea external, Vector2 candidatePosition, CollisionInformation collisionInformation)
		{
			int i = 0;
			foreach (var externalSubArea in this.Area.SubAreas)
			{
				if (externalSubArea.Intersects(external.Area, candidatePosition))
				{
					collisionInformation.AddMovementTerrainTypesIfDistinct(this.MovementTerrainTypes[i]);
					var sharedTerrainTypes = external.MovementTerrainTypes.Intersect(this.MovementTerrainTypes[i]);
					if (!sharedTerrainTypes.Any())
					{
						collisionInformation.UpdateCollisionBounds(this.Area, candidatePosition);
						collisionInformation.TerrainCollision = true;
					}
				}

				i++;
			}

			return collisionInformation;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external complex collision source.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <param name="collisionInformation">The collision information.</param>
		/// <returns>The collision information.</returns>
		private CollisionInformation GetCollisionInformation(ComplexCollisionArea external, Vector2 candidatePosition, CollisionInformation collisionInformation)
		{
			int i = 0;
			foreach (var externalSubArea in this.Area.SubAreas)
			{
				int j = 0;
				foreach (var internalSubArea in this.Area.SubAreas)
				{
					if (externalSubArea.Intersects(external.Area.SubAreas[j], candidatePosition))
					{
						collisionInformation.AddMovementTerrainTypesIfDistinct(this.MovementTerrainTypes[i]);
						var sharedTerrainTypes = external.MovementTerrainTypes[j].Intersect(this.MovementTerrainTypes[i]);
						if (!sharedTerrainTypes.Any())
						{
							collisionInformation.UpdateCollisionBounds(this.Area, candidatePosition);
							collisionInformation.TerrainCollision = true;
						}
					}

					j++;
				}

				i++;
			}

			return collisionInformation;
		}
	}
}
