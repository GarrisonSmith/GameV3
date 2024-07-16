using DiscModels.Engine.Physics.Areas.interfaces;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Areas
{
	[DataContract(Name = "areaCollection")]
	public class AreaCollectionModel : IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }

		[DataMember(Name = "areas", Order = 3)]
		public IAmAAreaModel[] Areas { get; set; }
	}
}
