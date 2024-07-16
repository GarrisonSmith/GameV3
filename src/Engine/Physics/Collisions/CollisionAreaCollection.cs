using DiscModels.Engine.Physics.Collisions;
using DiscModels.Engine.Physics.Collisions.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions.enums;
using Engine.Physics.Collisions.interfaces;
using Engine.Saving.Base.interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Physics.Collisions
{
	/// <summary>
	/// Represents a complex collision area.
	/// </summary>
	public class CollisionAreaCollection : IAmACollisionArea, ICanBeSaved<CollisionAreaCollectionModel>
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; private set; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get; private set; }

		/// <summary>
		/// Gets the top left of the collision area.
		/// </summary>
		public Vector2 TopLeft { get => this.Position.Coordinates; }

		/// <summary>
		/// Gets the center of the collision area.
		/// </summary>
		public Vector2 Center => throw new System.NotImplementedException();

		/// <summary>
		/// Gets the bottom right of the collision area.
		/// </summary>
		public Vector2 BottomRight => throw new System.NotImplementedException();

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; private set; }

		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		public IAmADefinedCollisionArea[] CollisionAreas { get; private set; }

		/// <summary>
		/// Initializes a new instance of the ComplexCollisionArea class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="collisionAreaModel">The collision area model.</param>
		public CollisionAreaCollection(Position position, CollisionAreaCollectionModel collisionAreaModel)
		{
			this.Position = position;
			this.CollisionAreas = collisionAreaModel.CollisionAreas.Select(x => Managers.PhysicsManager.GetCollisionArea(position, x) as IAmADefinedCollisionArea).ToArray();
			this.CalculateDimensions();
		}

		/// <summary>
		/// Initializes a new instance of the ComplexCollisionArea class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="collisionAreas">The collision areas.</param>
		public CollisionAreaCollection(Position position, IAmADefinedCollisionArea[] collisionAreas)
		{
			this.Position = position;
			this.CollisionAreas = collisionAreas;
			this.CalculateDimensions();
		}

		/// <summary>
		/// Calculates the dimensions of the area collection.
		/// </summary>
		public void CalculateDimensions()
		{
			this.Width = 0;
			this.Height = 0;
			foreach (var area in this.CollisionAreas)
			{
				if (area.Width > this.Width)
				{
					this.Width = area.Width;
				}

				if (area.Height > this.Height)
				{
					this.Height = area.Height;
				}
			}
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
			var collision = false;
			intersectedMovementTerrainTypes = new List<MovementTerrainTypes>();
			foreach (var collisionArea in this.CollisionAreas)
			{
				if (collisionArea.Intersects(external, out var subIntersectedMovementTerrainTypes, candidatePosition))
				{
					collision = true;
                    foreach (var terrainType in subIntersectedMovementTerrainTypes)
                    {
						if (!intersectedMovementTerrainTypes.Contains(terrainType))
						{
							intersectedMovementTerrainTypes.Add(terrainType);
						}
                    }
                }
			}

			return collision;
		}

		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		public CollisionAreaCollectionModel ToModel()
		{
			IAmACollisionAreaModel[] areaModels = new IAmACollisionAreaModel[this.CollisionAreas.Length];
			ushort index = 0;
			foreach (var area in this.CollisionAreas)
			{
				if (area is SimpleCollisionArea simpleCollisionArea)
				{
					areaModels[index++] = simpleCollisionArea.ToModel();
				}
				else if (area is OffsetCollisionArea offsetCollisionArea)
				{
					areaModels[index++] = offsetCollisionArea.ToModel();
				}
			}

			return new CollisionAreaCollectionModel
			{
				CollisionAreas = areaModels,
			};
		}
	}
}
