using Engine.Core.Base.interfaces;
using Engine.Loading.Base.interfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Core
{
	/// <summary>
	/// Represents a content manager.
	/// </summary>
	public class ContentManager : DrawableGameComponent, ICanBeLoaded
	{
        /// <summary>
        /// Starts the content manager.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns>The content manager.</returns>
        public static ContentManager StartContentManager(Game game)
        {
            return Managers.ContentManager ?? new ContentManager(game);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the activated Updateables. Where the key is the update order and 
		/// the value is another dictionary with the key being the content guid.
		/// </summary>
		protected SortedDictionary<ushort, Dictionary<Guid, IUpdateableContent>> ActivatedUpdateables { get; set; }

        /// <summary>
        /// Gets or sets the activated Drawables. Where the key is the draw order and 
        /// the value is another dictionary with the key being the content guid.
        /// </summary>
        protected SortedDictionary<ushort, Dictionary<Guid, IDrawableContent>> ActivatedDrawables { get; set; }

        /// <summary>
        /// Gets or sets the activated overlay Drawables. Where the key is the draw order and
        /// the value is another dictionary with the key being the content guid.
        /// </summary>
        protected SortedDictionary<ushort, Dictionary<Guid, IDrawableContent>> ActivatedOverlayDrawables { get; set; }

        /// <summary>
        /// Initializes a new instance of the ContentManager class.
        /// </summary>
        /// <param name="game">The game.</param>
        private ContentManager(Game game) : base(game)
        {

        }

        /// <summary>
        /// Initializes the content.
        /// </summary>
        public override void Initialize()
        {
			this.ActivatedUpdateables = new();
			this.ActivatedDrawables = new();
            this.ActivatedOverlayDrawables = new();
		}

        /// <summary>
        /// Updates the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (var keyValuePair in this.ActivatedUpdateables)
            {
                foreach (var updateableContent in keyValuePair.Value.Values)
                {
                    updateableContent.Update(gameTime);
                }
            }
        }

        /// <summary>
        /// Draws the content.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var keyValuePair in this.ActivatedDrawables)
            {
                foreach (var drawableContent in keyValuePair.Value.Values)
                {
                    drawableContent.Draw();
                }
            }
        }

		/// <summary>
		/// Draws the content.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public void DrawOverlay(GameTime gameTime)
		{
			foreach (var keyValuePair in this.ActivatedOverlayDrawables)
			{
				foreach (var drawableContent in keyValuePair.Value.Values)
				{
					drawableContent.Draw();
				}
			}
		}

		/// <summary>
		/// Adds the updateable.
		/// </summary>
		/// <param name="content">The updateable content.</param>
		public void AddUpdateable(IUpdateableContent content)
        {
            if (!content.UpdatingActivated)
            {
                return;
            }

            if (this.ActivatedUpdateables.TryGetValue(content.UpdateOrder, out var nestedDictionary))
            {
                nestedDictionary.Add(content.Guid, content);
            }
            else
            {
                nestedDictionary = new Dictionary<Guid, IUpdateableContent>
                {
                    { content.Guid, content }
                };

				this.ActivatedUpdateables.Add(content.UpdateOrder, nestedDictionary);
            }
        }

        /// <summary>
        /// Adds the drawable.
        /// </summary>
        /// <param name="content">The drawable content.</param>
        public void AddDrawable(IDrawableContent content)
        {
            if (!content.DrawingActivated)
            {
                return;
            }

            if (this.ActivatedDrawables.TryGetValue(content.DrawOrder, out var nestedDictionary))
            {
                nestedDictionary.Add(content.Guid, content);
            }
            else
            {
                nestedDictionary = new Dictionary<Guid, IDrawableContent>
                {
                    { content.Guid, content }
                };

				this.ActivatedDrawables.Add(content.DrawOrder, nestedDictionary);
            }
        }

		/// <summary>
		/// Adds the overlay drawable.
		/// </summary>
		/// <param name="content">The overlay drawable content.</param>
		public void AddOverlayDrawable(IDrawableContent content)
		{
			if (!content.DrawingActivated)
			{
				return;
			}

			if (this.ActivatedOverlayDrawables.TryGetValue(content.DrawOrder, out var nestedDictionary))
			{
				nestedDictionary.Add(content.Guid, content);
			}
			else
			{
				nestedDictionary = new Dictionary<Guid, IDrawableContent>
				{
					{ content.Guid, content }
				};

				this.ActivatedOverlayDrawables.Add(content.DrawOrder, nestedDictionary);
			}
		}

		/// <summary>
		/// Changes the update activation of the updateable.
		/// </summary>
		/// <param name="content">The content.</param>
		public void ChangeUpdateActivation(IUpdateableContent content)
        {
            if (content.UpdatingActivated)
            {
                if (this.ActivatedUpdateables.TryGetValue(content.UpdateOrder, out var nestedDictionary))
                {
                    nestedDictionary.Add(content.Guid, content);
                }
                else
                {
                    nestedDictionary = new Dictionary<Guid, IUpdateableContent>
                    {
                        { content.Guid, content }
                    };

					this.ActivatedUpdateables.Add(content.UpdateOrder, nestedDictionary);
                }
            }
            else
            {
                if (!this.ActivatedUpdateables.TryGetValue(content.UpdateOrder, out var nestedDictionary))
                {
                    Console.WriteLine("Update order dictionary not found");
                }

                if (nestedDictionary == null || !nestedDictionary.Remove(content.Guid))
                {
                    Console.WriteLine("Content not found in nested update dictionary");
                }

                if (nestedDictionary != null && nestedDictionary.Count == 0)
                {
					this.ActivatedUpdateables.Remove(content.UpdateOrder);
                }
            }
        }

        /// <summary>
        /// Changes the draw activation of the drawable.
        /// </summary>
        /// <param name="content">The content.</param>
        public void ChangeDrawActivation(IDrawableContent content)
        {
            if (content.DrawingActivated)
            {
                if (this.ActivatedDrawables.TryGetValue(content.DrawOrder, out var nestedDictionary))
                {
                    nestedDictionary.Add(content.Guid, content);
                }
                else
                {
                    nestedDictionary = new Dictionary<Guid, IDrawableContent>
                    {
                        { content.Guid, content }
                    };

					this.ActivatedDrawables.Add(content.DrawOrder, nestedDictionary);
                }
            }
            else
            {
                if (!this.ActivatedDrawables.TryGetValue(content.DrawOrder, out var nestedDictionary))
                {
                    Console.WriteLine("Update order dictionary not found");
                }

                if (nestedDictionary == null || !nestedDictionary.Remove(content.Guid))
                {
                    Console.WriteLine("Content not found in nested update dictionary");
                }

                if (nestedDictionary != null && nestedDictionary.Count == 0)
                {
					this.ActivatedDrawables.Remove(content.DrawOrder);
                }
            }
        }

		/// <summary>
		/// Changes the draw activation of the overlay drawable.
		/// </summary>
		/// <param name="content">The content.</param>
		public void ChangeOverlayDrawActivation(IDrawableContent content)
		{
			if (content.DrawingActivated)
			{
				if (this.ActivatedOverlayDrawables.TryGetValue(content.DrawOrder, out var nestedDictionary))
				{
					nestedDictionary.Add(content.Guid, content);
				}
				else
				{
					nestedDictionary = new Dictionary<Guid, IDrawableContent>
					{
						{ content.Guid, content }
					};

					this.ActivatedOverlayDrawables.Add(content.DrawOrder, nestedDictionary);
				}
			}
			else
			{
				if (!this.ActivatedOverlayDrawables.TryGetValue(content.DrawOrder, out var nestedDictionary))
				{
					Console.WriteLine("Update order dictionary not found");
				}

				if (nestedDictionary == null || !nestedDictionary.Remove(content.Guid))
				{
					Console.WriteLine("Content not found in nested update dictionary");
				}

				if (nestedDictionary != null && nestedDictionary.Count == 0)
				{
					this.ActivatedOverlayDrawables.Remove(content.DrawOrder);
				}
			}
		}

		/// <summary>
		/// Changes the content update order.
		/// </summary>
		/// <param name="oldUpdateOrder">The old update order.</param>
		/// <param name="content">The content.</param>
		public void ChangeUpdateOrder(ushort oldUpdateOrder, IUpdateableContent content)
        {
            if (!content.UpdatingActivated)
            {
                return;
            }

            if (!this.ActivatedUpdateables.TryGetValue(oldUpdateOrder, out var nestedOriginalDictionary))
            {
                Console.WriteLine("Update order dictionary not found");
            }

            if (nestedOriginalDictionary == null || !nestedOriginalDictionary.Remove(content.Guid))
            {
                Console.WriteLine("Content not found in nested update dictionary");
            }

            if (nestedOriginalDictionary == null || nestedOriginalDictionary.Count == 0)
            {
				this.ActivatedUpdateables.Remove(oldUpdateOrder);
            }

            if (this.ActivatedUpdateables.TryGetValue(content.UpdateOrder, out var nestedDestinationDictionary))
            {
                nestedDestinationDictionary.Add(content.Guid, content);
            }
            else
            {
                nestedDestinationDictionary = new Dictionary<Guid, IUpdateableContent>
                {
                    { content.Guid, content }
                };

				this.ActivatedUpdateables.Add(content.UpdateOrder, nestedDestinationDictionary);
            }
        }

        /// <summary>
        /// Changes the content draw order.
        /// </summary>
        /// <param name="oldDrawOrder">The old draw order.</param>
        /// <param name="content">The content.</param>
        public void ChangeDrawOrder(ushort oldDrawOrder, IDrawableContent content)
        {
            if (!content.DrawingActivated)
            {
                return;
            }

            if (!this.ActivatedDrawables.TryGetValue(oldDrawOrder, out var nestedOriginalDictionary))
            {
                Console.WriteLine("Draw order dictionary not found");
            }

            if (nestedOriginalDictionary == null || !nestedOriginalDictionary.Remove(content.Guid))
            {
                Console.WriteLine("Content not found in nested draw dictionary");
            }

            if (nestedOriginalDictionary == null || nestedOriginalDictionary.Count == 0)
            {
				this.ActivatedDrawables.Remove(oldDrawOrder);
            }

            if (this.ActivatedDrawables.TryGetValue(content.DrawOrder, out var nestedDestinationDictionary))
            {
                nestedDestinationDictionary.Add(content.Guid, content);
            }
            else
            {
                nestedDestinationDictionary = new Dictionary<Guid, IDrawableContent>
                {
                    { content.Guid, content }
                };

				this.ActivatedDrawables.Add(content.DrawOrder, nestedDestinationDictionary);
            }
        }

		/// <summary>
		/// Changes the content overlay draw order.
		/// </summary>
		/// <param name="oldDrawOrder">The old draw order.</param>
		/// <param name="content">The content.</param>
		public void ChangeOverlayDrawOrder(ushort oldDrawOrder, IDrawableContent content)
		{
			if (!content.DrawingActivated)
			{
				return;
			}

			if (!this.ActivatedOverlayDrawables.TryGetValue(oldDrawOrder, out var nestedOriginalDictionary))
			{
				Console.WriteLine("Draw order dictionary not found");
			}

			if (nestedOriginalDictionary == null || !nestedOriginalDictionary.Remove(content.Guid))
			{
				Console.WriteLine("Content not found in nested draw dictionary");
			}

			if (nestedOriginalDictionary == null || nestedOriginalDictionary.Count == 0)
			{
				this.ActivatedOverlayDrawables.Remove(oldDrawOrder);
			}

			if (this.ActivatedOverlayDrawables.TryGetValue(content.DrawOrder, out var nestedDestinationDictionary))
			{
				nestedDestinationDictionary.Add(content.Guid, content);
			}
			else
			{
				nestedDestinationDictionary = new Dictionary<Guid, IDrawableContent>
				{
					{ content.Guid, content }
				};

				this.ActivatedOverlayDrawables.Add(content.DrawOrder, nestedDestinationDictionary);
			}
		}

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			this.IsLoaded = true;
		}
	}
}
