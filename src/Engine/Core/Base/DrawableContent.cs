using Engine.Core.Base.interfaces;
using Engine.Drawing.Base;
using Engine.Drawing.Base.interfaces;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using System;

namespace Engine.Core.Base
{
	/// <summary>
	/// Represents drawable content. 
	/// </summary>
	public abstract class DrawableContent: IDrawableContent, ICanBeDrawn
    {
        private bool drawingActivated;
        private ushort drawOrder;

        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        public Guid Guid { get; set; }

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
					Managers.ContentManager.ChangeDrawOrder(drawOrder, this);
                }
            }
        }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

        /// <summary>
        /// Gets or sets the draw data.
        /// </summary>
        public DrawData DrawData { get; set; }

		/// <summary>
		/// Initializes a new instance of the DrawableContent class.
		/// </summary>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
        /// <param name="position">The position.</param>
		/// <param name="area">The area.</param>
		/// <param name="drawData">The draw data.</param>
		public DrawableContent(bool drawingActivated, ushort drawOrder, Position position, IAmAArea area, DrawData drawData)
        {
			this.Guid = Guid.NewGuid();
            this.drawingActivated = drawingActivated;
            this.drawOrder = drawOrder;
            this.Position = position;
			this.Area = area;
			this.DrawData = drawData;
			Managers.ContentManager.AddDrawable(this);
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        public void Draw()
        {
			Managers.DrawManager.Draw(this.DrawData, this.Position);
        }
    }
}
