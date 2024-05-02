using DiscModels.Engine.Physics.Areas.interfaces;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Areas
{
    [DataContract(Name = "simpleArea")]
    public class SimpleAreaModel : IAmAAreaModel
    {
        [DataMember(Name = "width")]
        public float Width { get; set; }

        [DataMember(Name = "height")]
        public float Height { get; set; }
    }
}

