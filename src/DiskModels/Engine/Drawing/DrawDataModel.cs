using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
    [DataContract(Name = "drawData")]
    public class DrawDataModel
    {
        [DataMember(Name = "spritesheetName", Order = 1)]
        public string SpritesheetName { get; set; }

		[DataMember(Name = "spritesheetBox", Order = 2)]
		public Rectangle SpritesheetBox { get; set; }
	}
}
