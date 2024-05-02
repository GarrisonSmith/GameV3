﻿using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping.interfaces;
using System.Runtime.Serialization;

namespace DiscModels.Engine.TileMapping
{
	[DataContract(Name = "tile")]
    public class TileModel<AreaType, CollisionType> : IAmATileModel
		where AreaType : IAmAAreaModel
		where CollisionType : IAmACollisionAreaModel
	{
        [DataMember(Name = "position")]
        public PositionModel Position { get; set; }

        [DataMember(Name = "drawData")]
        public DrawDataModel DrawData { get; set; }

        [DataMember(Name = "area")]
        public AreaType Area { get; set; }

        [DataMember(Name = "collisionArea")]
        public CollisionType CollisionArea { get; set; }
    }
}
