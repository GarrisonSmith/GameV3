namespace Engine.Physics.Areas.interfaces
{
	/// <summary>
	/// Represents something with a complex area.
	/// </summary>
	public interface IHaveComplexArea : IHaveArea
	{
		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		new public ComplexArea Area { get; set; }
	}
}
