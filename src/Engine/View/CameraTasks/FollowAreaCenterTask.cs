using Engine.Physics.Areas.interfaces;
using Engine.View.CameraTasks.interfaces;
using Microsoft.Xna.Framework;

namespace Engine.View.CameraTasks
{
    /// <summary>
    /// Represents a follow area center task.
    /// </summary>
    public class FollowAreaCenterTask : ICameraTask
	{
		/// <summary>
		/// Gets or sets the area that this tasks is following.
		/// </summary>
		public IHaveArea AreaToFollow { get; set; }

		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public Camera Camera { get; set; }

		/// <summary>
		/// Initializes a new instance of the FollowAreaCenterTask struct.
		/// </summary>
		/// <param name="areaToFollow">The area to follow.</param>
		/// <param name="camera">The camera.</param>
		public FollowAreaCenterTask(IHaveArea areaToFollow)
		{
			this.AreaToFollow = areaToFollow;
		}

		/// <summary>
		/// Starts the camera task. 
		/// </summary>
		public void StartTask()
		{
			this.Camera = Camera.GetCamera();
		}

		/// <summary>
		/// Progresses the camera task.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <returns>A value indicating whether the task is completed or not.</returns>
		public bool ProgressTask(GameTime gameTime)
		{
			this.Camera.CenterCameraOnLocation(this.AreaToFollow.Area.Center);
			return false; //This task cannot finish.
		}
	}
}
