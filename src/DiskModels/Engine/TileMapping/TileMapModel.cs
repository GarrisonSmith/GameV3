using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiscModels.Engine.TileMapping
{
	[DataContract(Name = "tileMap")]
	public class TileMapModel
	{
		[DataMember(Name = "name", Order = 1)]
		public string Name { get; set; }

		[DataMember(Name = "isActiveTileMap", Order = 2)]
		public bool IsActiveTileMap { get; set; }

		[DataMember(Name = "layers", Order = 3)]
		public List<TileMapLayerModel> Layers { get; set; }
	}
}
