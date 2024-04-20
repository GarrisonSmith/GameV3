using Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.TileMapping.Base.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace Engine.View
{
	/// <summary>
	/// Represents a Camera.
	/// </summary>
	public class Camera : IHaveArea
	{
		private static Camera camera;

		/// <summary>
		/// Start the camera.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="startingLocation">The starting location.</param>
		/// <returns>The camera.</returns>
		public static Camera StartCamera(Game game, Vector2 startingLocation)
		{
			camera ??= new Camera(game);
			camera.Position.Coordinates = startingLocation;
			return camera;
		}

		/// <summary>
		/// Gets the camera.
		/// </summary>
		/// <returns>The camera.</returns>
		public static Camera GetCamera()
		{
			return camera;
		}

		private byte zoom;
		private Matrix cameraMatrix;
		private SimpleArea area;

		/// <summary>
		/// Gets or sets a value indicating whether vertical movement is locked.
		/// </summary>
		public bool VerticalMovementLocked { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether horizontal movement is locked.
		/// </summary>
		public bool HorizontalMovementLocked { get; set; }

		/// <summary>
		/// Gets or sets the current zoom level of this Camera. Describes the pixel dimensions of a tile.
		/// </summary>
		public byte Zoom
		{
			get => this.zoom;

			set
			{
				if (this.MinZoom <= value && value <= this.MaxZoom)
				{
					this.zoom = value;
					this.Stretch = this.Zoom / (float)Tile.TILE_DIMENSIONS;
					Vector2 originalCenter = this.Area.Center;
					this.Area.Width = (int)(this.Game.GraphicsDevice.Viewport.Width / this.Stretch);
					this.Area.Height = (int)(this.Game.GraphicsDevice.Viewport.Height / this.Stretch);
					this.Area.Center = originalCenter;
				}
			}
		}

		/// <summary>
		/// Gets or sets the max zoom.
		/// </summary>
		public byte MaxZoom { get; set; }

		/// <summary>
		/// Gets or sets the min zoom.
		/// </summary>
		public byte MinZoom { get; set; }

		/// <summary>
		/// Gets or sets the stretch. 
		/// </summary>
		private float Stretch { get; set; }

		/// <summary>
		/// Gets or sets the rotation.
		/// </summary>
		public float Rotation { get; set; }

		/// <summary>
		/// Get or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the camera matrix. This is applied to the draw each frame.
		/// </summary>
		public Matrix CameraMatrix 
		{
			get
			{
				Matrix tempCameraMatrix = this.cameraMatrix;
				tempCameraMatrix.M41 = -this.Area.X;
				tempCameraMatrix.M42 = -this.Area.Y;

				return tempCameraMatrix * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(Stretch);
			}

			private set => this.cameraMatrix = value;
		}

		/// <summary>
		/// Gets or sets the camera area. This represents what is in view of the camera.
		/// </summary>
		public SimpleArea Area { get => this.area; set => this.area = value; }

		/// <summary>
		/// Gets the area.
		/// </summary>
		IAmAArea IHaveArea.Area { get => this.Area; }

		/// <summary>
		/// Gets or sets the camera bounding area. This is the area the camera center cannot leave.
		/// </summary>
		public SimpleArea CameraBounding { get; set; }

		/// <summary>
		/// Gets or sets the game.
		/// </summary>
		public Game Game { get; set; }

		/// <summary>
		/// Initializes a new instance of the Camera class.
		/// </summary>
		/// <param name="game">The game.</param>
		private Camera(Game game)
		{ 
			this.Game = game;
			this.Position = new Position(0, 0);
			this.Area = new SimpleArea(this.Position, this.Game.GraphicsDevice.Viewport.Width, this.Game.GraphicsDevice.Viewport.Height);
			this.VerticalMovementLocked = false;
			this.HorizontalMovementLocked = false;
			this.MaxZoom = 128;
			this.MinZoom = 32;
			this.Zoom = 64;
			this.Stretch = 1f;
			this.Rotation = 0f;
			this.CameraMatrix = new Matrix()
			{
				M11 = 1f,
				M12 = 0f,
				M13 = 0f,
				M14 = 0f,
				M21 = 0f,
				M22 = 1f,
				M23 = 0f,
				M24 = 0f,
				M31 = 0f,
				M32 = 0f,
				M33 = 1f,
				M34 = 0f,
				M41 = 0f,
				M42 = 0f,
				M43 = 0f,
				M44 = 1f
			};
		}

		/// <summary>
		/// Zooms the camera in by the provided amount if the camera is not at max zoom already.
		/// </summary>
		/// <param name="amount">The amount to zoom in by.</param>
		public void ZoomIn(byte amount = 1)
		{
			if (this.Zoom + amount <= this.MaxZoom)
			{
				this.Zoom += amount;
			}
		}

		/// <summary>
		/// Zooms the camera out by the provided amount if the camera is not at min zoom already.
		/// </summary>
		/// <param name="amount">The amount to zoom out by.</param>
		public void ZoomOut(byte amount = 1)
		{
			if (this.MinZoom <= this.Zoom - amount)
			{
				this.Zoom -= amount;
			}
		}

		/// <summary>
		/// Zooms the camera in by the provided percent of the current zoom if the camera is not at max zoom already.
		/// </summary>
		/// <param name="percent">The percent of the current zoom to zoom in by. At a minimum will increase zoom by 1.</param>
		public void SmoothZoomIn(float percent = .01f)
		{
			if (percent <= 0)
			{
				return;
			}

			if (this.Zoom + (this.Zoom * percent) <= this.MaxZoom)
			{
				this.Zoom += (byte)Math.Ceiling(this.Zoom * percent);
			}
		}

		/// <summary>
		/// Zooms the camera out by the provided percent of the current zoom if the camera is not at min zoom already.
		/// </summary>
		/// <param name="percent">The percent of the current zoom to zoom out by. At a minimum will decrease zoom by 1.</param>
		public void SmoothZoomOut(float percent = .01f)
		{
			if (percent <= 0)
			{
				return;
			}

			if (this.MinZoom <= this.Zoom - (this.Zoom * percent))
			{
				this.Zoom -= (byte)Math.Ceiling(this.Zoom * percent);
			}
		}

		/// <summary>
		/// Moves up by the specified amount.
		/// </summary>
		/// <param name="amount">The amount.</param>
		/// <param name="forced">A value indicating whether movement is forced.</param>
		public void Move(float directionRadians, float amount, bool forced = false)
		{
			float verticalMovementAmount = (float)Math.Round(amount * -Math.Sin(directionRadians), 5);
			float horizontalMovementAmount = (float)Math.Round(amount * (float)Math.Cos(directionRadians), 5);

			if (forced || (!this.VerticalMovementLocked && (this.CameraBounding == null || (this.CameraBounding.TopLeft.Y <= this.Area.Center.Y + verticalMovementAmount && this.CameraBounding.BottomRight.Y >= this.Area.Center.Y + verticalMovementAmount))))
			{
				this.Area.Y += verticalMovementAmount;
			}
			else if (!this.VerticalMovementLocked)
			{
				if (verticalMovementAmount < 0)
				{
					this.Area.Y = this.CameraBounding.TopLeft.Y - (this.Area.Height / 2);
				}
				else if (verticalMovementAmount > 0)
				{
					this.Area.Y = this.CameraBounding.BottomRight.Y - (this.Area.Height / 2);
				}
			}

			if (forced || (!this.HorizontalMovementLocked && (this.CameraBounding == null || (this.CameraBounding.TopLeft.X <= this.Area.Center.X + horizontalMovementAmount && this.CameraBounding.BottomRight.X >= this.Area.Center.X + horizontalMovementAmount))))
			{
				this.Area.X += horizontalMovementAmount;
			}
			else if (!this.HorizontalMovementLocked)
			{
				if (horizontalMovementAmount < 0)
				{
					this.Area.X = this.CameraBounding.TopLeft.X - (this.Area.Width / 2);
				}
				else if (horizontalMovementAmount > 0)
				{
					this.Area.X = this.CameraBounding.BottomRight.X - (this.Area.Width / 2);
				}
			}
		}
	}
}
