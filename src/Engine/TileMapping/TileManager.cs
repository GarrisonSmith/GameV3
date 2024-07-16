using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping;
using DiscModels.Engine.TileMapping.interfaces;
using Engine.Loading.Base.interfaces;
using Engine.Loading.Configurations;
using Engine.Physics.Base;
using Engine.TileMapping.Base;
using Engine.TileMapping.Base.interfaces;
using Engine.TileMapping.Base.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Initializes a new instance of the TileManager class.
		/// </summary>
		private TileManager()
		{
			this.TileMaps = new Dictionary<Guid, TileMap>();
			this.TileMapLayers = new Dictionary<Guid, TileMapLayer>();
			this.Tiles = new Dictionary<Guid, IAmATile>();
			this.IsLoaded = false;
		}

		/// <summary>
		/// Gets or sets a value indicating if a tile map is loaded.
		/// </summary>
		/// <returns></returns>
		public bool TileMapIsLoad(string tileMapName)
		{
			return true == this.TileMaps?.Any(x => x.Value.Name == tileMapName);
		}

		/// <summary>
		/// Gets the tile.
		/// </summary>
		/// <param name="layer">The layer.</param>
		/// <param name="tileModel">The tile model.</param>
		/// <returns>The tile.</returns>
		public IAmATile GetTile(ushort layer, IAmATileModel<IAmAAreaModel, IAmACollisionAreaModel> tileModel)
		{
			if (tileModel == null)
			{
				return null;
			}

			var position = new Position(tileModel.Position);
			var area = Managers.PhysicsManager.GetArea(position, tileModel.Area);
			var collisionArea = Managers.PhysicsManager.GetCollisionArea(position, tileModel.CollisionArea);

			switch (tileModel)
			{
				case TileModel<IAmAAreaModel, IAmACollisionAreaModel> model:
					return new Tile(true, layer, position, area, collisionArea, model);
				case AnimatedTileModel<IAmAAreaModel, IAmACollisionAreaModel> model:
					return new AnimatedTile(true, true, layer, layer, position, area, collisionArea, model);
			}

			return null;
		}

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			if (this.IsLoaded)
			{
				return;
			}

			Managers.DrawManager.Load();

			if (!Managers.DrawManager.TryGetSpriteSheet("debug_tileset", out _))
			{
				Console.WriteLine("Sprite sheet not found for: DEBUG");
			}

			foreach (var tileSet in TileSetsConfig.TileSetFileNames.Select(x => x.ToLower()))
			{
				if (!Managers.DrawManager.TryGetSpriteSheet(tileSet, out var spriteSheet))
				{
					Console.WriteLine("Sprite sheet not found for: " + tileSet);
					continue;
				}

				if (spriteSheet.Bounds.Width % Tile.TILE_DIMENSIONS != 0 && spriteSheet.Bounds.Height % Tile.TILE_DIMENSIONS != 0)
				{
					Console.WriteLine("Sprite sheet does not have proper dimensions: " + tileSet);
					continue;
				}
			}

			this.IsLoaded = true;
		}
	}
}
