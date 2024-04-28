using Microsoft.Xna.Framework;

namespace Engine.Physics.Base.interfaces
{
	/// <summary>
	/// Represents something with a position.
	/// </summary>
	public interface IHavePosition
    {
		/// <summary>
		/// Get or sets the top left X value of the position.
		/// </summary>
		float X { get; set; }

		/// <summary>
		/// Gets or sets the top left Y value of the position.
		/// </summary>
		float Y { get; set; }

		/// <summary>
		/// Gets or sets the top right position of the position.
		/// </summary>
		Vector2 TopLeft { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		Position Position { get; set; }
    }
}
