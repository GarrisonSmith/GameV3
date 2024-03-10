using Engine.Controls;
using Engine.Physics.Base;
using Engine.View.CameraTasks.interfaces;
using Microsoft.Xna.Framework;

namespace Engine.View.CameraTasks
{
	/// <summary>
	/// Represents a free movement task.
	/// </summary>
	public class FreeMovementTask : ICameraTask
	{
		/// <summary>
		/// Gets or sets the move speed.
		/// </summary>
		public MoveSpeed MoveSpeed { get; set; }

		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public Camera Camera { get; set; }

		/// <summary>
		/// Initializes a new instance of the FreeMovementTask class.
		/// </summary>
		public FreeMovementTask()
		{
			this.MoveSpeed = new MoveSpeed(6);
			this.Camera = Camera.GetCamera();
		}

		/// <summary>
		/// Initializes a new instance of the FreeMovementTask class.
		/// </summary>
		/// <param name="moveSpeed">The move speed.</param>
		public FreeMovementTask(MoveSpeed moveSpeed)
		{ 
			this.MoveSpeed = moveSpeed;
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
			if (Managers.ControlManager.ControlState.DirectionRadians.HasValue)
			{
				this.Camera.Move(Managers.ControlManager.ControlState.DirectionRadians.Value, this.MoveSpeed.TotalMovementAmount, false);
			}

			return false; //This task cannot finish.
		}
	}
}
