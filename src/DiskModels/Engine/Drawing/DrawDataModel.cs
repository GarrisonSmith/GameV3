using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
    [DataContract(Name = "drawData")]
    public class DrawDataModel
    {
        [DataMember(Name = "spritesheetName")]
        public string SpritesheetName { get; set; }

		[DataMember(Name = "spritesheetBox")]
		public Rectangle SpritesheetBox { get; set; }
	}
}
