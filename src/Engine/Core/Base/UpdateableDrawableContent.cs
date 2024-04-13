﻿using Engine.Core.Base.interfaces;
using Engine.Drawing.Base;
using Engine.Drawing.Base.interfaces;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Core.Base
{
	/// <summary>
	/// Represents updateable and drawable content. 
	/// </summary>
	public abstract class UpdateableDrawableContent : IUpdateableContent, IDrawableContent, ICanBeDrawn
    {
        private bool updatingActivated;
        private bool drawingActivated;
        private ushort updateOrder;
        private ushort drawOrder;

        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether updating is activated.
        /// </summary>
        public bool UpdatingActivated
        {
            get => this.updatingActivated;

            set
            {
                if (this.updatingActivated != value)
                {
					this.updatingActivated = value;
					Managers.ContentManager.ChangeUpdateActivation(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether updating is activated.
        /// </summary>
        public bool DrawingActivated
        {
            get => this.drawingActivated;

            set
            {
                if (this.drawingActivated != value)
                {
					this.drawingActivated = value;
					Managers.ContentManager.ChangeDrawActivation(this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the update order.
        /// </summary>
        public ushort UpdateOrder
        {
            get => this.updateOrder;

            set
            {
                if (this.updateOrder != value)
                {
					this.updateOrder = value;
					Managers.ContentManager.ChangeUpdateOrder(this.updateOrder, this);
                }
            }
        }

        /// <summary>
        /// Gets or sets the draw order.
        /// </summary>
        public ushort DrawOrder
        {
            get => this.drawOrder;

            set
            {
                if (this.drawOrder != value)
                {
					this.drawOrder = value;
					Managers.ContentManager.ChangeDrawOrder(this.drawOrder, this);
                }
            }
        }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; set => this.Area.Position = value; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Gets or sets the draw data.
		/// </summary>
		public DrawData DrawData { get; set; }

		/// <summary>
		/// Initializes a new instance of the UpdateableDrawableContent class.
		/// </summary>
		/// <param name="updatingActivated">A value indicating whether the content is updating.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="updateOrder">The update order.</param> 
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="area">The area.</param>
		/// <param name="drawData">The draw data.</param>
		protected UpdateableDrawableContent(bool updatingActivated, bool drawingActivated, ushort updateOrder, ushort drawOrder, IAmAArea area, DrawData drawData)
        {
			this.Guid = Guid.NewGuid();
            this.updatingActivated = updatingActivated;
            this.drawingActivated = drawingActivated;
            this.updateOrder = updateOrder;
            this.drawOrder = drawOrder;
			this.Area = area;
			this.DrawData = drawData;
			Managers.ContentManager.AddUpdateable(this);
			Managers.ContentManager.AddDrawable(this);
		}

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public abstract void Update(GameTime gameTime);

        /// <summary>
        /// Draws the content.
        /// </summary>
        public void Draw()
        {
			Managers.DrawManager.Draw(this.DrawData, this.Position);
        }
    }
}
