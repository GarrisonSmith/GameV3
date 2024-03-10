using Engine.Loading.Base.interfaces;
using System;

namespace Engine.Core
{
	/// <summary>
	/// Represents a random manager.
	/// </summary>
	public class RandomManager : ICanBeLoaded
	{
		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Starts the random manager.
		/// </summary>
		/// <returns>The random manager.</returns>
		public static RandomManager StartRandomManager()
		{
			return Managers.RandomManager ?? new RandomManager();
		}

		/// <summary>
		/// Gets or sets the random generator.
		/// </summary>
		protected Random Random { get; set; }

		/// <summary>
		/// Initializes a new instance of the RandomManager class.
		/// </summary>
		public RandomManager() 
		{
			this.Random = new();
		}

		/// <summary>
		/// Gets a random int.
		/// </summary>
		/// <param name="lowerBound">The lower bound.</param>
		/// <param name="upperBound">The upper bound.</param>
		/// <returns>A random int.</returns>
		public int GetRandomInt(int lowerBound, int upperBound)
		{
			return this.Random.Next(lowerBound, upperBound + 1);
		}

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			this.IsLoaded = true;
		}
	}
}
