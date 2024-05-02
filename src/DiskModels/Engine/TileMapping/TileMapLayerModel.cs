using DiscModels.Engine.TileMapping.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscModels.Engine.TileMapping
{
	[DataContract(Name = "tileMapLayer")]
	public class TileMapLayerModel
	{
		[DataMember(Name = "layer")]
		public ushort Layer { get; set; }

		[DataMember(Name = "tiles")]
		public List<IAmATileModel> Tiles { get; set; }
	}
}
