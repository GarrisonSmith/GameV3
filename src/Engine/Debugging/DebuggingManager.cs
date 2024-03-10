using Engine.Entities;
using Engine.Loading.Base.interfaces;
using Engine.Physics.Areas;
using Engine.Physics.Collisions.interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Debugging
{
	/// <summary>
	/// Represents the debugging manager.
	/// </summary>
	public class DebuggingManager : DrawableGameComponent, ICanBeLoaded
	{
		/// <summary>
		/// Start the debugging manager.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The debugger.</returns>
		public static DebuggingManager StartDebugger(Game game)
		{
			return Managers.DebuggingManager ?? new DebuggingManager(game);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the debugger is enabled.
		/// </summary>
		public bool DebuggerEnabled { get; set; }

		/// <summary>
		/// Gets or sets the collision textures.
		/// </summary>
		protected Dictionary<IHaveCollision, Texture2D> CollisionTextures;

		/// <summary>
		/// Initializes a new instance of the DebuggingManager class.
		/// </summary>
		/// <param name="game">The game.</param>
		public DebuggingManager(Game game) : base(game)
		{
			this.CollisionTextures = new();
		}

		/// <summary>
		/// Initializes the content.
		/// </summary>
		public override void Initialize()
		{

		}

		/// <summary>
		/// Updates the content.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			if (this.DebuggerEnabled)
			{
				var entityManger = Managers.EntityManager;
				var controlledEntity = entityManger.ControlledEntity;

				if (controlledEntity != null)
				{
					this.GetCollisionTexture(controlledEntity.Entity);

					foreach (var rowDictionary in controlledEntity.Entity.TileMapLayer.Tiles.Values)
					{
						foreach (var tile in rowDictionary.Values)
						{
							this.GetCollisionTexture(tile);
						}
					}
				}
			}
		}

		/// <summary>
		/// Draws the content.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Draw(GameTime gameTime)
		{
			if (this.DebuggerEnabled)
			{
				foreach (var keyValuePair in this.CollisionTextures)
				{
					Managers.DrawManager.Draw(keyValuePair.Value, keyValuePair.Key.CollisionArea.Area.TopLeft);
				}
			}
		}

		/// <summary>
		/// Gets the collision texture.
		/// </summary>
		/// <param name="collision">The collision.</param>
		private void GetCollisionTexture(IHaveCollision collision)
		{
			if (collision.CollisionArea == null)
			{
				return;
			}

			if (collision.CollisionArea.Area is SimpleArea simpleArea)
			{
				this.TryAddRectangleTexture(
					collision,
					new Rectangle(0, 0, (int)simpleArea.Width, (int)simpleArea.Height)
				);
			}
			else if (collision.CollisionArea.Area is OffsetArea offsetArea)
			{
				this.TryAddRectangleTexture(
					collision,
					new Rectangle(0, 0, (int)offsetArea.Width, (int)offsetArea.Height)
				);
			}
			else if (collision.CollisionArea.Area is ComplexArea complexArea)
			{
				foreach (var area in complexArea.SubAreas)
				{
					this.TryAddRectangleTexture(
						collision,
						new Rectangle(0, 0, (int)area.Width, (int)area.Height)
					);
				}
			}
		}

		/// <summary>
		/// Tries to add the rectangle texture.
		/// </summary>
		/// <param name="rectangle"></param>
		private bool TryAddRectangleTexture(IHaveCollision collision, Rectangle rectangle)
		{
			if (this.CollisionTextures.ContainsKey(collision))
			{
				return false;
			}

			Texture2D newTexture = new Texture2D(GraphicsDevice, rectangle.Width, rectangle.Height); ;
			Color[] data = new Color[newTexture.Width * newTexture.Height];

			for (int i = 0; i < data.Length; i++)
			{
				data[i] = Color.Transparent;
			}

			for (int x = 0; x < newTexture.Width; x++)
			{
				for (int y = 0; y < newTexture.Height; y++)
				{
					if (x < 1 || x >= newTexture.Width - 1 ||
						y < 1 || y >= newTexture.Height - 1)
					{
						data[x + y * newTexture.Width] = Color.Purple;
					}
				}
			}

			newTexture.SetData(data);
			return this.CollisionTextures.TryAdd(collision, newTexture);
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
