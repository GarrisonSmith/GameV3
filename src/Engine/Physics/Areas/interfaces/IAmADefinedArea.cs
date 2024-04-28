using Microsoft.Xna.Framework;

namespace Engine.Physics.Areas.interfaces
{   
	/// <summary>
	/// Represents a defined area.
	/// </summary>
	public interface IAmADefinedArea : IAmAArea
	{
		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		new float Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		new float Height { get; set; }

		/// <summary>
		/// Gets the center position of the area.
		/// </summary>
		new Vector2 Center { get; set; }
	}
}
