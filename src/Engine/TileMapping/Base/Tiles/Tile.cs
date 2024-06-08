using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping;
using DiscModels.Engine.TileMapping.interfaces;
using Engine.Core.Base;
using Engine.Drawing.Base;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base.interfaces;

namespace Engine.TileMapping.Base.Tiles
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public class Tile : DrawableContent, IAmATile
	{
		/// <summary>
		/// Gets the tile dimensions.
		/// </summary>
		public static int TILE_DIMENSIONS { get => 64; }

		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		public IAmACollisionArea CollisionArea { get; set; }

		/// <summary>
		/// Initializes a new instance of the Tile class.
		/// </summary>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="position">The position.</param>
		/// <param name="area">The area.</param>
		/// <param name="collision">The collision.</param>
		/// <param name="tileModel">The tile model.</param>
		public Tile(bool drawingActivated, ushort drawOrder, Position position, IAmAArea area, IAmACollisionArea collision, TileModel<IAmAAreaModel, IAmACollisionAreaModel> tileModel)
			: base(drawingActivated, drawOrder, position, area, new DrawData(tileModel.DrawData))
		{
			this.CollisionArea = collision;
			Managers.TileManager.Tiles.Add(this.Guid, this);
		}

		/// <summary>
		/// Initializes a new instance of the Tile class.
		/// </summary>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="position">The position.</param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="drawData">The draw data.</param>
		public Tile(bool drawingActivated, ushort drawOrder, Position position, IAmAArea area, IAmACollisionArea collisionArea, DrawData drawData)
			: base(drawingActivated, drawOrder, position, area, drawData)
		{
			this.CollisionArea = collisionArea;
			Managers.TileManager.Tiles.Add(this.Guid, this);
		}

		/// <summary>
		/// Deactivates the tile.
		/// </summary>
		/// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
		/// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
		public void DeactivateTile(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true)
		{
			this.DrawingActivated = !deactivateTileDrawing;
		}

		/// <summary>
		/// Disposes the tile.
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			this.CollisionArea = null;
			Managers.TileManager.Tiles.Remove(this.Guid);
			Managers.DebuggingManager.CollisionTextures.Remove(this);
		}

		/// <summary>
		/// Gets a tile model that corresponds to this tile.
		/// </summary>
		/// <returns>The tile model.</returns>
		public IAmATileModel<IAmAAreaModel, IAmACollisionAreaModel> ToTileModel()
		{
			return new TileModel<IAmAAreaModel, IAmACollisionAreaModel>
			{
				Position = this.Position.ToPositionModel(),
				DrawData = this.DrawData.ToDrawDataModel(),
				Area = Managers.PhysicsManager.GetAreaModel(this.Area),
				CollisionArea = Managers.PhysicsManager.GetCollisionAreaModel(this.CollisionArea)
			};
		}
	}
}
