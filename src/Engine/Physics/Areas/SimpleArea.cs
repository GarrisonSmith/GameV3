using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Areas
{
	/// <summary>
	/// Represents a area.
	/// </summary>
	public class SimpleArea : IAmAArea
	{
		/// <summary>
		/// Gets the collision epsilon.
		/// </summary>
		public static float COLLISION_EPSILON { get => .01f; }

		/// <summary>
		/// Gets double the collision epsilon.
		/// </summary>
		public static float COLLISION_EPSILON_DOUBLE { get => .02f; }

		/// <summary>
		/// Get or sets the top left X value of the area.
		/// </summary>
		public float X { get => this.Position.X; set => this.Position.X = value; }

		/// <summary>
		/// Gets or sets the top left Y value of the area.
		/// </summary>
		public float Y { get => this.Position.Y; set => this.Position.Y = value; }

		/// <summary>
		/// Gets or sets the width of the area.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Gets or sets the height of the area.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Gets or sets the top right position of the area.
		/// </summary>
		public Vector2 TopLeft { get => this.Position.Coordinates; set => this.Position.Coordinates = value; }

		/// <summary>
		/// Gets or sets the bottom right position of the area.
		/// </summary>
		public Vector2 BottomRight { get => new Vector2(this.TopLeft.X + this.Width, this.TopLeft.Y + this.Height); }

		/// <summary>
		/// Gets or sets the center position of the area.
		/// </summary>
		public Vector2 Center
		{
			get => new(this.TopLeft.X + this.Width / 2, this.TopLeft.Y + this.Height / 2);
			set
			{
				this.X = value.X - this.Width / 2;
				this.Y = value.Y - this.Height / 2;
			}
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Initializes a new instance of the Area class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="width">The width value.</param>
		/// <param name="height">The height value.</param>
		public SimpleArea(Position position, float width, float height)
		{
			this.Position = position;
			this.Width = width > 0 ? width : 0;
			this.Height = height > 0 ? height : 0;
		}

		/// <summary>
		/// Initializes a new instance of the Area class.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		public SimpleArea(Rectangle rectangle)
		{
			this.Position = new Position(rectangle.X, rectangle.Y);
			this.Width = rectangle.Width > 0 ? rectangle.Width : 0;
			this.Height = rectangle.Height > 0 ? rectangle.Height : 0;
		}

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Vector2 point)
		{
			return this.X <= point.X && point.X <= this.X + this.Width
				&& this.Y <= point.Y && point.Y <= this.Y + this.Height;
		}

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Point point)
		{
			return this.X <= point.X && point.X <= this.X + this.Width
				&& this.Y <= point.Y && point.Y <= this.Y + this.Height;
		}

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		public bool Intersects(IAmAArea external, Vector2? candidatePosition = null)
		{
			candidatePosition ??= external.TopLeft;
			var externalBottomRight = new Vector2(candidatePosition.Value.X + external.Width, candidatePosition.Value.Y + external.Height);
			var thisBottomRight = this.BottomRight;

			return !(this.X >= externalBottomRight.X + SimpleArea.COLLISION_EPSILON ||
					 thisBottomRight.X <= candidatePosition.Value.X - SimpleArea.COLLISION_EPSILON ||
					 this.Y >= externalBottomRight.Y + SimpleArea.COLLISION_EPSILON ||
					 thisBottomRight.Y <= candidatePosition.Value.Y - SimpleArea.COLLISION_EPSILON);
		}
	}
}