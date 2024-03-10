using Engine.Physics.Base;
using Engine.View.CameraTasks.interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Engine.View.CameraTasks
{
	/// <summary>
	/// Represents a pan to with zooming task.
	/// </summary>
	public class PanToWithZoomingTask : ICameraTask
	{
		/// <summary>
		/// Gets or sets a value indicating whether the first zooming is done.
		/// </summary>
		public bool FirstZoomingDone { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the panning is done.
		/// </summary>
		public bool PanningDone { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the second zooming is done.
		/// </summary>
		public bool SecondZoomingDone { get; set; }

		/// <summary>
		/// Gets or sets the zoom speed.
		/// </summary>
		public byte ZoomSpeed { get; set; }

		/// <summary>
		/// Gets or sets the destination zoom.
		/// </summary>
		public byte? DestinationZoom { get; set; }

		/// <summary>
		/// Gets or sets the zoom speed.
		/// </summary>
		public byte OriginalZoom { get; set; }

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
		/// Initializes a new instance of the PanToWithZoomingTask class.
		/// </summary>
		/// <param name="zoomSpeed">The zoom speed.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="panSpeed">The pan speed.</param>
		public PanToWithZoomingTask(byte zoomSpeed, Vector2 destination, MoveSpeed panSpeed)
		{
			this.ZoomSpeed = zoomSpeed;
			this.Destination = destination;
			this.MoveSpeed = panSpeed;
		}

		/// <summary>
		/// Initializes a new instance of the PanToWithZoomingTask class.
		/// </summary>
		/// <param name="zoomSpeed">The zoom speed.</param>
		/// <param name="destinationZoom">The destination zoom.</param>
		/// <param name="destination">The destination.</param>
		/// <param name="panSpeed">The pan speed.</param>
		public PanToWithZoomingTask(byte zoomSpeed, byte destinationZoom, Vector2 destination, MoveSpeed panSpeed)
		{
			this.ZoomSpeed = zoomSpeed;
			this.DestinationZoom = destinationZoom;
			this.Destination = destination;
			this.MoveSpeed = panSpeed;
		}

		/// <summary>
		/// Starts the camera task. 
		/// </summary>
		public void StartTask()
		{
			this.Camera = Camera.GetCamera();
			this.OriginalZoom = this.Camera.Zoom;
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
			if (!this.FirstZoomingDone)
			{
				this.FirstZoomingDone = this.ProgressFirstZooming();
			}
			else if (!this.PanningDone)
			{
				this.PanningDone = this.ProgressPanning(this.MoveSpeed.TotalMovementAmount);
			}
			else if (!this.SecondZoomingDone)
			{
				this.SecondZoomingDone = this.ProgressSecondZooming();
			}

			return this.FirstZoomingDone && this.PanningDone && this.SecondZoomingDone;
		}

		/// <summary>
		/// Progresses the first zooming.
		/// </summary>
		private bool ProgressFirstZooming()
		{
			if (this.DestinationZoom.HasValue)
			{
				if (Math.Abs(this.Camera.Zoom - this.ZoomSpeed) <= this.ZoomSpeed)
				{
					this.Camera.Zoom = this.DestinationZoom.Value;
				}
				else if (this.Camera.Zoom + this.ZoomSpeed < this.DestinationZoom)
				{
					this.Camera.Zoom += this.ZoomSpeed;
				}
				else if (this.Camera.Zoom - this.ZoomSpeed > this.DestinationZoom)
				{
					this.Camera.Zoom -= this.ZoomSpeed;
				}

				return this.Camera.Zoom == this.DestinationZoom;
			}
            else
            {
				if (Math.Abs(this.Camera.Zoom - this.ZoomSpeed) <= this.ZoomSpeed)
				{
					this.Camera.Zoom = this.Camera.MinZoom;
				}
				else
				{
					this.Camera.Zoom -= this.ZoomSpeed;
				}

				return this.Camera.Area.Contains(this.Destination) || this.Camera.Zoom == this.Camera.MinZoom;
			}
		}

		/// <summary>
		/// Progresses the panning.
		/// </summary>
		/// <param name="movementAmount">The movement amount.</param>
		private bool ProgressPanning(float movementAmount)
		{
			if (movementAmount < Math.Sqrt(Math.Pow(this.Camera.Area.Center.X - this.Destination.X, 2) + Math.Pow(this.Camera.Area.Center.Y - this.Destination.Y, 2)))
			{
				this.Camera.Move(this.Theta, movementAmount, false);
			}
			else
			{
				this.Camera.Area.Center = this.Destination;
			}

			return this.Camera.Area.Center == this.Destination;
		}

		/// <summary>
		/// Progress the second zooming.
		/// </summary>
		private bool ProgressSecondZooming()
		{
			if (Math.Abs(this.Camera.Zoom - this.ZoomSpeed) <= this.ZoomSpeed)
			{
				this.Camera.Zoom = this.OriginalZoom;
			}
			else if (this.Camera.Zoom > this.OriginalZoom)
			{
				this.Camera.Zoom -= this.ZoomSpeed;
			}
			else if (this.Camera.Zoom < this.OriginalZoom)
			{
				this.Camera.Zoom += this.ZoomSpeed;
			}

			return this.Camera.Zoom == this.OriginalZoom;
		}
	}
}
