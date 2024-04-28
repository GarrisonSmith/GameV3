using System.Runtime.Serialization;

namespace DiskModels.Engine.Physics
{
    [DataContract(Name = "position")]
    public class PositionModel
    {
        [DataMember(Name = "x")]
        public float X { get; set; }

        [DataMember(Name = "y")]
        public float Y { get; set; }
    }
}
