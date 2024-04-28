namespace Engine.Saving
{
	/// <summary>
	/// Represents a saving manager.
	/// </summary>
	public class SavingManager
	{
		/// <summary>
		/// Starts the load manager.
		/// </summary>
		/// <returns>The load manager.</returns>
		public static SavingManager StartSavingManager()
		{
			return Managers.SavingManager ?? new SavingManager();
		}

		/// <summary>
		/// Initializes a new instance of the SavingManager class.
		/// </summary>
		public SavingManager()
		{ 
		
		}
	}
}
