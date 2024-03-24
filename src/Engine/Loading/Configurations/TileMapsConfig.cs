namespace Engine.Loading.Configurations
{
	/// <summary>
	/// The collection of tile maps and their file names.
	/// </summary>
	public static class TileMapsConfig
	{
		public static string AnimatedTestMap { get; } = "animated_test_map";

		/// <summary>
		/// Gets the tile map file names to be loaded.
		/// </summary>
		public static string[] TileMapFileNames { get; } =
		{
			TileMapsConfig.AnimatedTestMap,
		};
	}
}
