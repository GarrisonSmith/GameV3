using Engine.Core.Base.interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Core.Base
{
	/// <summary>
	/// Represents updateable content. 
	/// </summary>
	public abstract class UpdateableContent : IUpdateableContent
    {
        private bool updatingActivated;
        private ushort updateOrder;

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
        /// Initializes a new instance of the UpdateableContent class.
        /// </summary>
        /// <param name="updatingActivated">A value indicating whether the content is updating.</param>
        /// <param name="updateOrder">The update order.</param>
        public UpdateableContent(bool updatingActivated, ushort updateOrder)
        {
			this.Guid = Guid.NewGuid();
            this.updatingActivated = updatingActivated;
            this.updateOrder = updateOrder;
			Managers.ContentManager.AddUpdateable(this);
        }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public abstract void Update(GameTime gameTime);
    }
}
