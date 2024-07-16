using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping.interfaces;
using Engine.Physics.Collisions.interfaces;
using Engine.Saving.Base.interfaces;
using System;

namespace Engine.TileMapping.Base.interfaces
{
	public interface IAmATile : IHaveCollision, IDisposable, ICanBeSaved<IAmATileModel<IAmAAreaModel, IAmACollisionAreaModel>>
	{
		/// <summary>
		/// Deactivates the tile.
		/// </summary>
		/// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
		/// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
		void DeactivateTile(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true);

		/// <summary>
		/// Gets a tile model that corresponds to this tile.
		/// </summary>
		/// <returns>The tile model.</returns>
		new IAmATileModel<IAmAAreaModel, IAmACollisionAreaModel> ToModel();
	}
}
