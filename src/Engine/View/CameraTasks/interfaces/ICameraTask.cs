using Microsoft.Xna.Framework;

namespace Engine.View.CameraTasks.interfaces
{
	/// <summary>
	/// Represents a type of Camera Task.
	/// </summary>
	public interface ICameraTask
	{
		/// <summary>
		/// Gets the Camera.
		/// </summary>
		Camera Camera { get; set; }

		/// <summary>
		/// Starts the camera task. 
		/// </summary>
		void StartTask();

		/// <summary>
		/// Progresses the camera task.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <returns>A value indicating whether the task is completed or not.</returns>
		bool ProgressTask(GameTime gameTime);
	}
}
