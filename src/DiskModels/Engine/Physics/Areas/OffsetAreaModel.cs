using DiskModels.Engine.Physics.Areas.interfaces;
using System.Runtime.Serialization;

namespace DiskModels.Engine.Physics.Areas
{
    [DataContract(Name = "offsetArea")]
    public class OffsetAreaModel : IAmAAreaModel
	{
        [DataMember(Name = "horizontalOffset")]
        public float HorizontalOffset { get; set; }

        [DataMember(Name = "verticalOffset")]
        public float VerticalOffset { get; set; }

        [DataMember(Name = "width")]
        public float Width { get; set; }

        [DataMember(Name = "height")]
        public float Height { get; set; }
    }
}
