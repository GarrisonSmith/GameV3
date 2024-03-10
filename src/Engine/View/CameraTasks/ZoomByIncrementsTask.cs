using Engine.View.CameraTasks.interfaces;
using Microsoft.Xna.Framework;
using System;

namespace Engine.View.CameraTasks
{
	/// <summary>
	/// Represents a zoom by increments task.
	/// </summary>
	public class ZoomByIncrementsTask : ICameraTask
	{
		/// <summary>
		/// The zoom speed.
		/// </summary>
		public byte ZoomSpeed { get; set; }

		/// <summary>
		/// The destination zoom for the task.
		/// </summary>
		public byte? DestinationZoom { get; set; }

		/// <summary>
		/// The point the task will attempt to zoom to view or until it hits the maximum zoom level.
		/// </summary>
		public Vector2? ViewPoint { get; set; }

		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public Camera Camera { get; set; }

		/// <summary>
		/// Initializes a new instance of the ZoomByIncrementsTask class.
		/// </summary>
		/// <param name="zoomSpeed">The zoom speed.</param>
		/// <param name="destinationZoom">The destination zoom.</param>
		public ZoomByIncrementsTask(byte zoomSpeed, byte destinationZoom)
		{
			this.ZoomSpeed = zoomSpeed;
			this.DestinationZoom = destinationZoom;
		}

		/// <summary>
		/// Initializes a new instance of the ZoomByIncrementsTask class.
		/// </summary>
		/// <param name="zoomSpeed">The zoom speed.</param>
		/// <param name="viewPoint">The view point.</param>
		public ZoomByIncrementsTask(byte zoomSpeed, Vector2 viewPoint)
		{
			this.ZoomSpeed = zoomSpeed;
			this.ViewPoint = viewPoint;
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
			if (this.ViewPoint.HasValue)
			{
				if (Math.Abs(this.Camera.Zoom - this.ZoomSpeed) <= this.ZoomSpeed)
				{
					this.Camera.Zoom = this.Camera.MinZoom;
				}
				else
				{
					this.Camera.Zoom -= this.ZoomSpeed;
				}

				return this.Camera.Area.Contains(this.ViewPoint.Value) || this.Camera.Zoom == this.Camera.MinZoom;
			}
			else
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
		}
	}
}
