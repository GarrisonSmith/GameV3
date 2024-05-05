using DiscModels.Engine.TileMapping;
using Engine.Loading.Base.interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.TileMapping.Base
{
	/// <summary>
	/// Represents a tile map.
	/// </summary>
	public class TileMap : ICanBeLoaded
    {
        private bool isActiveTileMap;

        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this is the active tile map.
		/// </summary>
		public bool IsActiveTileMap
        {
            get => this.isActiveTileMap;

            set
            {
                if (this.isActiveTileMap == value)
                {
                    return;
                }

                this.isActiveTileMap = value;
                if (value)
                {
                    if (Managers.TileManager.ActiveTileMap != null)
                    {
						Managers.TileManager.ActiveTileMap.IsActiveTileMap = false;
                    }

					Managers.TileManager.ActiveTileMap = this;
                }
                else
                {
					Managers.TileManager.ActiveTileMap = null;
                    this.DeactivateTileMap();
                }
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layers. The key is the layer number.
        /// </summary>
        public Dictionary<ushort, TileMapLayer> Layers { get; set; }

		/// <summary>
		/// Initializes a new instance of the TileMap class.
		/// </summary>
		/// <param name="tileMapModel">The tile map model.</param>
		public TileMap(TileMapModel tileMapModel)
        {
			this.Guid = Guid.NewGuid();
			this.IsLoaded = true;
			this.IsActiveTileMap = tileMapModel.IsActiveTileMap;
            this.Name = tileMapModel.Name;
			this.Layers = new Dictionary<ushort, TileMapLayer>();

            foreach (var mapLayer in tileMapModel.Layers)
            {
                this.Layers.Add(mapLayer.Layer, new TileMapLayer(mapLayer));
            }

			Managers.TileManager.TileMaps.Add(this.Guid, this);
		}

		/// <summary>
		/// Initializes a new instance of the TileMap class.
		/// </summary>
        /// <param name="activatedTileMap">A value indicating whether this is the activated tile map.</param>
        /// <param name="name">The name of the tile map.</param>
		public TileMap(bool activatedTileMap, string name)
        {
			this.Guid = Guid.NewGuid();
			this.IsActiveTileMap = activatedTileMap;
			this.Name = name;
            this.Layers = new Dictionary<ushort, TileMapLayer>();
			Managers.TileManager.TileMaps.Add(this.Guid, this);
        }

        /// <summary>
        /// Deactivates the tile map.
        /// </summary>
        /// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
        /// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
        public void DeactivateTileMap(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true)
        {
            foreach (var layer in this.Layers.Values)
            {
                layer.DeactivateTileMapLayer(deactivateTileUpdating, deactivateTileDrawing);
            }
        }

        /// <summary>
        /// Gets the tile map bounds.
        /// </summary>
        /// <returns>The tile map bounds.</returns>
        public Rectangle GetTileMapBounds()
        {
			int lowestX = int.MaxValue, lowestY = int.MaxValue;
			int highestX = int.MinValue, highestY = int.MinValue;
			foreach (var tileMapLayer in this.Layers.Values)
            {
                Rectangle layerRectangle = tileMapLayer.GetTileMapLayerBounds();
                if (layerRectangle.X < lowestX)
                { 
                    lowestX = layerRectangle.X;
                }

                if (layerRectangle.X + layerRectangle.Width > highestX)
                { 
                    highestX = layerRectangle.X + layerRectangle.Width;
                }

                if (layerRectangle.Y < lowestY)
                { 
                    lowestY = layerRectangle.Y;
                }

                if (layerRectangle.Y + layerRectangle.Height > highestY)
                { 
                    highestY = layerRectangle.Y + layerRectangle.Height;
                }
            }

            return new Rectangle(lowestX, lowestY, highestX - lowestX, highestY - lowestY);
        }

		/// <summary>
		/// Gets a tile map model that corresponds to this tile map.
		/// </summary>
		/// <returns>The tile map model.</returns>
		public TileMapModel ToTileMapModel()
		{
			return new TileMapModel
			{
                IsActiveTileMap = this.IsActiveTileMap,
                Name = this.Name,
                Layers = this.Layers.Values.Select(x => x.ToTileMapLayerModel()).ToList(),
			};
		}

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			Managers.LoadManager.LoadTileMap(this);
			this.IsLoaded = true;
		}
	}
}
