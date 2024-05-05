using DiscModels.Engine.Physics.Areas;
using DiscModels.Engine.Physics.Collisions.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Collisions
{
	[DataContract(Name = "simpleCollisionArea")]
	public class SimpleCollisionAreaModel : IAmACollisionAreaModel
	{
		[DataMember(Name = "movementTerrainTypes", Order = 1)]
		public List<int> MovementTerrainTypes { get; set; }

		[DataMember(Name = "area", Order = 2)]
		public SimpleAreaModel Area { get; set; }
	}
}
