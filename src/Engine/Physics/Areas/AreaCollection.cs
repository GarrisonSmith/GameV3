using System.Collections.Generic;
using System.Linq;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Base.interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Areas
{
	/// <summary>
	/// Represents a collection of areas.
	/// </summary>
    public class AreaCollection : IHavePosition
	{
		/// <summary>
		/// Get or sets the top left X value of the area.
		/// </summary>
		public float X { get => this.Position.X; set => this.Position.X = value; }

		/// <summary>
		/// Gets or sets the top left Y value of the area.
		/// </summary>
		public float Y { get => this.Position.Y; set => this.Position.Y = value; }

		/// <summary>
		/// Gets or sets the top right position of the area.
		/// </summary>
		public Vector2 TopLeft { get => this.Position.Coordinates; set => this.Position.Coordinates = value; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the areas.
		/// </summary>
		public List<IAmAArea> Areas;

		/// <summary>
		/// Initializes a new instance of the AreaCollection class.
		/// </summary>
		/// <param name="position">The position.</param>
		public AreaCollection(Position position)
		{ 
			this.Position = position;
			this.Areas = new();
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
	}
}
