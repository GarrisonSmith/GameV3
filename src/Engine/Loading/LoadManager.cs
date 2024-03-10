using Engine.Drawing.Base;
using Engine.Entities.Base;
using Engine.Physics.Areas;
using Engine.Physics.Base;
using Engine.Physics.Base.enums;
using Engine.Physics.Collisions;
using Engine.Physics.Collisions.enums;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base;
using Engine.TileMapping.Base.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Engine.Loading
{
	/// <summary>
	/// Represents a load manager.
	/// </summary>
	public class LoadManager
	{
		/// <summary>
		/// Starts the load manager.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The load manager.</returns>
		public static LoadManager StartLoadManager(Game game)
		{
			return Managers.LoadManager ?? new LoadManager(game);
		}

		/// <summary>
		/// Gets or sets the game.
		/// </summary>
		protected Game Game { get; set; }

		/// <summary>
		/// Gets or sets the XMLDocuments.
		/// </summary>
		protected Dictionary<string, XmlDocument> XMLDocuments { get; set; }

		/// <summary>
		/// Initializes a new instance of the LoadManager class.
		/// </summary>
		/// <param name="game">The game.</param>
		private LoadManager(Game game)
		{
			this.Game = game;
			this.XMLDocuments = new();
		}

		/// <summary>
		/// Loads the draw manager.
		/// </summary>
		public void LoadDrawManager()
		{
			foreach (var spriteSheetName in LoadingConfiguration.TileSetNames.Select(x => x.ToLower()))
			{
				Managers.DrawManager.SpriteSheets.Add(spriteSheetName, Game.Content.Load<Texture2D>(@"TileSets\" + spriteSheetName));
			}

			foreach (var spriteSheetName in LoadingConfiguration.CharacterSetNames.Select(x => x.ToLower()))
			{
				Managers.DrawManager.SpriteSheets.Add(spriteSheetName, Game.Content.Load<Texture2D>(@"CharacterSets\" + spriteSheetName));
			}
		}

		/// <summary>
		/// Loads the tile manager.
		/// </summary>
		public void LoadTileManager()
		{
			var drawManager = Managers.DrawManager;
			if (!drawManager.IsLoaded)
			{
				drawManager.Load();
			}

			var debugColor = new Color[1];
			if (!drawManager.TryGetSpriteSheet("debug_tileset", out var debugSheet))
			{
				Console.WriteLine("Sprite sheet not found for: DEBUG");
			}

			_ = new TileData("debug_tileset|0,0");
			debugSheet.GetData(0, new Rectangle(0, 0, 1, 1), debugColor, 0, 1);
			foreach (var tileSet in LoadingConfiguration.TileSetNames.Select(x => x.ToLower()))
			{
				if (!drawManager.TryGetSpriteSheet(tileSet, out var spriteSheet))
				{
					Console.WriteLine("Sprite sheet not found for: " + tileSet);
					continue;
				}

				if (spriteSheet.Bounds.Width % Tile.TILE_DIMENSIONS != 0 && spriteSheet.Bounds.Height % Tile.TILE_DIMENSIONS != 0)
				{
					Console.WriteLine("Sprite sheet does not have proper dimensions: " + tileSet);
					continue;
				}

				for (var x = 0; x < spriteSheet.Bounds.Width - 1; x += Tile.TILE_DIMENSIONS)
				{
					for (var y = 0; y < spriteSheet.Bounds.Height - 1; y += Tile.TILE_DIMENSIONS)
					{
						var topRightPixel = new Color[1];
						var bottomLeftPixel = new Color[1];
						spriteSheet.GetData(0, new Rectangle(x, y, 1, 1), topRightPixel, 0, 1);
						spriteSheet.GetData(0, new Rectangle(x + Tile.TILE_DIMENSIONS - 1, y + Tile.TILE_DIMENSIONS - 1, 1, 1), bottomLeftPixel, 0, 1);
						if (debugColor[0] != topRightPixel[0] && debugColor[0] != bottomLeftPixel[0])
						{
							_ = new TileData(tileSet + '|' + x / Tile.TILE_DIMENSIONS + ',' + y / Tile.TILE_DIMENSIONS);
						}
					}
				}
			}
		}

		/// <summary>
		/// Gets or loads the tile draw data from the tile name.
		/// </summary>
		/// <param name="tileDrawDataName">The tile draw data name.</param>
		/// <param name="spriteSheetName">The sprite sheet name</param>
		/// <param name="col">The row.</param>
		/// <param name="row">The col.</param>
		private DrawData GetOrLoadTileDrawDataFromTileName(string tileDrawDataName, string spriteSheetName, int col, int row)
		{
			if (!Managers.DrawManager.DrawDataByName.TryGetValue(tileDrawDataName, out var drawData))
			{
				Managers.DrawManager.TryGetSpriteSheet(spriteSheetName, out var texture);
				var sheetBox = new Rectangle(col * Tile.TILE_DIMENSIONS, row * Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS);
				drawData = new DrawData(tileDrawDataName, sheetBox, texture);
			}

			return drawData;
		}

		/// <summary>
		/// Loads the tile map.
		/// </summary>
		/// <param name="tileMap">The tile map.</param>
		public void LoadTileMap(TileMap tileMap)
		{
			var tileManager = Managers.TileManager;
			if (!tileManager.IsLoaded)
			{
				tileManager.Load();
			}

			var drawManager = Managers.DrawManager;
			if (!drawManager.IsLoaded)
			{
				drawManager.Load();
			}

			var randomManager = Managers.RandomManager;
			if (!randomManager.IsLoaded)
			{
				randomManager.Load();
			}

			if (!this.XMLDocuments.TryGetValue(tileMap.Name, out var xmlDocument))
			{
				xmlDocument = new XmlDocument();
				xmlDocument.Load(Path.Combine(Game.Content.RootDirectory, "TileMaps", tileMap.Name + ".xml"));
				this.XMLDocuments.Add(tileMap.Name, xmlDocument);
			}

			foreach (XmlElement tileElement in xmlDocument.GetElementsByTagName("Tile"))
			{
				var tileName = tileElement.GetAttribute("tileName");
				string spriteSheetName = null;
				ushort? col = null, row = null;
				int? collisionWidth = null, collisionHeight = null;
				int? collisionHorizontalOffset = null, collisionVerticalOffset = null;
				List<MovementTerrainTypes> movementTerrainTypes = null;
				DrawData drawData = null;
				Animation animation = null;
				Dictionary<ushort, List<Vector2>> locations = new();
				foreach (XmlElement tileDetails in tileElement)
				{
					if (tileDetails.Name.Equals("SpriteSheet"))
					{
						spriteSheetName = tileDetails.InnerText.ToLower();
					}
					else if (tileDetails.Name.Equals("SheetCoordinates"))
					{
						col = ushort.Parse(tileDetails.GetAttribute("col"));
						row = ushort.Parse(tileDetails.GetAttribute("row"));
					}
					else if (tileDetails.Name.Equals("TileSetAnimation"))
					{
						int? currentFrameIndex = null, frameMinDuration = null, frameMaxDuration = null;
						DrawData[] frames = null;
						foreach (XmlElement animationDetails in tileDetails)
						{
							if (animationDetails.Name.Equals("CurrentFrameIndex"))
							{
								currentFrameIndex = int.Parse(animationDetails.InnerText.ToLower());
							}
							else if (animationDetails.Name.Equals("FrameMinDuration"))
							{
								frameMinDuration = int.Parse(animationDetails.InnerText.ToLower());
							}
							else if (animationDetails.Name.Equals("FrameMaxDuration"))
							{
								frameMaxDuration = int.Parse(animationDetails.InnerText.ToLower());
							}
							else if (animationDetails.Name.Equals("Frames"))
							{
								var numberOfFrames = int.Parse(animationDetails.GetAttribute("length"));
								frames = new DrawData[numberOfFrames];
								var i = 0;
								foreach (XmlElement frame in animationDetails)
								{
									var frameTileDrawDataName = frame.GetAttribute("tileName");
									var frameSpriteSheet = frame.GetAttribute("spriteSheet");
									var frameCol = int.Parse(frame.GetAttribute("col"));
									var frameRow = int.Parse(frame.GetAttribute("row"));
									frames[i] = this.GetOrLoadTileDrawDataFromTileName(frameTileDrawDataName, frameSpriteSheet, frameCol, frameRow);
									i++;
								}
							}
						}

						animation = new Animation(currentFrameIndex.Value, frameMinDuration.Value, frameMaxDuration.Value, frames);
					}
					else if (tileDetails.Name.Equals("Locations"))
					{
						ushort layer = ushort.Parse(tileDetails.GetAttribute("layer"));
						foreach (XmlElement locationElement in tileDetails)
						{
							var x = int.Parse(locationElement.GetAttribute("x"));
							var y = int.Parse(locationElement.GetAttribute("y"));
							if (!locations.TryGetValue(layer, out var locationList))
							{
								locationList = new List<Vector2>();
								locations.Add(layer, locationList);
							}

							locationList.Add(new Vector2(x, y));
						}
					}
					else if (tileDetails.Name.Equals("TileCollision"))
					{
						foreach (XmlElement collisionElement in tileDetails)
						{
							if (collisionElement.Name.Equals("MovementTerrainTypes"))
							{
								movementTerrainTypes = new List<MovementTerrainTypes>();
								foreach (XmlElement movementTerrainTypeElement in collisionElement)
								{
									switch (movementTerrainTypeElement.InnerText)
									{
										case "Blocked":
											movementTerrainTypes.Add(MovementTerrainTypes.Blocked);
											break;
										case "Land":
											movementTerrainTypes.Add(MovementTerrainTypes.Land);
											break;
										case "Water":
											movementTerrainTypes.Add(MovementTerrainTypes.Water);
											break;
										case "Air":
											movementTerrainTypes.Add(MovementTerrainTypes.Water);
											break;
										case "Fire":
											movementTerrainTypes.Add(MovementTerrainTypes.Water);
											break;
									}
								}
							}
							else if (collisionElement.Name.Equals("Width"))
							{
								if (collisionElement.InnerText.Equals("TILE_DIMENSIONS"))
								{
									collisionWidth = Tile.TILE_DIMENSIONS;
								}
								else
								{
									collisionWidth = int.Parse(collisionElement.InnerText);
								}
							}
							else if (collisionElement.Name.Equals("Height"))
							{
								if (collisionElement.InnerText.Equals("TILE_DIMENSIONS"))
								{
									collisionHeight = Tile.TILE_DIMENSIONS;
								}
								else
								{
									collisionHeight = int.Parse(collisionElement.InnerText);
								}
							}
							else if (collisionElement.Name.Equals("HorizontalOffset"))
							{
								collisionHorizontalOffset = int.Parse(collisionElement.InnerText);
							}
							else if (collisionElement.Name.Equals("VerticalOffset"))
							{
								collisionVerticalOffset = int.Parse(collisionElement.InnerText);
							}
						}
					}
				}

				if (string.IsNullOrEmpty(tileName))
				{
					Console.WriteLine("Tile with no tile name provided: " + tileMap.Name);
				}

				if (string.IsNullOrEmpty(spriteSheetName) && animation != null)
				{
					Console.WriteLine("Tile with no sprite sheet name provided: " + tileName + "," + tileMap.Name);
				}

				if ((!row.HasValue || !col.HasValue) && animation != null)
				{
					Console.WriteLine("Tile with no sprite sheet row or column provided: " + tileName + "," + tileMap.Name);
				}

				if (locations.Count == 0)
				{
					Console.WriteLine("Tile contains no locations on " + tileName + "," + tileMap.Name);
				}

				if (animation == null && string.IsNullOrEmpty(spriteSheetName))
				{
					Console.WriteLine("Animated Tile with no animation: " + tileName + "," + tileMap.Name);
				}

				if (!tileManager.TileDataByTileName.TryGetValue(tileName, out var tileData))
				{
					Console.WriteLine("Tile has no tile data on " + tileName + "," + tileMap.Name);
				}

				if (animation == null)
				{
					var drawDataName = spriteSheetName + '|' + col.Value + ',' + row.Value;
					drawData = this.GetOrLoadTileDrawDataFromTileName(drawDataName, spriteSheetName, col.Value, row.Value);
				}

				foreach (var layer in locations.Keys)
				{
					if (!tileMap.Layers.TryGetValue(layer, out var tileMapLayer))
					{
						tileMapLayer = new TileMapLayer(layer, tileMap);
					}

					foreach (var location in locations[layer])
					{
						var position = new Position(location);
						var area = new SimpleArea(position, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS);
						IAmACollisionArea collision = null;
						if (collisionWidth.HasValue && collisionHeight.HasValue && collisionHorizontalOffset.HasValue && collisionVerticalOffset.HasValue && movementTerrainTypes != null && movementTerrainTypes.Count > 0)
						{
							if (collisionWidth == Tile.TILE_DIMENSIONS && collisionHeight == Tile.TILE_DIMENSIONS && collisionHorizontalOffset == 0 && collisionVerticalOffset == 0)
							{
								collision = new SimpleCollisionArea(area, movementTerrainTypes);
							}
							else
							{
								//need to make a complex collision area here.
							}
						}

						if (animation == null)
						{
							_ = new Tile(true, layer, position, area, collision, drawData, tileData, tileMapLayer);
						}
						else
						{
							_ = new AnimatedTile(true, true, layer, layer, position, area, collision, animation.CloneAnimation(), tileData, tileMapLayer);
						}
					}
				}
			}
		}

		/// <summary>
		/// Loads the entity manager.
		/// </summary>
		public void LoadEntityManager()
		{
			var drawManager = Managers.DrawManager;
			if (!drawManager.IsLoaded)
			{
				drawManager.Load();
			}

			var debugColor = new Color[1];
			if (!drawManager.TryGetSpriteSheet("debug_tileset", out var debugSheet))
			{
				Console.WriteLine("Sprite sheet not found for: DEBUG");
			}

			debugSheet.GetData(0, new Rectangle(0, 0, 1, 1), debugColor, 0, 1);

			foreach (string characterSet in LoadingConfiguration.CharacterSetNames.Select(x => x.ToLower()))
			{
				if (!drawManager.TryGetSpriteSheet(characterSet, out var spriteSheet))
				{
					Console.WriteLine("Sprite sheet not found for: " + characterSet);
					continue;
				}

				if (spriteSheet.Bounds.Width % 64 != 0 && spriteSheet.Bounds.Height % 64 != 0)
				{
					Console.WriteLine("Sprite sheet does not have proper dimensions: " + characterSet);
					continue;
				}

				for (var col = 0; col < spriteSheet.Bounds.Width - 1; col += 64) //TODO config entity width
				{
					for (var row = 0; row < spriteSheet.Bounds.Height - 1; row += 128) //TODO config entity height
					{
						var topRightPixel = new Color[1];
						var bottomLeftPixel = new Color[1];
						spriteSheet.GetData(0, new Rectangle(col, row, 1, 1), topRightPixel, 0, 1);
						spriteSheet.GetData(0, new Rectangle(col + 63, row + 63, 1, 1), bottomLeftPixel, 0, 1);
						if (debugColor[0] != topRightPixel[0] && debugColor[0] != bottomLeftPixel[0])
						{
							_ = new EntityData(characterSet + '|' + col / 64 + ',' + row / 128);
						}
					}
				}
			}
		}

		/// <summary>
		/// Loads the entity.
		/// </summary>
		/// <param name="entityName">The entity name.</param>
		public void LoadEntity(string entityName)
		{
			var entityManager = Managers.EntityManager;
			if (!entityManager.IsLoaded)
			{
				entityManager.Load();
			}

			var tileManager = Managers.TileManager;
			if (!tileManager.IsLoaded)
			{
				tileManager.Load();
			}

			var physicsManager = Managers.PhysicsManager;
			if (!physicsManager.IsLoaded)
			{ 
				physicsManager.Load();
			}

			var animations = this.GetEntityAnimations(entityName);
			var position = new Position(64, 64); //TODO entity starting location
			var area = new SimpleArea(position, 64, 128);
			var collision = new OffsetCollisionArea(new OffsetArea(position, 11, 117, 42, 11), new List<MovementTerrainTypes> { MovementTerrainTypes.Entity });
			_ = new Entity(true, true, 1, 1, OrientationTypes.Downward, new MoveSpeed(20), position, area, collision, animations[0], animations, tileManager.ActiveTileMap.Layers[1]);
		}

		/// <summary>
		/// Gets the entity animations.
		/// </summary>
		/// <returns>The entity animations.</returns>
		private Animation[] GetEntityAnimations(string entityName)
		{
			var drawManager = Managers.DrawManager;
			if (!drawManager.IsLoaded)
			{
				drawManager.Load();
			}

			var randomManager = Managers.RandomManager;
			if (!randomManager.IsLoaded)
			{
				randomManager.Load();
			}

			var spriteSheetName = entityName + "_characterset";
			if (!drawManager.SpriteSheets.TryGetValue(spriteSheetName, out var spriteSheet))
			{
				Console.WriteLine("Sprite sheet not found for: " + spriteSheetName);
			}

			var animations = new Animation[4];
			var animationIndex = 0;
			for (var y = 0; y < spriteSheet.Bounds.Height - 1; y += 128) //TODO config entity height
			{
				var rowFrames = new DrawData[4];
				var framesIndex = 0;
				for (var x = 0; x < (64 * 4) - 1; x += 64) //TODO config entity width
				{
					var drawDataName = spriteSheetName + '|' + x + ',' + y;
					if (!drawManager.DrawDataByName.TryGetValue(drawDataName, out var drawData))
					{
						drawManager.TryGetSpriteSheet(spriteSheetName, out var texture);
						var sheetBox = new Rectangle(x, y, 64, 128);
						drawData = new DrawData(drawDataName, sheetBox, texture);
					}

					rowFrames[framesIndex++] = drawData;
				}

				animations[animationIndex++] = new Animation(false, 0, 200, rowFrames); //TODO config entity animations
			}

			return animations;
		}
	}
}
