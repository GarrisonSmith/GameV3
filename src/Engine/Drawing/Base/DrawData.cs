using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Drawing.Base
{
    /// <summary>
    /// Represents draw data.
    /// </summary>
    public class DrawData
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the draw data name.
        /// </summary>
        public string DrawDataName { get => this.SpritesheetName + this.SpritesheetCoordinate; }

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
        /// Gets or sets the texture.
        /// </summary>
        public Texture2D Texture { get; set; }

		/// <summary>
		/// Initializes a new instance of the DrawData class.
		/// </summary>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <param name="spritesheetCoordinate">The spritesheet coordinate.</param>
		/// <param name="sheetBox">The sheet box.</param>
		/// <param name="texture">The texture.</param>
		public DrawData(string spritesheetName, Point spritesheetCoordinate, Rectangle sheetBox, Texture2D texture)
        {
            this.Guid = Guid.NewGuid();
            this.SpritesheetName = spritesheetName;
            this.SpritesheetCoordinate = spritesheetCoordinate;
			this.TextureBox = sheetBox;
			this.Texture = texture;
			Managers.DrawManager.DrawData.Add(this.Guid, this);
        }
    }
}
