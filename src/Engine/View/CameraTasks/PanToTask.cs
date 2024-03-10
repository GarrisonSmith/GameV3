using Engine.Physics.Base;
using Engine.View.CameraTasks.interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Engine.View.CameraTasks
{
	/// <summary>
	/// Represents a pan to task.
	/// </summary>
	public class PanToTask : ICameraTask
	{
		/// <summary>
		/// Gets or sets the theta.
		/// </summary>
		public float Theta { get; set; }

		/// <summary>
		/// Gets or sets the destination.
		/// </summary>
		public Vector2 Destination { get; set; }

		/// <summary>
		/// Gets or sets the move speed.
		/// </summary>
		public MoveSpeed MoveSpeed { get; set; }

		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public Camera Camera { get; set; }

		/// <summary>
		/// Initializes a new instance of the PanToTask class.
		/// </summary
		/// <param name="destination">The destination.</param>
		public PanToTask(Vector2 destination)
		{
			this.Destination = destination;
			this.MoveSpeed = new MoveSpeed(6);
		}

		/// <summary>
		/// Initializes a new instance of the PanToTask class.
		/// </summary
		/// <param name="destination">The destination.</param>
		/// <param name="moveSpeed">The move speed.</param>
		public PanToTask(Vector2 destination, MoveSpeed moveSpeed)
		{ 
			this.Destination = destination;
			this.MoveSpeed = moveSpeed;
		}

		/// <summary>
		/// Starts the camera task. 
		/// </summary>
		public void StartTask()
		{
			this.Camera = Camera.GetCamera();
			this.Theta = (float)Math.Atan2(
				-this.Destination.Y + (this.Camera.Area.Center.Y == 0 ? .0001 : this.Camera.Area.Center.Y),
				this.Destination.X - (this.Camera.Area.Center.X == 0 ? .0001 : this.Camera.Area.Center.X)
			);
		}

		/// <summary>
		/// Progresses the camera task.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <returns>A value indicating whether the task is completed or not.</returns>
		public bool ProgressTask(GameTime gameTime)
		{
			if (this.MoveSpeed.TotalMovementAmount < Math.Sqrt(Math.Pow(this.Camera.Area.Center.X - this.Destination.X, 2) + Math.Pow(this.Camera.Area.Center.Y - this.Destination.Y, 2)))
			{
				this.Camera.Move(this.Theta, this.MoveSpeed.TotalMovementAmount, false);
			}
			else
			{ 
				this.Camera.Area.Center = this.Destination;
			}

			return this.Camera.Area.Center == this.Destination;
		}
	}
}
