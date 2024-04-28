using Engine.Physics.Collisions.interfaces;

namespace Engine.TileMapping.Base.interfaces
{
	public interface IAmATile : IHaveCollision
	{
		/// <summary>
		/// Deactivates the tile.
		/// </summary>
		/// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
		/// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
		void DeactivateTile(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true);
	}
}
