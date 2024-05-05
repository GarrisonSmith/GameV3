using DiscModels.Engine.Drawing;
using Engine.TileMapping.Base.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Drawing.Base
{
	/// <summary>
	/// Represents draw data.
	/// </summary>
	public class DrawData : IDisposable
	{
		/// <summary>
		/// Gets or sets the guid.
		/// </summary>
		public Guid Guid { get; private set; }

		/// <summary>
		/// Gets or sets the draw data name.
		/// </summary>
		public string DrawDataName { get => this.SpritesheetName + this.SpritesheetBox; }

		/// <summary>
		/// Gets or sets the spritesheet name.
		/// </summary>
		public string SpritesheetName { get; set; }

		/// <summary>
		/// Gets or sets the spritesheet coordinate.
		/// </summary>
		public Point SpritesheetCoordinate { get; set; }

		/// <summary>
		/// Gets or sets the texture box.
		/// </summary>
		public Rectangle TextureBox { get; set; }

		/// <summary>
		/// Gets the spritesheet box.
		/// </summary>
		public Rectangle SpritesheetBox { get => new(this.SpritesheetCoordinate.X, this.SpritesheetCoordinate.Y, this.TextureBox.Width, this.TextureBox.Height); }

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Initializes a new instance of the DrawData class.
		/// </summary>
		/// <param name="drawDataModel">The draw data model.</param>
		public DrawData(DrawDataModel drawDataModel)
		{
			this.Guid = Guid.NewGuid();
			this.SpritesheetName = drawDataModel.SpritesheetName;
			this.SpritesheetCoordinate = drawDataModel.SpritesheetBox.Location;
			this.TextureBox = new Rectangle(2, 2, Tile.TILE_DIMENSIONS, Tile.TILE_DIMENSIONS);
			this.Texture = Managers.LoadManager.GetTileTexture(this.SpritesheetName, this.SpritesheetBox, this.SpritesheetBox.Location);
			Managers.DrawManager.DrawData.Add(this.Guid, this);
		}

		/// <summary>
		/// Initializes a new instance of the DrawData class.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spritesheetCoordinate">The spritesheet coordinate.</param>
		/// <param name="textureBox">The texture box.</param>
		/// <param name="texture">The texture.</param>
		public DrawData(string spritesheetName, Point spritesheetCoordinate, Rectangle textureBox, Texture2D texture)
		{
			this.Guid = Guid.NewGuid();
			this.SpritesheetName = spritesheetName;
			this.SpritesheetCoordinate = spritesheetCoordinate;
			this.TextureBox = textureBox;
			this.Texture = texture;
			Managers.DrawManager.DrawData.Add(this.Guid, this);
		}

		/// <summary>
		/// Dispose of the draw data texture.
		/// </summary>
		public void Dispose()
		{ 
			this.Texture.Dispose();
			this.Texture = null;
			Managers.DrawManager.DrawData.Remove(this.Guid);
		}

		/// <summary>
		/// Gets a draw data model that corresponds to this draw data.
		/// </summary>
		/// <returns>The draw data model.</returns>
		public DrawDataModel ToDrawDataModel()
		{
			return new DrawDataModel
			{
				SpritesheetName = this.SpritesheetName,
				SpritesheetBox = this.SpritesheetBox,
			};
		}
	}
}
