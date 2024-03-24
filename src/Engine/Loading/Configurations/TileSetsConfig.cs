namespace Engine.Loading.Configurations
{
	/// <summary>
	/// The collection of tile sets and their file names.
	/// </summary>
	public static class TileSetsConfig
	{
		public static string Debug { get; } = "debug_tileset";

		public static string BrickWall { get; } = "brickwall_tileset";

		public static string Grass { get; } = "grass_tileset";

		public static string WoodFloor { get; } = "woodfloor_tileset";

		/// <summary>
		/// Gets the tile set file names to be loaded.
		/// </summary>
		public static string[] TileSetFileNames { get; } =
		{
			TileSetsConfig.Debug,
			TileSetsConfig.BrickWall,
			TileSetsConfig.Grass,
			TileSetsConfig.WoodFloor,
		};
	}
}
