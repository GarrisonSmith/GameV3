namespace Engine.Physics.Areas.interfaces
{
	/// <summary>
	/// Represents something with a simple area.
	/// </summary>
	public interface IHaveSimpleArea : IHaveArea
	{
		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		new public SimpleArea Area { get; set; }
	}
}
