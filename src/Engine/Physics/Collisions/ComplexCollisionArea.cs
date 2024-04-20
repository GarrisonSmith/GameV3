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
		/// Get a value indicating if the external collision area intersects this collision area. 
		/// </summary>
		/// <param name="external">The external collision area.</param>
		/// <param name="intersectedMovementTerrainTypes">The intersected movement terrain types.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating if the external collision area intersects this collision area.</returns>
		public bool Intersects(IAmACollisionArea external, out IList<MovementTerrainTypes> intersectedMovementTerrainTypes, Vector2? candidatePosition = null)
		{
			if (external is SimpleCollisionArea simpleArea)
			{
				return this.GetCollisionInformation(simpleArea, out intersectedMovementTerrainTypes, candidatePosition);
			}
			else if (external is OffsetCollisionArea offsetArea)
			{
				return this.GetCollisionInformation(offsetArea, out intersectedMovementTerrainTypes, candidatePosition);
			}
			else if (external is ComplexCollisionArea complexArea)
			{
				return this.GetCollisionInformation(complexArea, out intersectedMovementTerrainTypes, candidatePosition);
			}

			intersectedMovementTerrainTypes = null;

			return false;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external collision source.</param>
		/// <param name="intersectedMovementTerrainTypes">The intersected movement terrain types.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>The collision information.</returns>
		private bool GetCollisionInformation(SimpleCollisionArea external, out IList<MovementTerrainTypes> intersectedMovementTerrainTypes, Vector2? candidatePosition = null)
		{
			int i = 0;
			foreach (var subArea in this.Area.OffsetAreas)
			{
				if (subArea.Intersects(external.Area, candidatePosition))
				{
					intersectedMovementTerrainTypes = this.MovementTerrainTypes[i].Where(x => !external.MovementTerrainTypes.Contains(x)).ToList();
					if (intersectedMovementTerrainTypes.Any())
					{
						return true;
					}
				}

				i++;
			}

			intersectedMovementTerrainTypes = null;

			return false;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external collision source.</param>
		/// <param name="intersectedMovementTerrainTypes">The intersected movement terrain types.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>The collision information.</returns>
		private bool GetCollisionInformation(OffsetCollisionArea external, out IList<MovementTerrainTypes> intersectedMovementTerrainTypes, Vector2? candidatePosition = null)
		{
			int i = 0;
			foreach (var subArea in this.Area.OffsetAreas)
			{
				if (subArea.Intersects(external.Area, candidatePosition))
				{
					intersectedMovementTerrainTypes = this.MovementTerrainTypes[i].Where(x => !external.MovementTerrainTypes.Contains(x)).ToList();
					if (intersectedMovementTerrainTypes.Any())
					{
						return true;
					}
				}

				i++;
			}

			intersectedMovementTerrainTypes = null;

			return false;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external complex collision source.</param>
		/// <param name="intersectedMovementTerrainTypes">The intersected movement terrain types.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>The collision information.</returns>
		private bool GetCollisionInformation(ComplexCollisionArea external, out IList<MovementTerrainTypes> intersectedMovementTerrainTypes, Vector2? candidatePosition = null)
		{
			int i = 0;
			foreach (var externalSubArea in external.Area.OffsetAreas)
			{
				int j = 0;
				foreach (var subArea in this.Area.OffsetAreas)
				{
					if (subArea.Intersects(external.Area, candidatePosition))
					{
						intersectedMovementTerrainTypes = this.MovementTerrainTypes[j].Where(x => !external.MovementTerrainTypes[i].Contains(x)).ToList();
						if (intersectedMovementTerrainTypes.Any())
						{
							return true;
						}
					}

					j++;
				}

				i++;
			}

			intersectedMovementTerrainTypes = null;

			return false;
		}
	}
}
