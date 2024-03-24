namespace Engine.Loading.Configurations
{
	/// <summary>
	/// The collection of characters and their file names.
	/// </summary>
	public static class CharactersConfig
	{
		public static string Character { get; } = "character3_characterset";

		/// <summary>
		/// Gets the character file names to be loaded.
		/// </summary>
		public static string[] CharacterFileNames { get; } =
		{
			CharactersConfig.Character,
		};
	}
}
