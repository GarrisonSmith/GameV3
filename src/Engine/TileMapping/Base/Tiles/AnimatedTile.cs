using Engine.Core.Base;
using Engine.Drawing.Base;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base.interfaces;

namespace Engine.TileMapping.Base.Tiles
{
	/// <summary>
	/// Represents a animated tile.
	/// </summary>
	public class AnimatedTile : UpdateableAnimatedContent, IAmATile
    {
        /// <summary>
        /// Gets the layer.
        /// </summary>
        public ushort Layer { get => TileMapLayer.Layer; }

		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		public IAmACollisionArea CollisionArea { get; set; }

		/// <summary>
		/// Gets or sets the tile data.
		/// </summary>
		public TileData TileData { get; set; }

        /// <summary>
        /// Gets or sets the tile map layer.
        /// </summary>
        public TileMapLayer TileMapLayer { get; set; }

		/// <summary>
		/// Gets the tile map.
		/// </summary>
		public TileMap TileMap { get => this.TileMapLayer.TileMap; }

		/// <summary>
		/// Initializes a new instance of the Tile class.
		/// </summary>
		/// <param name="updatingActivated">A value indicating whether the content is updating.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="updateOrder">The update order.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="tileData">The tile data.</param>
		/// <param name="tileMapLayer">The tile map layer.</param>
		public AnimatedTile(bool updatingActivated, bool drawingActivated, ushort updateOrder, ushort drawOrder, Position position, IAmAArea area, IAmACollisionArea collisionArea, Animation animation, TileData tileData, TileMapLayer tileMapLayer)
            : base(updatingActivated, drawingActivated, updateOrder, drawOrder, position, area, animation)
        {
            this.TileData = tileData;
            this.TileMapLayer = tileMapLayer;
			this.CollisionArea = collisionArea;
			this.TileData.Tiles.Add(this);
			Managers.TileManager.Tiles.Add(Guid, this);
            this.TileMapLayer.AddTile((int)Position.Y / Tile.TILE_DIMENSIONS, (int)Position.X / Tile.TILE_DIMENSIONS, this);
		}

        /// <summary>
        /// Deactivates the tile.
        /// </summary>
        /// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
        /// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
        public void DeactivateTile(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true)
        {
            this.DrawingActivated = !deactivateTileDrawing;
            this.UpdatingActivated = !deactivateTileUpdating;
        }
    }
}
