using System.Runtime.Serialization;

namespace DiskModels.Engine.Drawing
{
    [DataContract(Name = "drawData")]
    public class DrawDataModel
    {
        [DataMember(Name = "drawDataName")]
        public string DrawDataName { get; set; }
    }
}
