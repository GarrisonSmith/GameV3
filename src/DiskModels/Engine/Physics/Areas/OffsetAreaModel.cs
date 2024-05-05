using DiscModels.Engine.Physics.Areas.interfaces;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Areas
{
    [DataContract(Name = "offsetArea")]
    public class OffsetAreaModel : IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }

		[DataMember(Name = "horizontalOffset", Order = 3)]
        public float HorizontalOffset { get; set; }

        [DataMember(Name = "verticalOffset", Order = 4)]
        public float VerticalOffset { get; set; }
    }
}
