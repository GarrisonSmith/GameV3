namespace Engine.Loading.Base.interfaces
{
    /// <summary>
    /// Represents something that can be loaded.
    /// </summary>
    public interface ICanBeLoaded
    {
        /// <summary>
        /// Gets or sets a value indicating whether this has been loaded.
        /// </summary>
        bool IsLoaded { get; set; }

		/// <summary>
		/// Loads data.
		/// </summary>
		void Load();
    }
}
