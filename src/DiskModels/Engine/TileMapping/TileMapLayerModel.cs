using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscModels.Engine.TileMapping
{
	[DataContract(Name = "tileMapLayer")]
	public class TileMapLayerModel
	{
		[DataMember(Name = "layer", Order = 1)]
		public ushort Layer { get; set; }

		[DataMember(Name = "tiles", Order = 2)]
		public List<IAmATileModel<IAmAAreaModel, IAmACollisionAreaModel>> Tiles { get; set; }
	}
}
