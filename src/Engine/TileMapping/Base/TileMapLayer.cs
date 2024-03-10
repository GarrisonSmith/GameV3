﻿using Engine.Entities.Base.interfaces;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base.interfaces;
using Engine.TileMapping.Base.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.TileMapping.Base
{
	/// <summary>
	/// Represents a tile map layer.
	/// </summary>
	public class TileMapLayer
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the layer.
        /// </summary>
        public ushort Layer { get; set; }

        /// <summary>
        /// Gets or sets the tile map.
        /// </summary>
        public TileMap TileMap { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        public List<IAmAEntity> Entities { get; set; }

        /// <summary>
        /// Gets or sets the tiles. The first key is the row, the second key is the column.
        /// </summary>
        public Dictionary<int, Dictionary<int, IAmATile>> Tiles { get; set; }

		/// <summary>
		/// Gets or sets the tiles with collision. The first key is the row, the second key is the column.
		/// </summary>
		public Dictionary<int, Dictionary<int, IAmATile>> TilesWithCollision { get; set; }

        /// <summary>
        /// Initializes a new instance of the TileMapLayer class.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="tileMap">The tile map.</param>
        public TileMapLayer(ushort layer, TileMap tileMap)
        {
            this.Guid = Guid.NewGuid();
			this.Layer = layer;
			this.TileMap = tileMap;
			this.TileMap.Layers.Add(layer, this);
            this.Entities = new List<IAmAEntity>();
            this.Tiles = new Dictionary<int, Dictionary<int, IAmATile>>();
			this.TilesWithCollision = new Dictionary<int, Dictionary<int, IAmATile>>();
			Managers.TileManager.TileMapLayers.Add(this.Guid, this);
		}

		/// <summary>
		/// Tries to get the tile.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="col">The col.</param>
		/// <param name="tile">The tile.</param>
		/// <returns>A value indicating whether the tile was found.</returns>
		public bool TryGetTile(int row, int col, out IAmATile tile)
        {
			tile = default;
            return this.Tiles.TryGetValue(row, out var colDictionary) && colDictionary.TryGetValue(col, out tile);
        }

        /// <summary>
        /// Tries to get the tile collision area.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <param name="collisionArea">The collision area.</param>
        /// <returns>A value indicating whether the collision are was found.</returns>
        public bool TryGetTileCollision(int row, int col, out IAmACollisionArea collisionArea)
        { 
            collisionArea = default;
            if (this.TilesWithCollision.TryGetValue(row, out var colDictionary) && colDictionary.TryGetValue(col, out var tile))
            {
                if (tile.CollisionArea != null)
                {
					collisionArea = tile.CollisionArea;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds the tile.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        /// <param name="tile">The tile.</param>
        public void AddTile(int row, int col, IAmATile tile)
        {
            if (!this.Tiles.TryGetValue(row, out var colDictionary))
            {
				colDictionary = new Dictionary<int, IAmATile>();
				this.Tiles.Add(row, colDictionary);
            }

            colDictionary.Add(col, tile);

            if (tile.CollisionArea != null)
            {
                if (!this.TilesWithCollision.TryGetValue(row, out var colCollisionDictionary))
                {
					colCollisionDictionary = new Dictionary<int, IAmATile>();
                    this.TilesWithCollision.Add(row, colCollisionDictionary);
                }

				colCollisionDictionary.Add(col, tile);
			}
		}

		/// <summary>
		/// Deactivates the tile map layer.
		/// </summary>
		/// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
		/// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
		public void DeactivateTileMapLayer(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true)
        {
            foreach (var columnDictionary in this.Tiles.Values)
            {
                foreach (var tile in columnDictionary.Values)
                {
					tile.DeactivateTile(deactivateTileUpdating, deactivateTileDrawing);
				}
            }
        }

		/// <summary>
		/// Gets the tile map bounds.
		/// </summary>
		/// <returns>The tile map bounds.</returns>
		public Rectangle GetTileMapLayerBounds()
		{
            int lowestRow = int.MaxValue, lowestColumn = int.MaxValue;
            int highestRow = int.MinValue, highestColumn = int.MinValue;
            foreach (var row in this.Tiles.Keys)
            {
				if (row < lowestRow)
				{
					lowestRow = row;
				}

				if (row > highestRow)
				{
					highestRow = row;
				}

				foreach (var column in this.Tiles[row].Keys)
                {
					if (column < lowestColumn)
					{
						lowestColumn = column;
					}

					if (column > highestColumn)
					{
						highestColumn = column;
					}
				}
            }

            return new Rectangle(lowestColumn * Tile.TILE_DIMENSIONS, lowestRow * Tile.TILE_DIMENSIONS, 
                (highestColumn * Tile.TILE_DIMENSIONS) + Tile.TILE_DIMENSIONS - lowestColumn, 
                (highestRow * Tile.TILE_DIMENSIONS) + Tile.TILE_DIMENSIONS - lowestRow);
		}
	}
}
