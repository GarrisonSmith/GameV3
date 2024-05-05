using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
    [DataContract(Name = "animation")]
    public class AnimationModel
    {
        [DataMember(Name = "isPlaying", Order = 1)]
        public bool IsPlaying { get; set; }

        [DataMember(Name = "currentFrameIndex", Order = 2)]
        public int CurrentFrameIndex { get; set; }

        [DataMember(Name = "frameConstantDuration", Order = 3)]
        public int? FrameConstantDuration { get; set; }

        [DataMember(Name = "frameMinDuration", Order = 4)]
        public int? FrameMinDuration { get; set; }

        [DataMember(Name = "frameMaxDuration", Order = 5)]
        public int? FrameMaxDuration { get; set; }

        [DataMember(Name = "frames", Order = 6)]
        public DrawDataModel[] Frames { get; set; }
    }
}
