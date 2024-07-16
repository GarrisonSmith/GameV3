using Engine.Drawing.Base;
using Engine.Loading.Base.interfaces;
using Engine.Loading.Configurations;
using Engine.Physics.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Engine.Drawing
{
	/// <summary>
	/// Represents a draw manager.
	/// </summary>
	public class DrawManager : ICanBeLoaded
	{
		/// <summary>
		/// Start the draw manager.
		/// </summary>
		/// <param name="graphicDevice">The graphic device.</param>
		/// <returns>The draw manager.</returns>
		public static DrawManager StartDrawManager(GraphicsDevice graphicDevice)
		{
			return Managers.DrawManager ?? new DrawManager(graphicDevice);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the spritebatch.
		/// </summary>
		public SpriteBatch SpriteBatch { get; set; }

		/// <summary>
		/// Gets or sets the sprite fonts.
		/// </summary>
		public Dictionary<string, SpriteFont> SpriteFonts { get; set; }

		/// <summary>
		/// Gets or sets the sprite sheets.
		/// </summary>
		public Dictionary<string, Texture2D> SpriteSheets { get; set; }

		/// <summary>
		/// Gets or sets the draw data.
		/// </summary>
		public Dictionary<Guid, DrawData> DrawData { get; set; }

		/// <summary>
		/// Gets or sets a instance to the texture by name.
		/// </summary>
		public Dictionary<string, Texture2D> TextureByName { get; set; }

		/// <summary>
		/// Gets or sets the animations.
		/// </summary>
		public Dictionary<Guid, Animation> Animations { get; set; }

		/// <summary>
		/// Initializes a new instance of the DrawManager class.
		/// </summary>
		/// <param name="graphicDevice">The graphics device.</param>
		private DrawManager(GraphicsDevice graphicDevice)
		{
			this.SpriteBatch = new SpriteBatch(graphicDevice);
			this.SpriteFonts = new Dictionary<string, SpriteFont>();
			this.SpriteSheets = new Dictionary<string, Texture2D>();
			this.DrawData = new Dictionary<Guid, DrawData>();
			this.TextureByName = new Dictionary<string, Texture2D>();
			this.Animations = new Dictionary<Guid, Animation>();
			this.IsLoaded = false;
		}

		/// <summary>
		/// Begins the draw.
		/// </summary>
		public void Begin()
		{
			this.SpriteBatch.Begin(
				SpriteSortMode.Deferred, //first things drawn on bottom, last things on top
				BlendState.AlphaBlend);
		}

		/// <summary>
		/// Begins the spritebatch drawing.
		/// </summary>
		/// <param name="transformMatrix">The matrix.</param>
		public void Begin(Effect effect, Matrix transformMatrix)
		{
			//Matrix projectionMatrix = Matrix.CreateOrthographicOffCenter(0, 1500, 1000, 0, 0, 1);
			//effect.Parameters["worldViewProjection"].SetValue(transformMatrix * projectionMatrix);

			this.SpriteBatch.Begin(
				SpriteSortMode.Deferred, //first things drawn on bottom, last things on top
				BlendState.AlphaBlend,
				SamplerState.PointWrap,
				null,
				null,
				null, //effect,
				transformMatrix);
		}

		/// <summary>
		/// Ends the draw.
		/// </summary>
		public void End()
		{
			this.SpriteBatch.End();
		}

		/// <summary>
		/// Draws the area of the texture at origin.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="area">The area.</param>
		public void Draw(Texture2D texture, Rectangle area)
		{
			this.SpriteBatch.Draw(texture, new Vector2(0, 0), area, Color.White);
		}

		/// <summary>
		/// Draws the texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Draw(Texture2D texture, Vector2 coordinates)
		{
			this.SpriteBatch.Draw(texture, coordinates, Color.White);
		}

		/// <summary>
		/// Draws the draw data.
		/// </summary>
		/// <param name="drawData">The draw data.</param>
		/// <param name="position">The position.</param>
		public void Draw(DrawData drawData, Position position)
		{
			this.SpriteBatch.Draw(drawData.Texture, position.Coordinates, drawData.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the animation.
		/// </summary>
		/// <param name="animation">The animation.</param>
		/// <param name="position">The position.</param>
		public void Draw(Animation animation, Position position)
		{
			this.Draw(animation.CurrentFrame, position);
		}

		/// <summary>
		/// Writes the text.
		/// </summary>
		/// <param name="spriteFont">The sprite font.</param>
		/// <param name="text">The text.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		public void Write(SpriteFont spriteFont, string text, Vector2 coordinates, Color color)
		{
			this.SpriteBatch.DrawString(spriteFont, text, coordinates, color);
		}

		/// <summary>
		/// Clones the texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <returns>The texture.</returns>
		public Texture2D CloneTexture(Texture2D texture)
		{
			var clonedTexture = new Texture2D(Managers.Graphics.GraphicsDevice, texture.Width, texture.Height);
			var data = new Color[texture.Width * texture.Height];
			texture.GetData(data);
			clonedTexture.SetData(data);

			return clonedTexture;
		}

		/// <summary>
		/// Tries to get the sprite sheet.
		/// </summary>
		/// <param name="spriteSheetName">sprite sheet name.</param>
		/// <param name="texture">The texture.</param>
		/// <returns>A value indicating whether the sprite sheet was found.</returns>
		public bool TryGetSpriteSheet(string spriteSheetName, out Texture2D texture)
		{
			return this.SpriteSheets.TryGetValue(spriteSheetName, out texture);
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

			foreach (var spriteSheetName in TileSetsConfig.TileSetFileNames)
			{
				Managers.DrawManager.SpriteSheets.TryAdd(spriteSheetName, Managers.Game.Content.Load<Texture2D>(@"TileSets\" + spriteSheetName));
			}

			foreach (var spriteSheetName in CharactersConfig.CharacterFileNames)
			{
				Managers.DrawManager.SpriteSheets.TryAdd(spriteSheetName, Managers.Game.Content.Load<Texture2D>(@"CharacterSets\" + spriteSheetName));
			}

			foreach (var fontName in FontsConfig.FontFileNames)
			{
				Managers.DrawManager.SpriteFonts.TryAdd(fontName, Managers.Game.Content.Load<SpriteFont>(@"Fonts\" + fontName));
			}

			this.IsLoaded = true;
		}
	}
}
