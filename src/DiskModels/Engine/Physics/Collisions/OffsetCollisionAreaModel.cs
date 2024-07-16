using DiscModels.Engine.Physics.Areas;
using DiscModels.Engine.Physics.Collisions.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Collisions
{
	[DataContract(Name = "offsetCollisionAreaModel")]
	public class OffsetCollisionAreaModel : IAmACollisionAreaModel
	{
		[DataMember(Name = "movementTerrainTypes", Order = 1)]
		public List<int> MovementTerrainTypes { get; set; }

		[DataMember(Name = "area", Order = 2)]
		public OffsetAreaModel Area { get; set; }
	}
}
