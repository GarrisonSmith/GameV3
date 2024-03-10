using Microsoft.Xna.Framework;

namespace Engine.Physics.Base
{
	/// <summary>
	/// Represents a position.
	/// </summary>
	public class Position
	{
		private Vector2 coordinates;

		/// <summary>
		/// Gets or sets the coordinates.
		/// </summary>
		public Vector2 Coordinates { get => coordinates; set => coordinates = value; }

		/// <summary>
		/// Gets or sets the X coordinate.
		/// </summary>
		public float X { get => coordinates.X; set => coordinates.X = value; }

		/// <summary>
		/// Gets or sets the Y coordinate.
		/// </summary>
		public float Y { get => coordinates.Y; set => coordinates.Y = value; }

		/// <summary>
		/// Initializes a new instance of the Position class.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		public Position(Vector2 coordinate)
		{ 
			this.coordinates = coordinate;
		}

		/// <summary>
		/// Initializes a new instance of the Position class.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public Position(float x, float y)
		{
			this.coordinates = new Vector2(x, y);
		}
	}
}
