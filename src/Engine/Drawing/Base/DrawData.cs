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
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sheet box.
        /// </summary>
        public Rectangle SheetBox { get; set; }

        /// <summary>
        /// Gets or sets the spritesheet.
        /// </summary>
        public Texture2D SpriteSheet { get; set; }

        /// <summary>
        /// Initializes a new instance of the DrawData class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="sheetBox">The sheet box.</param>
        /// <param name="spriteSheet">The sprite sheet.</param>
        public DrawData(string name, Rectangle sheetBox, Texture2D spriteSheet)
        {
            this.Guid = Guid.NewGuid();
            this.Name = name;
			this.SheetBox = sheetBox;
			this.SpriteSheet = spriteSheet;
			Managers.DrawManager.DrawData.Add(this.Guid, this);
			Managers.DrawManager.DrawDataByName.Add(this.Name, this);
        }
    }
}
