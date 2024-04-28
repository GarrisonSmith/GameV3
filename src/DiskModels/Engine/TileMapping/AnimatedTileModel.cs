﻿using DiskModels.Engine.Drawing;
using DiskModels.Engine.Physics;
using DiskModels.Engine.Physics.Areas.interfaces;
using DiskModels.Engine.Physics.Collisions.interfaces;
using DiskModels.Engine.TileMapping.interfaces;
using System.Runtime.Serialization;

namespace DiskModels.Engine.TileMapping
{
	[DataContract(Name = "animatedTile")]
    public class AnimatedTileModel<AreaType, CollisionType> : IAmATileModel
		where AreaType : IAmAAreaModel
		where CollisionType : IAmACollisionAreaModel
	{
        [DataMember(Name = "position")]
        public PositionModel Position { get; set; }

        [DataMember(Name = "animation")]
        public AnimationModel Animation { get; set; }

        [DataMember(Name = "area")]
        public AreaType Area { get; set; }

        [DataMember(Name = "collisionArea")]
        public CollisionType CollisionArea { get; set; }
    }
}
