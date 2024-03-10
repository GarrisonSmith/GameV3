using Engine.Loading.Base.interfaces;
using Engine.Physics.Base;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Physics
{
	/// <summary>
	/// Represents a physics manager.
	/// </summary>
	public class PhysicsManager : GameComponent, ICanBeLoaded
	{
		/// <summary>
		/// Start the physics manager.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The physics manager.</returns>
		public static PhysicsManager StartPhysicsManager(Game game)
		{
			return Managers.PhysicsManager ?? new PhysicsManager(game);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the move speeds.
		/// </summary>
		public List<MoveSpeed> MoveSpeeds { get; set; }

		/// <summary>
		/// Initializes a new instance of the PhysicsManager class.
		/// </summary>
		/// <param name="game">The game.</param>
		private PhysicsManager(Game game) : base(game)
		{
			this.MoveSpeeds = new List<MoveSpeed>();
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
			this.MoveSpeeds.ForEach(x => x.Update(gameTime));
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
