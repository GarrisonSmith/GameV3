using Engine.Physics.Base.interfaces;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Areas.interfaces
{
	/// <summary>
	/// Represents an area.
	/// </summary>
	public interface IAmAArea : IHavePosition
	{
		/// <summary>
		/// Get or sets the top left X value of the area.
		/// </summary>
		public float X { get; }

		/// <summary>
		/// Gets or sets the top left Y value of the area.
		/// </summary>
		public float Y { get; }

		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width { get; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height { get; }

		/// <summary>
		/// Gets or sets the top right position of the area.
		/// </summary>
		public Vector2 TopLeft { get; set; }

		/// <summary>
		/// Gets the center position of the area.
		/// </summary>
		public Vector2 Center { get; }

		/// <summary>
		/// Gets the bottom right position of the area.
		/// </summary>
		public Vector2 BottomRight { get; }

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Vector2 point);

		/// <summary>
		/// Returns a value indicating whether the point is contained by this area.
		/// </summary>
		/// <param name="point">The point.</param>
		/// <returns>A value indicating whether the point is contained by this area.</returns>
		public bool Contains(Point point);

		/// <summary>
		/// Returns a value indicating whether the external area is intersecting by this area.
		/// </summary>
		/// <param name="external">The external area.</param>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>A value indicating whether the point is intersected by this area.</returns>
		public bool Intersects(IAmAArea external, Vector2? candidatePosition = null);
	}
}
