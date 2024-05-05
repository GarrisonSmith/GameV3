using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping.interfaces;
using System.Runtime.Serialization;

namespace DiscModels.Engine.TileMapping
{
	[DataContract(Name = "tile")]
    public class TileModel<AreaType, CollisionType> : IAmATileModel<AreaType, CollisionType>
		where AreaType : IAmAAreaModel
		where CollisionType : IAmACollisionAreaModel
	{
        [DataMember(Name = "position", Order = 1)]
        public PositionModel Position { get; set; }

        [DataMember(Name = "drawData", Order = 2)]
        public DrawDataModel DrawData { get; set; }

        [DataMember(Name = "area", Order = 3)]
        public AreaType Area { get; set; }

        [DataMember(Name = "collisionArea", Order = 4)]
        public CollisionType CollisionArea { get; set; }
	}
}
