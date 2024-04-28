using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DiskModels.Engine.TileMapping
{
	[DataContract(Name = "tileMap")]
	public class TileMapModel
	{
		[DataMember(Name = "isLoaded")]
		public bool IsLoaded { get; set; }

		[DataMember(Name = "isActiveTileMap")]
		public bool IsActiveTileMap { get; set; }

		[DataMember(Name = "name")]
		public string Name { get; set; }

		[DataMember(Name = "layers")]
		public List<TileMapLayerModel> Layers { get; set; }
	}
}
