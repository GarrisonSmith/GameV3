using Engine.Physics.Areas.interfaces;

namespace Engine.Drawing.Base.interfaces
{
	/// <summary>
	/// Represents something that is animated.
	/// </summary>
	public interface IAmAnimated : IHaveArea
	{
		/// <summary>
		/// Gets or sets the animation.
		/// </summary>
		Animation Animation { get; set; }
	}
}
