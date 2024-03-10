using Engine.Loading.Base.interfaces;
using Engine.TileMapping.Base;
using Engine.TileMapping.Base.interfaces;
using System;
using System.Collections.Generic;

namespace Engine.TileMapping
{
	/// <summary>
	/// Represents a tile manager.
	/// </summary>
	public class TileManager : ICanBeLoaded
    {
        /// <summary>
        /// Starts the tile manager.
        /// </summary>
        /// <returns>The tile manager.</returns>
        public static TileManager StartTileManager()
        {
            return Managers.TileManager ?? new TileManager();
        }

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the active tile map.
		/// </summary>
		public TileMap ActiveTileMap { get; set; }

        /// <summary>
        /// Gets or sets the tile maps.
        /// </summary>
        public Dictionary<Guid, TileMap> TileMaps { get; set; }

        /// <summary>
        /// Gets or sets the tile map layers.
        /// </summary>
        public Dictionary<Guid, TileMapLayer> TileMapLayers { get; set; }

        /// <summary>
        /// Gets or sets the tiles.
        /// </summary>
        public Dictionary<Guid, IAmATile> Tiles { get; set; }

        /// <summary>
        /// Gets or sets the tile data.
        /// </summary>
        public Dictionary<Guid, TileData> TileData { get; set; }

		/// <summary>
		/// Gets or sets the tile data by tile name.
		/// </summary>
		public Dictionary<string, TileData> TileDataByTileName { get; set; }

		/// <summary>
		/// Initializes a new instance of the TileManager class.
		/// </summary>
		private TileManager()
        {
			this.TileMaps = new();
			this.TileMapLayers = new();
			this.Tiles = new();
			this.TileData = new();
			this.TileDataByTileName = new();
            this.IsLoaded = false;
        }

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			Managers.LoadManager.LoadTileManager();
			this.IsLoaded = true;
		}
	}
}
