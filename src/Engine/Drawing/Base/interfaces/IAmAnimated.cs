using Engine.Physics.Areas.interfaces;
using System;

namespace Engine.Drawing.Base.interfaces
{
	/// <summary>
	/// Represents something that is animated.
	/// </summary>
	public interface IAmAnimated : IHaveArea, IDisposable
	{
		/// <summary>
		/// Gets or sets the animation.
		/// </summary>
		Animation Animation { get; set; }
	}
}
