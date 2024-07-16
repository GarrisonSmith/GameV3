﻿using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Areas.interfaces
{
	public interface IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }
	}
}
