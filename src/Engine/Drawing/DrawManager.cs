using Engine.Drawing.Base;
using Engine.Loading.Base.interfaces;
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
		/// Gets or sets the draw data by name.
		/// </summary>
		public Dictionary<string, DrawData> DrawDataByName { get; set; }

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
			this.SpriteFonts = new();
			this.SpriteSheets = new();
			this.DrawData = new();
            this.DrawDataByName = new();
            this.Animations = new();
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
		/// Draws the texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Draw(Texture2D texture, Vector2 coordinates)
        {
            this.SpriteBatch.Draw(texture, coordinates, Color.White);
        }

		/// <summary>
		/// Draws the texture.
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		public void Draw(Texture2D texture, Vector2 coordinates, Color color)
		{
			this.SpriteBatch.Draw(texture, coordinates, color);
		}

		/// <summary>
		/// Draws the draw data.
		/// </summary>
		/// <param name="drawData">The draw data.</param>
		/// <param name="position">The position.</param>
		public void Draw(DrawData drawData, Position position)
        {
			this.SpriteBatch.Draw(drawData.SpriteSheet, position.Coordinates, drawData.SheetBox, Color.White);
        }

        /// <summary>
        /// Draws the draw data.
        /// </summary>
        /// <param name="drawData">The draw data.</param>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        public void Draw(DrawData drawData, Position position, Color color)
        {
			this.SpriteBatch.Draw(drawData.SpriteSheet, position.Coordinates, drawData.SheetBox, color);
        }

		/// <summary>
		/// Draws the animation.
		/// </summary>
		/// <param name="animation">The animation.</param>
		/// <param name="position">The position.</param>
		public void Draw(Animation animation, Position position)
		{
			this.Draw(animation.CurrentFrame, position, Color.White);
		}

		/// <summary>
		/// Draws the animation.
		/// </summary>
		/// <param name="animation">The animation.</param>
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		public void Draw(Animation animation, Position position, Color color)
		{
			this.Draw(animation.CurrentFrame, position, color);
		}

		/// <summary>
		/// Writes the text.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="text">The text.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Write(string fontName, string text, Vector2 coordinates)
		{
			if (this.SpriteFonts.TryGetValue(fontName, out var spriteFont))
			{
				this.SpriteBatch.DrawString(spriteFont, text, coordinates, Color.White);
			}
		}

		/// <summary>
		/// Writes the text.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="text">The text.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		public void Write(string fontName, string text, Vector2 coordinates, Color color)
		{
			if (this.SpriteFonts.TryGetValue(fontName, out var spriteFont))
			{
				this.SpriteBatch.DrawString(spriteFont, text, coordinates, color);
			}
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
			Managers.LoadManager.LoadDrawManager();
			this.IsLoaded = true;
		}
	}
}
