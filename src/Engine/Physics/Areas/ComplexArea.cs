using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Physics.Areas
{
	/// <summary>
	/// Represents a complex area.
	/// </summary>
	public class ComplexArea : IAmAArea
	{
		private List<OffsetArea> subAreas;

		/// <summary>
		/// Get or sets the top left X value of the area.
		/// </summary>
		public float X { get => this.Position.X; }

		/// <summary>
		/// Gets or sets the top left Y value of the area.
		/// </summary>
		public float Y { get => this.Position.Y; }

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Gets or sets the top right position of the area.
		/// </summary>
		public Vector2 TopLeft { get => this.Position.Coordinates; set => this.Position.Coordinates = value; }

		/// <summary>
		/// Gets the center position of the area.
		/// </summary>
		public Vector2 Center { get => new(this.X + (this.Width / 2), this.Y + (this.Height / 2)); }

		/// <summary>
		/// Gets the bottom right position of the area.
		/// </summary>
		public Vector2 BottomRight { get => new(this.X + this.Width, this.Y + this.Height); }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the sub areas.
		/// </summary>
		public List<OffsetArea> SubAreas { get => subAreas; private set => this.subAreas = value; }

		/// <summary>
		/// Initializes a new instance of the ComplexArea class.
		/// </summary>
		/// <param name="position">The position.</param>
		public ComplexArea(Position position, List<OffsetArea> subAreas)
		{ 
			this.Position = position;
			this.SubAreas = subAreas;
			this.Width = 0;
			this.Height = 0;
			foreach (var complexSubArea in this.SubAreas)
			{ 
				if (complexSubArea.HorizontalOffset > this.Width)
				{
					this.Width = complexSubArea.HorizontalOffset;
				}

				if (complexSubArea.VerticalOffset > this.Height)
				{
					this.Height = complexSubArea.VerticalOffset;
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
			foreach (var complexSubArea in this.SubAreas) 
			{
				if (complexSubArea.Contains(point))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Point point)
		{
			foreach (var complexSubArea in this.SubAreas)
			{
				if (complexSubArea.Contains(point))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		public bool Intersects(IAmAArea external, Vector2? candidatePosition)
		{
			foreach (var complexSubArea in this.SubAreas)
			{
				if (complexSubArea.Intersects(external, candidatePosition))
				{
					return true;
				}
			}

			return false;
		}
	}
}
