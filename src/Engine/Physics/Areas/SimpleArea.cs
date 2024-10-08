﻿using DiscModels.Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Saving.Base.interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Areas
{
	/// <summary>
	/// Represents a simple area.
	/// </summary>
	public class SimpleArea : IAmADefinedArea, ICanBeSaved<SimpleAreaModel>
	{
		/// <summary>
		/// Gets the collision epsilon.
		/// </summary>
		public static float COLLISION_EPSILON { get => .01f; }

		/// <summary>
		/// Gets double the collision epsilon.
		/// </summary>
		public static float DOUBLE_COLLISION_EPSILON { get => .02f; }

		/// <summary>
		/// Get or sets the top left X value of the area.
		/// </summary>
		public float X { get => this.Position.X; }

		/// <summary>
		/// Gets or sets the top left Y value of the area.
		/// </summary>
		public float Y { get => this.Position.Y; }

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
		public Vector2 TopLeft { get => this.Position.Coordinates; }

		/// <summary>
		/// Gets or sets the center position of the area.
		/// </summary>
		public Vector2 Center { get => new(this.TopLeft.X + this.Width / 2, this.TopLeft.Y + this.Height / 2); }

		/// <summary>
		/// Gets or sets the bottom right position of the area.
		/// </summary>
		public Vector2 BottomRight { get => new(this.TopLeft.X + this.Width, this.TopLeft.Y + this.Height); }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; private set; }

		/// <summary>
		/// Initializes a new instance of the Simple Area class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="simpleAreaModel">The simple area model.</param>
		public SimpleArea(Position position, SimpleAreaModel simpleAreaModel)
		{
			this.Position = position;
			this.Width = simpleAreaModel.Width;
			this.Height = simpleAreaModel.Height;
		}

		/// <summary>
		/// Initializes a new instance of the Simple Area class.
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
		/// Initializes a new instance of the Simple Area class.
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
			if (external is SimpleArea simpleArea)
			{
				return this.GetIntersects(simpleArea, candidatePosition);
			}
			else if (external is OffsetArea offsetArea)
			{
				return this.GetIntersects(offsetArea, candidatePosition);
			}
			else if (external is AreaCollection areaCollection)
			{
				return this.GetIntersects(areaCollection, candidatePosition);
			}

			return false;
		}

		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		public SimpleAreaModel ToModel()
		{
			return new SimpleAreaModel
			{
				Width = this.Width,
				Height = this.Height
			};
		}

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		private bool GetIntersects(SimpleArea external, Vector2? candidatePosition = null)
		{
			candidatePosition ??= external.TopLeft;
			var externalBottomRight = new Vector2(candidatePosition.Value.X + external.Width, candidatePosition.Value.Y + external.Height);
			var thisBottomRight = this.BottomRight;

			return !(this.X >= externalBottomRight.X + SimpleArea.COLLISION_EPSILON ||
					 thisBottomRight.X <= candidatePosition.Value.X - SimpleArea.COLLISION_EPSILON ||
					 this.Y >= externalBottomRight.Y + SimpleArea.COLLISION_EPSILON ||
					 thisBottomRight.Y <= candidatePosition.Value.Y - SimpleArea.COLLISION_EPSILON);
		}

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		private bool GetIntersects(OffsetArea external, Vector2? candidatePosition = null)
		{
			if (candidatePosition.HasValue)
			{
				candidatePosition = new Vector2(candidatePosition.Value.X + external.HorizontalOffset, candidatePosition.Value.Y + external.VerticalOffset);
			}

			candidatePosition ??= external.TopLeft;
			var externalBottomRight = new Vector2(candidatePosition.Value.X + external.Width, candidatePosition.Value.Y + external.Height);
			var thisBottomRight = this.BottomRight;

			return !(this.X >= externalBottomRight.X + SimpleArea.COLLISION_EPSILON ||
					 thisBottomRight.X <= candidatePosition.Value.X - SimpleArea.COLLISION_EPSILON ||
					 this.Y >= externalBottomRight.Y + SimpleArea.COLLISION_EPSILON ||
					 thisBottomRight.Y <= candidatePosition.Value.Y - SimpleArea.COLLISION_EPSILON);
		}

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		private bool GetIntersects(AreaCollection external, Vector2? candidatePosition = null)
		{
			return external.Intersects(this, candidatePosition);
		}
	}
}