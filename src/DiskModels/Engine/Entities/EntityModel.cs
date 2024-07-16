using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Entities
{
	[DataContract(Name = "entity")]
	public class EntityModel<AreaType, CollisionType>
		where AreaType : IAmAAreaModel
		where CollisionType : IAmACollisionAreaModel
	{
		[DataMember(Name = "layer", Order = 1)]
		public ushort Layer { get; set; }

		[DataMember(Name = "name", Order = 2)]
		public string Name { get; set; }

		[DataMember(Name = "moveSpeed", Order = 3)]
		public MoveSpeedModel MoveSpeed { get; set; }

		[DataMember(Name = "orientation", Order = 4)]
		public ushort Orientation { get; set; }

		[DataMember(Name = "position", Order = 5)]
		public PositionModel Position { get; set; }

		[DataMember(Name = "animations", Order = 6)]
		public AnimationModel[] Animations { get; set; }

		[DataMember(Name = "area", Order = 7)]
		public AreaType Area { get; set; }

		[DataMember(Name = "collisionArea", Order = 8)]
		public CollisionType CollisionArea { get; set; }
	}
}
