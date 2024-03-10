namespace Engine.Loading
{
	/// <summary>
	/// Represents a loading configuration.
	/// </summary>
	public static class LoadingConfiguration
	{
		/// <summary>
		/// Gets the tileset names to be loaded.
		/// </summary>
		public static string[] TileSetNames { get; } =
		{
			"debug_tileset",
			"brickwall_tileset",
			"grass_tileset",
			"woodfloor_tileset"
		};


		public static string[] CharacterSetNames { get; } =
		{
			"character3_characterset"
		};

		/// <summary>
		/// Gets the tile map names to be loaded.
		/// </summary>
		public static string[] TileMapNames { get; } =
		{
			//Needs to be lower case.
			"test_map",
			"animated_test_map"
		};
	}
}
