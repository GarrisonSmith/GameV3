using DiscModels.Engine.Physics.Areas;
using DiscModels.Engine.Physics.Collisions.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Collisions
{
	[DataContract(Name = "simpleCollisionArea")]
	public class SimpleCollisionAreaModel : IAmACollisionAreaModel
	{
		[DataMember(Name = "area")]
		public SimpleAreaModel Area { get; set; }

		[DataMember(Name = "movementTerrainTypes")]
		public List<int> MovementTerrainTypes { get; set; }
	}
}
