namespace Engine.Loading.Configurations
{
	/// <summary>
	/// The collection of fonts and their file names.
	/// </summary>
	public static class FontsConfig
	{
		public static string MonoLight12 { get; } = "MonoLight12";

		public static string MonoRegular12 { get; } = "MonoRegular12";

		public static string MonoBold12 { get; } = "MonoBold12";

		/// <summary>
		/// Gets the font file names to be loaded.
		/// </summary>
		public static string[] FontFileNames { get; } =
		{
			FontsConfig.MonoLight12,
			FontsConfig.MonoRegular12,
			FontsConfig.MonoBold12,
		};
	}
}
