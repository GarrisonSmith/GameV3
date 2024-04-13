﻿using Engine.Core.Base;
using Engine.Drawing.Base;
using Engine.Physics.Areas.interfaces;
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
        /// Gets the layer.
        /// </summary>
        public ushort Layer { get => TileMapLayer.Layer; }

        /// <summary>
        /// Gets or sets the collision area.
        /// </summary>
		public IAmACollisionArea CollisionArea { get; set; }

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
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="drawData">The draw data.</param>
		/// <param name="tileMapLayer">The tile map layer.</param>
		public Tile(bool drawingActivated, ushort drawOrder, IAmAArea area, IAmACollisionArea collisionArea, DrawData drawData, TileMapLayer tileMapLayer)
            : base(drawingActivated, drawOrder, area, drawData)
        {
            this.TileMapLayer = tileMapLayer;
			this.CollisionArea = collisionArea;
			Managers.TileManager.Tiles.Add(this.Guid, this);
            this.TileMapLayer.AddTile((int)this.Position.Y / Tile.TILE_DIMENSIONS, (int)this.Position.X / Tile.TILE_DIMENSIONS, this);
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
	}
}
