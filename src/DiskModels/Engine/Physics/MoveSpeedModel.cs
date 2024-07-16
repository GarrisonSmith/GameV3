using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics
{
	[DataContract(Name = "moveSpeedModel")]
	public class MoveSpeedModel
	{
		[DataMember(Name = "tilesPerSecond", Order = 1)]
		public float TilesPerSecond { get; set; }
	}
}
