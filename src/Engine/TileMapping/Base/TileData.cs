using Engine.TileMapping.Base.interfaces;
using System;
using System.Collections.Generic;

namespace Engine.TileMapping.Base
{
    /// <summary>
    /// Represents a tile data.
    /// </summary>
    public class TileData
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the tile name.
        /// </summary>
        public string TileName { get; set; }

        /// <summary>
        /// Gets or sets the tiles for this tile data.
        /// </summary>
        public List<IAmATile> Tiles { get; set; }

        /// <summary>
        /// Initializes a new instance of the TileData class.
        /// </summary>
        /// <param name="tileName">The tile name.</param>
        public TileData(string tileName)
        {
            this.Guid = Guid.NewGuid();
            this.TileName = tileName;
            this.Tiles = new List<IAmATile>();
			Managers.TileManager.TileData.Add(Guid, this);
			Managers.TileManager.TileDataByTileName.Add(TileName, this);
        }
    }
}
