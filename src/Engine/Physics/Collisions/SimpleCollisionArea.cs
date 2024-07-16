using DiscModels.Engine.Physics.Collisions;
using Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions.enums;
using Engine.Physics.Collisions.interfaces;
using Engine.Saving.Base.interfaces;
using Microsoft.Xna.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Physics.Collisions
{
	/// <summary>
	/// Represents a simple collision area.
	/// </summary>
	public class SimpleCollisionArea : IAmADefinedCollisionArea, ICanBeSaved<SimpleCollisionAreaModel>
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get => this.Area.Width; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get => this.Area.Height; }

		/// <summary>
		/// Gets the top left of the collision area.
		/// </summary>
		public Vector2 TopLeft { get => this.Area.TopLeft; }

		/// <summary>
		/// Gets the center of the collision area.
		/// </summary>
		public Vector2 Center { get => this.Area.Center; }

		/// <summary>
		/// Gets the bottom right of the collision area.
		/// </summary>
		public Vector2 BottomRight { get => this.Area.BottomRight; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		public SimpleArea Area { get; private set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		IAmAArea IHaveArea.Area { get => this.Area; }

		/// <summary>
		/// Gets the collision area.
		/// </summary>
		IAmADefinedArea IAmADefinedCollisionArea.Area { get => this.Area; }

		/// <summary>
		/// Gets or sets the movement terrain types.
		/// </summary>
		public List<MovementTerrainTypes> MovementTerrainTypes { get; set; }

		/// <summary>
		/// Gets the movement terrain types.
		/// </summary>
		IEnumerable IAmADefinedCollisionArea.MovementTerrainTypes { get => this.MovementTerrainTypes; }

		/// <summary>
		/// Initializes a new instance of the SimpleCollisionArea class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="simpleCollisionAreaModel">The simple collision area model.</param>
		public SimpleCollisionArea(Position position, SimpleCollisionAreaModel simpleCollisionAreaModel)
		{
			this.Area = new SimpleArea(position, simpleCollisionAreaModel.Area);
			this.MovementTerrainTypes = simpleCollisionAreaModel.MovementTerrainTypes.Select(x => (MovementTerrainTypes)x).ToList();
		}

		/// <summary>
		/// Initializes a new instance of the SimpleCollisionArea class.
		/// </summary>
		/// <param name="area">The area.</param>
		/// <param name="movementTerrainTypes">The movement terrain types.</param>
		public SimpleCollisionArea(SimpleArea area, List<MovementTerrainTypes> movementTerrainTypes)
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
			else if (external is CollisionAreaCollection complexArea)
			{
				return this.GetCollisionInformation(complexArea, out intersectedMovementTerrainTypes, candidatePosition);
			}

			intersectedMovementTerrainTypes = null;

			return false;
		}

		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		public SimpleCollisionAreaModel ToModel()
		{
			return new SimpleCollisionAreaModel
			{
				Area = this.Area.ToModel(),
				MovementTerrainTypes = this.MovementTerrainTypes.Select(x => (int)x).ToList()
			};
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
			if (this.Area.Intersects(external.Area, candidatePosition))
			{
				intersectedMovementTerrainTypes = this.MovementTerrainTypes.Where(x => !external.MovementTerrainTypes.Contains(x)).ToList();
				if (intersectedMovementTerrainTypes.Any())
				{
					return true;
				}
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
			if (this.Area.Intersects(external.Area, candidatePosition))
			{
				intersectedMovementTerrainTypes = this.MovementTerrainTypes.Where(x => !external.MovementTerrainTypes.Contains(x)).ToList();
				if (intersectedMovementTerrainTypes.Any())
				{
					return true;
				}
			}

			intersectedMovementTerrainTypes = null;

			return false;
		}

		/// <summary>
		/// Gets the collision information.
		/// </summary>
		/// <param name="external">The external collision collection source.</param>
		/// <param name="intersectedMovementTerrainTypes">The intersected movement terrain types.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>The collision information.</returns>
		private bool GetCollisionInformation(CollisionAreaCollection external, out IList<MovementTerrainTypes> intersectedMovementTerrainTypes, Vector2? candidatePosition = null)
		{
			return external.Intersects(this, out intersectedMovementTerrainTypes, candidatePosition);
		}
	}
}
