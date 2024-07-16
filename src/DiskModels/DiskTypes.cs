using DiscModels.Engine.Entities;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Areas;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.Physics.Collisions;
using DiscModels.Engine.Physics;
using DiscModels.Engine.TileMapping;
using System;

namespace DiscModels
{
	/// <summary>
	/// Represents the disk types.
	/// </summary>
	public static class DiskTypes
	{
		/// <summary>
		/// Gets the disc types.
		/// </summary>
		/// <returns>The types.</returns>
		public static Type[] GetDiscTypes()
		{
			return new Type[]
			{
				typeof(TileModel<IAmAAreaModel, IAmACollisionAreaModel>),
				typeof(AnimatedTileModel<IAmAAreaModel, IAmACollisionAreaModel>),
				typeof(SimpleAreaModel),
				typeof(OffsetAreaModel),
				typeof(AreaCollectionModel),
				typeof(SimpleCollisionAreaModel),
				typeof(OffsetCollisionAreaModel),
				typeof(CollisionAreaCollectionModel),
				typeof(EntityModel<IAmAAreaModel, IAmACollisionAreaModel>),
				typeof(MoveSpeedModel)
			};
		}
	}
}
