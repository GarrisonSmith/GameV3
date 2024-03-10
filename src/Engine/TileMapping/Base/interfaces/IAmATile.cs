using Engine.Physics.Collisions.interfaces;

namespace Engine.TileMapping.Base.interfaces
{
	public interface IAmATile : IHaveCollision
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
		/// Gets the tile map.
		/// </summary>
		TileMap TileMap { get; }

		/// <summary>
		/// Deactivates the tile.
		/// </summary>
		/// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
		/// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
		void DeactivateTile(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true);
	}
}
