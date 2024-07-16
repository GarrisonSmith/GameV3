using DiscModels.Engine.Physics;
using Engine.Saving.Base.interfaces;
using Engine.TileMapping.Base.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Physics.Base
{
	/// <summary>
	/// Represents a move speed.
	/// </summary>
	public class MoveSpeed : IDisposable, ICanBeSaved<MoveSpeedModel>
	{
		/// <summary>
		/// Gets or sets the tiles per second.
		/// </summary>
		public float TilesPerSecond { get; set; }

		/// <summary>
		/// Gets or sets the total movement amount.
		/// </summary>
		public float TotalMovementAmount { get; set; }

		/// <summary>
		/// Initializes a new instance of the MoveSpeed class.
		/// </summary>
		/// <param name="moveSpeedModel">The move speed model.</param>
		public MoveSpeed(MoveSpeedModel moveSpeedModel)
		{ 
			this.TilesPerSecond = moveSpeedModel.TilesPerSecond;
			this.TotalMovementAmount = 0f;
			Managers.PhysicsManager.MoveSpeeds.Add(this);
		}

		/// <summary>
		/// Initializes a new instance of the MoveSpeed class.
		/// </summary>
		/// <param name="tilesPerSecond">The tiles per second.</param>
		public MoveSpeed(float tilesPerSecond)
		{
			this.TilesPerSecond = tilesPerSecond;
			this.TotalMovementAmount = 0f;
			Managers.PhysicsManager.MoveSpeeds.Add(this);
		}

		/// <summary>
		/// Gets the vertical movement amount.
		/// </summary>
		/// <param name="directionRadians">The direction radians.</param>
		/// <returns>The vertical movement amount.</returns>
		public float GetVerticalMovementAmount(float directionRadians)
		{
			return (float)Math.Round(this.TotalMovementAmount * -Math.Sin(directionRadians), 5);
		}

		/// <summary>
		/// Gets the horizontal movement amount.
		/// </summary>
		/// <param name="directionRadians">The direction radians.</param>
		/// <returns>The horizontal movement amount.</returns>
		public float GetHorizontalMovementAmount(float directionRadians)
		{
			return (float)Math.Round(this.TotalMovementAmount * Math.Cos(directionRadians), 5);
		}

		/// <summary>
		/// Updates the move speed.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public void Update(GameTime gameTime)
		{
			this.TotalMovementAmount = Tile.TILE_DIMENSIONS * this.TilesPerSecond * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000);
		}

		/// <summary>
		/// Disposes the MoveSpeed.
		/// </summary>
		public void Dispose()
		{
			Managers.PhysicsManager.MoveSpeeds.Remove(this);
		}

		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		public MoveSpeedModel ToModel()
		{
			return new MoveSpeedModel
			{
				TilesPerSecond = this.TilesPerSecond
			};
		}
	}
}
