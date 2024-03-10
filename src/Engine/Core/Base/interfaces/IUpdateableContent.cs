using Microsoft.Xna.Framework;

namespace Engine.Core.Base.interfaces
{
    /// <summary>
    /// Represents updateable content.
    /// </summary>
    public interface IUpdateableContent : IContent
    {
        /// <summary>
        /// Gets or sets a value indicating whether updating is activated.
        /// </summary>
        bool UpdatingActivated { get; set; }

        /// <summary>
        /// Gets or sets the update order.
        /// </summary>
        ushort UpdateOrder { get; set; }

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        void Update(GameTime gameTime);
    }
}
