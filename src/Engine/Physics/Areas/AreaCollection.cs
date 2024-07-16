using DiscModels.Engine.Physics.Areas;
using DiscModels.Engine.Physics.Areas.interfaces;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Saving.Base.interfaces;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Engine.Physics.Areas
{
	/// <summary>
	/// Represents a collection of areas.
	/// </summary>
	public class AreaCollection : IAmAArea, ICanBeSaved<AreaCollectionModel>
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; private set; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get;  private set; }

		/// <summary>
		/// Gets top right of the area.
		/// </summary>
		public Vector2 TopLeft { get => this.Position.Coordinates; }

		/// <summary>
		/// Gets the center of the area.
		/// </summary>
		public Vector2 Center { get => new (this.TopLeft.X + this.Width / 2, this.TopLeft.Y + this.Height / 2); }

		/// <summary>
		/// Gets the bottom right of the area.
		/// </summary>
		public Vector2 BottomRight { get => new (this.TopLeft.X + this.Width, this.TopLeft.Y + this.Height); }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; private set; }

		/// <summary>
		/// Gets or sets the areas.
		/// </summary>
		public IAmADefinedArea[] Areas { get; private set; }

		/// <summary>
		/// Initializes a new instance of the AreaCollection class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="areaCollectionModel">The area collection model.</param>
		public AreaCollection(Position position, AreaCollectionModel areaCollectionModel)
		{
			this.Position = position;
			this.Areas = areaCollectionModel.Areas.Select(x => Managers.PhysicsManager.GetArea(position, x) as IAmADefinedArea).ToArray();
			this.CalculateDimensions();
		}

		/// <summary>
		/// Initializes a new instance of the AreaCollection class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="areas">The areas.</param>
		public AreaCollection(Position position, IAmADefinedArea[] areas)
		{ 
			this.Position = position;
			this.Areas = areas;
			this.CalculateDimensions();
		}

		/// <summary>
		/// Calculates the dimensions of the area collection.
		/// </summary>
		public void CalculateDimensions()
		{
			this.Width = 0;
			this.Height = 0;

			foreach (var area in this.Areas)
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
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Vector2 point)
		{
			return this.Areas.Any(x => x.Contains(point));
		}

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Point point)
		{
			return this.Areas.Any(x => x.Contains(point));
		}

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		public bool Intersects(IAmAArea external, Vector2? candidatePosition = null)
		{
			return this.Areas.Any(x => x.Intersects(external, candidatePosition));
		}

		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		public AreaCollectionModel ToModel()
		{
			IAmAAreaModel[] areaModels = new IAmAAreaModel[this.Areas.Length];
			ushort index = 0;
			foreach (var area in this.Areas)
			{
				if (area is SimpleArea simpleArea)
				{
					areaModels[index++] = simpleArea.ToModel();
				}
				else if (area is OffsetArea offsetArea)
				{
					areaModels[index++] = offsetArea.ToModel();
				}
			}

			return new AreaCollectionModel
			{
				Width = this.Width,
				Height = this.Height,
				Areas = areaModels
			};
		}
	}
}
