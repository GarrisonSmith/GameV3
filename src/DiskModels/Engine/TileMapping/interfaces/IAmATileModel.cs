using DiscModels.Engine.Physics;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using System.Runtime.Serialization;

namespace DiscModels.Engine.TileMapping.interfaces
{
	public interface IAmATileModel<AreaType, CollisionType>
		where AreaType : IAmAAreaModel
		where CollisionType : IAmACollisionAreaModel
	{
		[DataMember(Name = "position")]
		public PositionModel Position { get; set; }

		[DataMember(Name = "area")]
		public AreaType Area { get; set; }

		[DataMember(Name = "collisionArea")]
		public CollisionType CollisionArea { get; set; }
	}
}
