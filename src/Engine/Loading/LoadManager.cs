using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Areas;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.Physics.Collisions;
using DiscModels.Engine.TileMapping;
using Engine.Drawing.Base;
using Engine.Entities.Base;
using Engine.Loading.Configurations;
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
using System.Net.WebSockets;
using System.Runtime.Serialization.Json;
using System.Xml;
using static System.Net.WebRequestMethods;
using System.Text;

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
		public static LoadManager StartLoadManager()
		{
			return Managers.LoadManager ?? new LoadManager();
		}

		/// <summary>
		/// Gets or sets the XMLDocuments.
		/// </summary>
		protected Dictionary<string, XmlDocument> XMLDocuments { get; set; }

		/// <summary>
		/// Initializes a new instance of the LoadManager class.
		/// </summary>
		private LoadManager()
		{
			this.XMLDocuments = new();
		}

		/// <summary>
		/// Loads the draw manager.
		/// </summary>
		public void LoadDrawManager()
		{
			foreach (var spriteSheetName in TileSetsConfig.TileSetFileNames)
			{
				Managers.DrawManager.SpriteSheets.Add(spriteSheetName, Managers.Game.Content.Load<Texture2D>(@"TileSets\" + spriteSheetName));
			}

			foreach (var spriteSheetName in CharactersConfig.CharacterFileNames)
			{
				Managers.DrawManager.SpriteSheets.Add(spriteSheetName, Managers.Game.Content.Load<Texture2D>(@"CharacterSets\" + spriteSheetName));
			}

			foreach (var fontName in FontsConfig.FontFileNames)
			{
				Managers.DrawManager.SpriteFonts.Add(fontName, Managers.Game.Content.Load<SpriteFont>(@"Fonts\" + fontName));
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

			debugSheet.GetData(0, new Rectangle(0, 0, 1, 1), debugColor, 0, 1);
			foreach (var tileSet in TileSetsConfig.TileSetFileNames.Select(x => x.ToLower()))
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
			}
		}

		/// <summary>
		/// Gets or loads the tile draw data.
		/// </summary>
		/// <param name="spriteSheetName">The sprite sheet name</param>
		/// <param name="col">The row.</param>
		/// <param name="row">The col.</param>
		/// <returns>The draw data.</returns>
		private DrawData LoadTileDrawData(string spriteSheetName, int col, int row)
		{
			var spritesheetCoordinate = new Point(col * Tile.TILE_DIMENSIONS, row * Tile.TILE_DIMENSIONS);
			var spritesheetBox = new Rectangle(spritesheetCoordinate.X, spritesheetCoordinate.Y, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS);

			return this.LoadTileDrawData(spriteSheetName, spritesheetBox, spritesheetCoordinate);
		}

		/// <summary>
		/// Gets or loads the tile draw data.
		/// </summary>
		/// <param name="drawDataModel">The draw data model.</param>
		/// <returns>The draw data.</returns>
		public DrawData LoadTileDrawData(DrawDataModel drawDataModel)
		{
			return this.LoadTileDrawData(drawDataModel.SpritesheetName, drawDataModel.SpritesheetBox, drawDataModel.SpritesheetBox.Location);
		}

		/// <summary>
		/// Gets or loads the tile draw data.
		/// </summary>
		/// <param name="spriteSheetName">The spritesheet name.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <param name="spritesheetCoordinate">The spritesheet coordinate.</param>
		/// <returns>The draw data.</returns>
		public DrawData LoadTileDrawData(string spriteSheetName, Rectangle spritesheetBox, Point spritesheetCoordinate)
		{
			if (Managers.DrawManager.TryGetSpriteSheet(spriteSheetName, out var spritesheet))
			{
				var textureBox = new Rectangle(2, 2, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS);
				if (Managers.DrawManager.DrawDataInstanceByName.TryGetValue(spriteSheetName + spritesheetBox.ToString(), out var drawData))
				{
					return drawData.CloneDrawData();
				}

				var tileTextureData = this.GetTextureDataFromSpritesheetArea(spritesheet, new Rectangle(spritesheetCoordinate.X, spritesheetCoordinate.Y, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS));
				var leftTextureData = new Color[Tile.TILE_DIMENSIONS * 2];
				var rightTextureData = new Color[Tile.TILE_DIMENSIONS * 2];
				var topTextureData = new Color[Tile.TILE_DIMENSIONS * 2];
				var bottomTextureData = new Color[Tile.TILE_DIMENSIONS * 2];

				for (int i = 0; i < Tile.TILE_DIMENSIONS; i++)
				{
					//left of texture
					var leftIndex = i * Tile.TILE_DIMENSIONS;
					leftTextureData[i * 2] = leftTextureData[i * 2 + 1] = tileTextureData[leftIndex];

					//right of texture
					var rightIndex = leftIndex + Tile.TILE_DIMENSIONS - 1;
					rightTextureData[i * 2] = rightTextureData[i * 2 + 1] = tileTextureData[rightIndex];

					//top of texture
					var topIndex = i;
					topTextureData[i] = topTextureData[i + Tile.TILE_DIMENSIONS] = tileTextureData[topIndex];

					//bottom of texture
					var bottomIndex = tileTextureData.Length - Tile.TILE_DIMENSIONS + i;
					bottomTextureData[i] = bottomTextureData[i + Tile.TILE_DIMENSIONS] = tileTextureData[bottomIndex];
				}

				var leftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, Tile.TILE_DIMENSIONS);
				leftTexture.SetData(leftTextureData);

				var rightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, Tile.TILE_DIMENSIONS);
				rightTexture.SetData(rightTextureData);

				var topTexture = new Texture2D(Managers.Graphics.GraphicsDevice, Tile.TILE_DIMENSIONS, 2);
				topTexture.SetData(topTextureData);

				var bottomTexture = new Texture2D(Managers.Graphics.GraphicsDevice, Tile.TILE_DIMENSIONS, 2);
				bottomTexture.SetData(bottomTextureData);

				var topLeftPixel = tileTextureData[0];
				var topLeftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				topLeftTexture.SetData(new Color[] { topLeftPixel, topLeftPixel, topLeftPixel, topLeftPixel });

				var topRightPixel = tileTextureData[Tile.TILE_DIMENSIONS];
				var topRightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				topRightTexture.SetData(new Color[] { topRightPixel, topRightPixel, topRightPixel, topRightPixel });

				var bottomLeftPixel = tileTextureData[tileTextureData.Length - Tile.TILE_DIMENSIONS];
				var bottomLeftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				bottomLeftTexture.SetData(new Color[] { bottomLeftPixel, bottomLeftPixel, bottomLeftPixel, bottomLeftPixel });

				var bottomRightPixel = tileTextureData[tileTextureData.Length - 1];
				var bottomRightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				bottomRightTexture.SetData(new Color[] { bottomRightPixel, bottomRightPixel, bottomRightPixel, bottomRightPixel });

				var middleTexture = new Texture2D(Managers.Graphics.GraphicsDevice, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS);
				middleTexture.SetData(tileTextureData);

				var textures = new Texture2D[][]
				{
					new Texture2D[] { topLeftTexture, topTexture, topRightTexture },
					new Texture2D[] { leftTexture, middleTexture, rightTexture },
					new Texture2D[] { bottomLeftTexture, bottomTexture, bottomRightTexture }
				};

				var tileTexture = this.CombineTexture(textures, Tile.TILE_DIMENSIONS + 4, Tile.TILE_DIMENSIONS + 4);

				drawData = new DrawData(spriteSheetName, spritesheetCoordinate, textureBox, tileTexture);
				Managers.DrawManager.DrawDataInstanceByName.Add(spriteSheetName + spritesheetBox.ToString(), drawData.CloneDrawData());

				return drawData;
			}

			return null; //missing spritesheet.
		}

		/// <summary>
		/// Gets the tile texture.
		/// </summary>
		/// <param name="spriteSheetName">The sprite sheet name.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <param name="spritesheetCoordinate">The spritesheet coordinate.</param>
		/// <returns>The texture/</returns>
		public Texture2D GetTileTexture(string spriteSheetName, Rectangle spritesheetBox, Point spritesheetCoordinate)
		{
			if (Managers.DrawManager.TryGetSpriteSheet(spriteSheetName, out var spritesheet))
			{
				if (Managers.DrawManager.DrawDataInstanceByName.TryGetValue(spriteSheetName + spritesheetBox.ToString(), out var drawData))
				{
					return drawData.CloneDrawData().Texture;
				}

				var tileTextureData = this.GetTextureDataFromSpritesheetArea(spritesheet, new Rectangle(spritesheetCoordinate.X, spritesheetCoordinate.Y, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS));
				var leftTextureData = new Color[Tile.TILE_DIMENSIONS * 2];
				var rightTextureData = new Color[Tile.TILE_DIMENSIONS * 2];
				var topTextureData = new Color[Tile.TILE_DIMENSIONS * 2];
				var bottomTextureData = new Color[Tile.TILE_DIMENSIONS * 2];

				for (int i = 0; i < Tile.TILE_DIMENSIONS; i++)
				{
					//left of texture
					var leftIndex = i * Tile.TILE_DIMENSIONS;
					leftTextureData[i * 2] = leftTextureData[i * 2 + 1] = tileTextureData[leftIndex];

					//right of texture
					var rightIndex = leftIndex + Tile.TILE_DIMENSIONS - 1;
					rightTextureData[i * 2] = rightTextureData[i * 2 + 1] = tileTextureData[rightIndex];

					//top of texture
					var topIndex = i;
					topTextureData[i] = topTextureData[i + Tile.TILE_DIMENSIONS] = tileTextureData[topIndex];

					//bottom of texture
					var bottomIndex = tileTextureData.Length - Tile.TILE_DIMENSIONS + i;
					bottomTextureData[i] = bottomTextureData[i + Tile.TILE_DIMENSIONS] = tileTextureData[bottomIndex];
				}

				var leftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, Tile.TILE_DIMENSIONS);
				leftTexture.SetData(leftTextureData);

				var rightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, Tile.TILE_DIMENSIONS);
				rightTexture.SetData(rightTextureData);

				var topTexture = new Texture2D(Managers.Graphics.GraphicsDevice, Tile.TILE_DIMENSIONS, 2);
				topTexture.SetData(topTextureData);

				var bottomTexture = new Texture2D(Managers.Graphics.GraphicsDevice, Tile.TILE_DIMENSIONS, 2);
				bottomTexture.SetData(bottomTextureData);

				var topLeftPixel = tileTextureData[0];
				var topLeftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				topLeftTexture.SetData(new Color[] { topLeftPixel, topLeftPixel, topLeftPixel, topLeftPixel });

				var topRightPixel = tileTextureData[Tile.TILE_DIMENSIONS];
				var topRightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				topRightTexture.SetData(new Color[] { topRightPixel, topRightPixel, topRightPixel, topRightPixel });

				var bottomLeftPixel = tileTextureData[tileTextureData.Length - Tile.TILE_DIMENSIONS];
				var bottomLeftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				bottomLeftTexture.SetData(new Color[] { bottomLeftPixel, bottomLeftPixel, bottomLeftPixel, bottomLeftPixel });

				var bottomRightPixel = tileTextureData[tileTextureData.Length - 1];
				var bottomRightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				bottomRightTexture.SetData(new Color[] { bottomRightPixel, bottomRightPixel, bottomRightPixel, bottomRightPixel });

				var middleTexture = new Texture2D(Managers.Graphics.GraphicsDevice, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS);
				middleTexture.SetData(tileTextureData);

				var textures = new Texture2D[][]
				{
					new Texture2D[] { topLeftTexture, topTexture, topRightTexture },
					new Texture2D[] { leftTexture, middleTexture, rightTexture },
					new Texture2D[] { bottomLeftTexture, bottomTexture, bottomRightTexture }
				};

				var tileTexture = this.CombineTexture(textures, Tile.TILE_DIMENSIONS + 4, Tile.TILE_DIMENSIONS + 4);

				return tileTexture;
			}

			return null; //missing spritesheet.
		}

		/// <summary>
		/// Gets the texture data from the area of the texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="area">The area.</param>
		/// <returns>The texture data for the area of the texture.</returns>
		private Color[] GetTextureDataFromSpritesheetArea(Texture2D texture, Rectangle area)
		{
			var renderTarget = new RenderTarget2D(Managers.Graphics.GraphicsDevice, area.Width, area.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			Managers.Graphics.GraphicsDevice.SetRenderTarget(renderTarget);

			Managers.DrawManager.Begin();

			Managers.DrawManager.Draw(texture, area);

			Managers.DrawManager.End();

			Managers.Graphics.GraphicsDevice.SetRenderTarget(null);
			Color[] data = new Color[renderTarget.Width * renderTarget.Height];
			renderTarget.GetData(data);

			return data;
		}

		/// <summary>
		/// Combines the textures.
		/// </summary>
		/// <param name="textures">The textures.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The combined texture.</returns>
		private Texture2D CombineTexture(Texture2D[][] textures, int width, int height)
		{
			var renderTarget = new RenderTarget2D(Managers.Graphics.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			Managers.Graphics.GraphicsDevice.SetRenderTarget(renderTarget);

			Managers.DrawManager.Begin();

			var verticalOffset = 0;
			var horizontalOffset = 0;

			foreach (var textureRows in textures)
			{
				horizontalOffset = 0;

				foreach (var texture in textureRows)
				{
					Managers.DrawManager.Draw(texture, horizontalOffset, verticalOffset);
					horizontalOffset += texture.Width;
				}

				verticalOffset += textureRows.Select(x => x.Height).OrderByDescending(x => x).FirstOrDefault();
			}

			Managers.DrawManager.End();

			Managers.Graphics.GraphicsDevice.SetRenderTarget(null);
			Color[] data = new Color[renderTarget.Width * renderTarget.Height];
			renderTarget.GetData(data);
			var combinedTexture = new Texture2D(Managers.Graphics.GraphicsDevice, renderTarget.Width, renderTarget.Height);
			combinedTexture.SetData(data);

			return combinedTexture;
		}

		public void SaveTileMap(TileMap tileMap)
		{
			var tileMapModel = tileMap.ToTileMapModel();
			var tileMapJson = this.SerializeToJson(tileMapModel);

			var tileMapPath = Path.Combine(Managers.Game.Content.RootDirectory, "TileMaps", tileMap.Name + ".json");
            System.IO.File.WriteAllText(tileMapPath, tileMapJson);
		}

		private string SerializeToJson(TileMapModel tileMap)
		{
			using (var stream = new MemoryStream())
			{
				var serializer = new DataContractJsonSerializer(typeof(TileMapModel), GetKnownTypes());
				serializer.WriteObject(stream, tileMap);
				stream.Position = 0;
				using (var reader = new StreamReader(stream))
				{
					return reader.ReadToEnd();
				}
			}
		}

		private Type[] GetKnownTypes()
		{
			return new Type[] { typeof(TileModel<IAmAAreaModel, IAmACollisionAreaModel>), typeof(AnimatedTileModel<IAmAAreaModel, IAmACollisionAreaModel>), typeof(SimpleAreaModel), typeof(SimpleCollisionAreaModel) };
		}

		public void LoadTileMap(string tileMapName)
		{
			if (Managers.TileManager.TileMapIsLoad(tileMapName))
			{
				return;
			}

			var tileMapPath = Path.Combine(Managers.Game.Content.RootDirectory, "TileMaps", tileMapName + ".json");
			var tileMapJson = System.IO.File.ReadAllText(tileMapPath);
			var tileMapModel = DeserializeFromJson(tileMapJson);
			_ = new TileMap(tileMapModel);
		}

		private TileMapModel DeserializeFromJson(string json)
		{
			using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				var serializer = new DataContractJsonSerializer(typeof(TileMapModel), GetKnownTypes());
				return (TileMapModel)serializer.ReadObject(stream);
			}
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
				xmlDocument.Load(Path.Combine(Managers.Game.Content.RootDirectory, "TileMaps", tileMap.Name + ".xml"));
				this.XMLDocuments.Add(tileMap.Name, xmlDocument);
			}

			foreach (XmlElement tileElement in xmlDocument.GetElementsByTagName("Tile"))
			{
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
									var frameSpriteSheet = frame.GetAttribute("spriteSheet");
									var frameCol = int.Parse(frame.GetAttribute("col"));
									var frameRow = int.Parse(frame.GetAttribute("row"));
									frames[i] = this.LoadTileDrawData(frameSpriteSheet, frameCol, frameRow);
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

				if (string.IsNullOrEmpty(spriteSheetName) && animation != null)
				{
					Console.WriteLine("Tile with no sprite sheet name provided: " + tileMap.Name);
				}

				if ((!row.HasValue || !col.HasValue) && animation != null)
				{
					Console.WriteLine("Tile with no sprite sheet row or column provided: " + tileMap.Name);
				}

				if (locations.Count == 0)
				{
					Console.WriteLine("Tile contains no locations on " + tileMap.Name);
				}

				if (animation == null && string.IsNullOrEmpty(spriteSheetName))
				{
					Console.WriteLine("Animated Tile with no animation: " + tileMap.Name);
				}

				if (animation == null)
				{
					drawData = this.LoadTileDrawData(spriteSheetName, col.Value, row.Value);
				}

				foreach (var layer in locations.Keys)
				{
					if (!tileMap.Layers.TryGetValue(layer, out var tileMapLayer))
					{
						tileMapLayer = new TileMapLayer(layer);
						tileMap.Layers.Add(tileMapLayer.Layer, tileMapLayer);
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
							var tile = new Tile(true, layer, position, area, collision, drawData);
							tileMapLayer.AddTile((int)tile.Position.Y / Tile.TILE_DIMENSIONS, (int)tile.Position.X / Tile.TILE_DIMENSIONS, tile);
						}
						else
						{
							var animatedTile = new AnimatedTile(true, true, layer, layer, position, area, collision, animation.CloneAnimation());
							tileMapLayer.AddTile((int)animatedTile.Position.Y / Tile.TILE_DIMENSIONS, (int)animatedTile.Position.X / Tile.TILE_DIMENSIONS, animatedTile);
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
			foreach (string characterSet in CharactersConfig.CharacterFileNames)
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
			//_ = new Entity(true, true, 1, 1, OrientationTypes.Downward, new MoveSpeed(25), position, area, collision, animations[0], animations, tileManager.ActiveTileMap.Layers[1]);
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

			if (drawManager.TryGetSpriteSheet(spriteSheetName, out var spritesheetTexture))
			{
				var animations = new Animation[4];
				var animationIndex = 0;
				for (var y = 0; y < spriteSheet.Bounds.Height - 1; y += 128) //TODO config entity height
				{
					var rowFrames = new DrawData[4];
					var framesIndex = 0;
					for (var x = 0; x < (64 * 4) - 1; x += 64) //TODO config entity width
					{
						var textureBox = new Rectangle(0, 0, 64, 128);
						var spritesheetCoordinate = new Point(x, y);
						var spritesheetBox = new Rectangle(spritesheetCoordinate.X, spritesheetCoordinate.Y, 64, 128);
						DrawData drawData;
						if (!Managers.DrawManager.DrawDataInstanceByName.TryGetValue(spriteSheetName + spritesheetBox.ToString(), out drawData))
						{
							var textureData = this.GetTextureDataFromSpritesheetArea(spritesheetTexture, new Rectangle(x, y, 64, 128));
							var texture = new Texture2D(Managers.Graphics.GraphicsDevice, 64, 128);
							texture.SetData(textureData);
							drawData = new DrawData(spriteSheetName, spritesheetCoordinate, textureBox, texture);
							Managers.DrawManager.DrawDataInstanceByName.Add(spriteSheetName + spritesheetBox.ToString(), drawData.CloneDrawData());
						}

						rowFrames[framesIndex++] = drawData;
					}

					animations[animationIndex++] = new Animation(false, 0, 200, rowFrames); //TODO config entity animations
				}

				return animations;
			}

			return null; //missing spritesheet 
		}
	}
}
