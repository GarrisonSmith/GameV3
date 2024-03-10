using Engine.Drawing.Base;
using Engine.Physics.Base.interfaces;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base;

namespace Engine.Entities.Base.interfaces
{
	/// <summary>
	/// Represents a entity.
	/// </summary>
	public interface IAmAEntity : IHaveCollision, ICanMove, IHaveOrientation
	{
		/// <summary>
		/// Gets the layer.
		/// </summary>
		ushort Layer { get; }

		/// <summary>
		/// Gets or sets the tile map layer.
		/// </summary>
		TileMapLayer TileMapLayer { get; set; }

		/// <summary>
		/// Gets or sets the animations.
		/// The 0th index is the downward animation, the 1st index is the right animation,
		/// the 2nd index is the left animation, the 3rd index is the upward animation.
		/// </summary>
		Animation[] Animations { get; set; }
	}
}
