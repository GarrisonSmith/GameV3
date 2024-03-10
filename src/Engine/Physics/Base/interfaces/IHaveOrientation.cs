using Engine.Physics.Base.enums;

namespace Engine.Physics.Base.interfaces
{
	/// <summary>
	/// Represents something with a orientation.
	/// </summary>
	public interface IHaveOrientation
	{
		/// <summary>
		/// Gets or sets the orientation type.
		/// </summary>
		OrientationTypes Orientation { get; set; }
	}
}
