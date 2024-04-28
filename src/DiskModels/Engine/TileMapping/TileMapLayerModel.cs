using DiskModels.Engine.TileMapping.interfaces;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiskModels.Engine.TileMapping
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
