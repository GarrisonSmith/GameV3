namespace Engine.Saving.Base.interfaces
{
	/// <summary>
	/// Represents something that can be saved.
	/// </summary>
	/// <typeparam name="T">The corresponding disk model type.</typeparam>
	public interface ICanBeSaved<T>
	{
		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		T ToModel();
	}
}
