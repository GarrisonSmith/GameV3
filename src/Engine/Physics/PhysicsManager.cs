using DiscModels.Engine.Physics.Areas;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions;
using DiscModels.Engine.Physics.Collisions.interfaces;
using Engine.Loading.Base.interfaces;
using Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions;
using Engine.Physics.Collisions.interfaces;
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

		/// <summary>
		/// Gets the area.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="areaModel">The area model.</param>
		/// <returns>The area.</returns>
		public IAmAArea GetArea(Position position, IAmAAreaModel areaModel)
		{
			switch (areaModel)
			{
				case SimpleAreaModel simpleAreaModel:
					return new SimpleArea(position, simpleAreaModel);
				case OffsetAreaModel offSetAreaModel:
					return new OffsetArea(position, offSetAreaModel);
				case AreaCollectionModel areaCollectionModel:
					return new AreaCollection(position, areaCollectionModel);
			}

			return null;
		}

		/// <summary>
		/// Gets the collision area.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="collisionAreaModel">The collision area model.</param>
		/// <returns>The collision area.</returns>
		public IAmACollisionArea GetCollisionArea(Position position, IAmACollisionAreaModel collisionAreaModel)
		{
			switch (collisionAreaModel)
			{
				case SimpleCollisionAreaModel simpleCollisionAreaModel:
					return new SimpleCollisionArea(position, simpleCollisionAreaModel);
				case OffsetCollisionAreaModel offsetCollisionAreaModel:
					return new OffsetCollisionArea(position, offsetCollisionAreaModel);
				case CollisionAreaCollectionModel collisionAreaCollectionModel:
					return new CollisionAreaCollection(position, collisionAreaCollectionModel);
			}

			return null;
		}

		/// <summary>
		/// Gets the area model.
		/// </summary>
		/// <param name="area">The area.</param>
		/// <returns>The area model.</returns>
		public IAmAAreaModel GetAreaModel(IAmAArea area)
		{
			switch (area)
			{
				case SimpleArea simpleArea:
					return simpleArea.ToModel();
				case OffsetArea offSetArea:
					return offSetArea.ToModel();
				case AreaCollection areaCollection:
					return areaCollection.ToModel();
			}

			return null;
		}

		/// <summary>
		/// Gets the collision area model.
		/// </summary>
		/// <param name="collision">The collision.</param>
		/// <returns>The collision area model.</returns>
		public IAmACollisionAreaModel GetCollisionAreaModel(IAmACollisionArea collision)
		{
			switch (collision)
			{
				case SimpleCollisionArea simpleCollisionArea:
					return simpleCollisionArea.ToModel();
				case OffsetCollisionArea offSetCollisionArea:
					return offSetCollisionArea.ToModel();
				case CollisionAreaCollection collisionAreaCollection:
					return collisionAreaCollection.ToModel();
			}

			return null;
		}
	}
}
