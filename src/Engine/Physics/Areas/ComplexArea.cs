using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Physics.Areas
{
	/// <summary>
	/// Represents a complex area.
	/// </summary>
	public class ComplexArea : IAmAArea
	{
		private float StoredHeight;
		private float StoredWidth;

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
		public float Width 
		{
			get
			{
				if (this.SubAreasChanged)
				{
					this.UpdateDimensions();
				}

				return this.StoredWidth;
			}
		}

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height 
		{ 
			get 
			{
				if (this.SubAreasChanged)
				{
					this.UpdateDimensions();
				}

				return this.StoredHeight;
			}
		}

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
		public IReadOnlyList<OffsetArea> OffsetAreas { get => SubAreas; }

		/// <summary>
		/// Gets or sets a value indicated if the sub areas has changed.
		/// </summary>
		private bool SubAreasChanged { get; set; }

		/// <summary>
		/// Gets or sets the sub areas.
		/// </summary>
		private List<OffsetArea> SubAreas { get; set; }

		/// <summary>
		/// Initializes a new instance of the ComplexArea class.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="offsetAreas">The offset areas.</param>
		public ComplexArea(Position position, List<OffsetArea> offsetAreas)
		{ 
			this.Position = position;
			this.SubAreas = offsetAreas;
			this.SubAreasChanged = true;
		}

		/// <summary>
		/// Adds the offset area if its not already in this complex area.
		/// </summary>
		/// <param name="offsetArea">The offset area.</param>
		public void AddOffsetArea(OffsetArea offsetArea)
		{
			if (!this.SubAreas.Contains(offsetArea))
			{
				this.SubAreas.Add(offsetArea);
				this.SubAreasChanged = true;
			}
		}

		/// <summary>
		/// Adds the removes area in this complex area.
		/// </summary>
		/// <param name="offsetArea">The offset area.</param>
		public void RemoveOffsetArea(OffsetArea offsetArea)
		{
			if (!this.SubAreas.Contains(offsetArea))
			{
				this.SubAreas.Remove(offsetArea);
				this.SubAreasChanged = true;
			}
		}

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Vector2 point)
		{
			return this.OffsetAreas.Any(x => x.Contains(point));
		}

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Point point)
		{
			return this.OffsetAreas.Any(x => x.Contains(point));
		}

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		public bool Intersects(IAmAArea external, Vector2? candidatePosition = null)
		{
			return this.OffsetAreas.Any(x => x.Intersects(external, candidatePosition));
		}

		/// <summary>
		/// Updates the complex areas dimensions.
		/// </summary>
		private void UpdateDimensions()
		{
			this.StoredWidth = 0f;
			this.StoredHeight = 0f;
			foreach (var complexSubArea in this.OffsetAreas)
			{
				if (complexSubArea.HorizontalOffset > this.StoredWidth)
				{
					this.StoredWidth = complexSubArea.VerticalOffset + complexSubArea.Width;
				}

				if (complexSubArea.VerticalOffset > this.StoredHeight)
				{
					this.StoredHeight = complexSubArea.VerticalOffset + complexSubArea.Height;
				}
			}

			this.SubAreasChanged = false;
		}
	}
}
