using DiscModels.Engine.Entities;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping;
using Engine.Entities.Base;
using Engine.Physics.Base;
using Engine.TileMapping.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
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
		/// <returns>The load manager.</returns>
		public static LoadManager StartLoadManager()
		{
			return Managers.LoadManager ?? new LoadManager();
		}

		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <param name="spriteSheetName">The sprite sheet name.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <param name="spritesheetCoordinate">The spritesheet coordinate.</param>
		/// <returns>The texture/</returns>
		public Texture2D GetTexture(string spriteSheetName, Rectangle spritesheetBox, Point spritesheetCoordinate)
		{
			if (Managers.DrawManager.TryGetSpriteSheet(spriteSheetName, out var spritesheet))
			{
				if (Managers.DrawManager.TextureByName.TryGetValue(spriteSheetName + spritesheetBox.ToString(), out var texture))
				{
					return Managers.DrawManager.CloneTexture(texture);
				}

				var tileTextureData = this.GetColorDataFromTextureArea(spritesheet, new Rectangle(spritesheetCoordinate.X, spritesheetCoordinate.Y, spritesheetBox.Width, spritesheetBox.Height));
				var leftTextureData = new Color[spritesheetBox.Height * 2];
				var rightTextureData = new Color[spritesheetBox.Height * 2];
				var topTextureData = new Color[spritesheetBox.Width * 2];
				var bottomTextureData = new Color[spritesheetBox.Width * 2];

				for (int i = 0; i < spritesheetBox.Width; i++)
				{
					//top of texture
					var topIndex = i;
					topTextureData[i] = topTextureData[i + spritesheetBox.Width] = tileTextureData[topIndex];

					//bottom of texture
					var bottomIndex = tileTextureData.Length - spritesheetBox.Width + i;
					bottomTextureData[i] = bottomTextureData[i + spritesheetBox.Width] = tileTextureData[bottomIndex];
				}

				for (int i = 0; i < spritesheetBox.Height; i++)
				{
					//left of texture
					var leftIndex = i * spritesheetBox.Width;
					leftTextureData[i * 2] = leftTextureData[i * 2 + 1] = tileTextureData[leftIndex];

					//right of texture
					var rightIndex = leftIndex + spritesheetBox.Width - 1;
					rightTextureData[i * 2] = rightTextureData[i * 2 + 1] = tileTextureData[rightIndex];
				}

				var leftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, spritesheetBox.Height);
				leftTexture.SetData(leftTextureData);

				var rightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, spritesheetBox.Height);
				rightTexture.SetData(rightTextureData);

				var topTexture = new Texture2D(Managers.Graphics.GraphicsDevice, spritesheetBox.Width, 2);
				topTexture.SetData(topTextureData);

				var bottomTexture = new Texture2D(Managers.Graphics.GraphicsDevice, spritesheetBox.Width, 2);
				bottomTexture.SetData(bottomTextureData);

				var topLeftPixel = tileTextureData[0];
				var topLeftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				topLeftTexture.SetData(new Color[] { topLeftPixel, topLeftPixel, topLeftPixel, topLeftPixel });

				var topRightPixel = tileTextureData[spritesheetBox.Width];
				var topRightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				topRightTexture.SetData(new Color[] { topRightPixel, topRightPixel, topRightPixel, topRightPixel });

				var bottomLeftPixel = tileTextureData[tileTextureData.Length - spritesheetBox.Width];
				var bottomLeftTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				bottomLeftTexture.SetData(new Color[] { bottomLeftPixel, bottomLeftPixel, bottomLeftPixel, bottomLeftPixel });

				var bottomRightPixel = tileTextureData[tileTextureData.Length - 1];
				var bottomRightTexture = new Texture2D(Managers.Graphics.GraphicsDevice, 2, 2);
				bottomRightTexture.SetData(new Color[] { bottomRightPixel, bottomRightPixel, bottomRightPixel, bottomRightPixel });

				var middleTexture = new Texture2D(Managers.Graphics.GraphicsDevice, spritesheetBox.Width, spritesheetBox.Height);
				middleTexture.SetData(tileTextureData);

				var textures = new Texture2D[][]
				{
					new Texture2D[] { topLeftTexture, topTexture, topRightTexture },
					new Texture2D[] { leftTexture, middleTexture, rightTexture },
					new Texture2D[] { bottomLeftTexture, bottomTexture, bottomRightTexture }
				};

				var tileTexture = this.CombineTexture(textures, spritesheetBox.Width + 4, spritesheetBox.Height + 4);

				leftTexture.Dispose();
				rightTexture.Dispose();
				topTexture.Dispose();
				bottomTexture.Dispose();
				topLeftTexture.Dispose();
				topRightTexture.Dispose();
				bottomLeftTexture.Dispose();
				bottomRightTexture.Dispose();
				middleTexture.Dispose();

				var tileTextureClone = Managers.DrawManager.CloneTexture(tileTexture);
				Managers.DrawManager.TextureByName.Add(spriteSheetName + spritesheetBox.ToString(), tileTextureClone);

				return tileTexture;
			}

			return null; //missing spritesheet.
		}

		/// <summary>
		/// Loads the tile map.
		/// </summary>
		/// <param name="tileMapName">The tile map name.</param>
		public void LoadTileMap(string tileMapName)
		{
			if (Managers.TileManager.TileMapIsLoad(tileMapName))
			{
				return;
			}

			var tileMapPath = Path.Combine(Managers.Game.Content.RootDirectory, "TileMaps", tileMapName + ".json");
			var tileMapJson = File.ReadAllText(tileMapPath);
			var tileMapModel = this.DeserializeFromJson<TileMapModel>(tileMapJson);
			_ = new TileMap(tileMapModel);
		}

		/// <summary>
		/// Loads the entity.
		/// </summary>
		/// <param name="entityName">The entity name.</param>
		public void LoadEntity(string entityName)
		{
			var entityPath = Path.Combine(Managers.Game.Content.RootDirectory, "Entities", entityName + ".json");
			var entityJson = File.ReadAllText(entityPath);
			var entityModel = this.DeserializeFromJson<EntityModel<IAmAAreaModel, IAmACollisionAreaModel>>(entityJson);

			var position = new Position(entityModel.Position); //TODO entity starting location
			var area = Managers.PhysicsManager.GetArea(position, entityModel.Area);
			var collision = Managers.PhysicsManager.GetCollisionArea(position, entityModel.CollisionArea);

			_ = new Entity(true, true, 2, 2, position, area, collision, entityModel);
		}

		/// <summary>
		/// Deserializes the JSON to a tile map.
		/// </summary>
		/// <param name="json">The JSON.</param>
		/// <returns>The tile map model.</returns>
		private static T DeserializeFromJson<T>(string json)
		{
			using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
			var serializer = new DataContractJsonSerializer(typeof(T), DiscModels.DiskTypes.GetDiscTypes());
			return (T)serializer.ReadObject(stream);
		}

		/// <summary>
		/// Gets the color data from the area of the texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="area">The area.</param>
		/// <returns>The color data for the area of the texture.</returns>
		private static Color[] GetColorDataFromTextureArea(Texture2D texture, Rectangle area)
		{
			var renderTarget = new RenderTarget2D(Managers.Graphics.GraphicsDevice, area.Width, area.Height, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
			Managers.Graphics.GraphicsDevice.SetRenderTarget(renderTarget);

			Managers.DrawManager.Begin();

			Managers.DrawManager.Draw(texture, area);

			Managers.DrawManager.End();

			Managers.Graphics.GraphicsDevice.SetRenderTarget(null);
			Color[] data = new Color[renderTarget.Width * renderTarget.Height];
			renderTarget.GetData(data);
			renderTarget.Dispose();

			return data;
		}

		/// <summary>
		/// Combines the textures.
		/// </summary>
		/// <param name="textures">The textures.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The combined texture.</returns>
		private static Texture2D CombineTexture(Texture2D[][] textures, int width, int height)
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
					var coordinates = new Vector2(horizontalOffset, verticalOffset);
					Managers.DrawManager.Draw(texture, coordinates);
					horizontalOffset += texture.Width;
				}

				verticalOffset += textureRows.Select(x => x.Height).OrderByDescending(x => x).FirstOrDefault();
			}

			Managers.DrawManager.End();

			Managers.Graphics.GraphicsDevice.SetRenderTarget(null);
			Color[] data = new Color[renderTarget.Width * renderTarget.Height];
			renderTarget.GetData(data);
			renderTarget.Dispose();
			var combinedTexture = new Texture2D(Managers.Graphics.GraphicsDevice, renderTarget.Width, renderTarget.Height);
			combinedTexture.SetData(data);

			return combinedTexture;
		}
	}
}
