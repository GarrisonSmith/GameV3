using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
    [DataContract(Name = "animation")]
    public class AnimationModel
    {
        [DataMember(Name = "isPlaying")]
        public bool IsPlaying { get; set; }

        [DataMember(Name = "currentFrameIndex")]
        public int CurrentFrameIndex { get; set; }

        [DataMember(Name = "frameMinDuration")]
        public int? FrameMinDuration { get; set; }

        [DataMember(Name = "frameMaxDuration")]
        public int? FrameMaxDuration { get; set; }

        [DataMember(Name = "frames")]
        public DrawDataModel[] Frames { get; set; }
    }
}
