using DiscModels.Engine.Physics.Collisions.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Collisions
{
	[DataContract(Name = "collisionAreaCollectionModel")]
	public class CollisionAreaCollectionModel : IAmACollisionAreaModel
	{
		[DataMember(Name = "movementTerrainTypes", Order = 1)]
		public List<int> MovementTerrainTypes { get; set; }

		[DataMember(Name = "area", Order = 2)]
		public IAmACollisionAreaModel[] CollisionAreas { get; set; }
	}
}
