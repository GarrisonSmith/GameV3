using DiskModels.Engine.Physics.Areas;
using DiskModels.Engine.Physics.Collisions.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiskModels.Engine.Physics.Collisions
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
