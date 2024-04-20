using Engine.Physics.Areas;
using Engine.Physics.Collisions;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base;
using Engine.TileMapping.Base.Tiles;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Physics.Base
{
	/// <summary>
	/// Represents information about a collision.
	/// </summary>
	public class CollisionInformation
	{
		/// <summary>
		/// Gets a value indicating whether a movement collision occurred.
		/// A movement collision either a terrain or entity collision.
		/// </summary>
		public bool MovementCollision { get => this.TerrainCollision || this.EntityCollision; }

		/// <summary>
		/// Gets or sets a value indicating whether or not terrain collision occurred.
		/// </summary>
		public bool TerrainCollision { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not a entity collision occurred.
		/// </summary>
		public bool EntityCollision { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether or not event collision occurred.
		/// </summary>
		public bool EventCollision { get; set; }

		/// <summary>
		/// Gets or sets the direction radians.
		/// </summary>
		public float DirectionRadians { get; set; }

		/// <summary>
		/// Gets or sets the final position.
		/// </summary>
		public Vector2 FinalPosition { get; set; }

		/// <summary>
		/// Gets or sets the moving entity.
		/// </summary>
		public IAmACollisionArea MovingCollisionArea { get; set; }

		/// <summary>
		/// Gets or sets the tile map layer.
		/// </summary>
		public TileMapLayer TileMapLayer { get; set; }

		/// <summary>
		/// Initializes a new instance of the CollisionInformation class.
		/// </summary>
		/// <param name="movingCollisionArea">The moving collision area.</param>
		/// <param name="tileMapLayer">The tile map layer.</param>
		/// <param name="startingPosition">The starting position.</param>
		/// <param name="directionRadians">The direction radians.</param>
		/// <param name="totalHorizontalMovement">The total horizontal movement.</param>
		/// <param name="totalVerticalMovement">The total vertical movement.</param>
		public CollisionInformation(IAmACollisionArea movingCollisionArea, TileMapLayer tileMapLayer, Vector2 startingPosition, float directionRadians, float totalHorizontalMovement, float totalVerticalMovement)
		{
			this.MovingCollisionArea = movingCollisionArea;
			this.TileMapLayer = tileMapLayer;
			this.TerrainCollision = false;
			this.EventCollision = false;
			this.EntityCollision = false;
			this.FinalPosition = startingPosition;
			this.DirectionRadians = directionRadians;
			this.GetMovementCollisionResult(totalHorizontalMovement, totalVerticalMovement);
		}

		/// <summary>
		/// Get the movement collision result.
		/// </summary>
		/// <param name="totalHorizontalMovement">The total horizontal movement.</param>
		/// <param name="totalVerticalMovement">The total vertical movement.</param>
		/// <returns></returns>
		private void GetMovementCollisionResult(float totalHorizontalMovement, float totalVerticalMovement)
		{
			if (totalHorizontalMovement == 0 && totalVerticalMovement == 0)
			{
				return;
			}

			var startingPosition = this.FinalPosition;
			float stepFactor;
			if (Math.Abs(totalHorizontalMovement) >= Math.Abs(totalVerticalMovement))
			{
				stepFactor = Math.Abs(this.MovingCollisionArea.Area.Width / totalHorizontalMovement);
			}
			else
			{
				stepFactor = Math.Abs(this.MovingCollisionArea.Area.Height / totalVerticalMovement);
			}


			if (stepFactor < 1)
			{
				var horizontalStep = totalHorizontalMovement * stepFactor;
				var verticalStep = totalVerticalMovement * stepFactor;
				var numberOfSteps = 1;

				while (horizontalStep * numberOfSteps <= totalHorizontalMovement && verticalStep * numberOfSteps <= totalVerticalMovement)
				{
					var candidatePosition = this.FinalPosition + new Vector2(horizontalStep, verticalStep);
					if (this.ProcessCheckCandidatePosition(candidatePosition, totalHorizontalMovement, totalVerticalMovement))
					{
						return;
					}

					this.FinalPosition = candidatePosition;
					numberOfSteps++;
				}
			}

			var destinationPosition = startingPosition + new Vector2(totalHorizontalMovement, totalVerticalMovement);
			if (this.ProcessCheckCandidatePosition(destinationPosition, totalHorizontalMovement, totalVerticalMovement))
			{
				return;
			};

			this.FinalPosition = destinationPosition;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="candidatePosition"></param>
		/// <param name="totalHorizontalMovement"></param>
		/// <returns></returns>
		private bool ProcessCheckCandidatePosition(Vector2 candidatePosition, float totalHorizontalMovement, float totalVerticalMovement)
		{
			if (this.CollisionAtCandidatePosition(candidatePosition))
			{
				var priorPosition = this.FinalPosition;
				this.FinalPosition = this.BisectSearchForFinalPosition(candidatePosition);

				if (totalHorizontalMovement != 0 && totalVerticalMovement != 0)
				{
					if (priorPosition == this.FinalPosition)
					{
						var offset = totalHorizontalMovement > 0 ? SimpleArea.DOUBLE_COLLISION_EPSILON : -SimpleArea.DOUBLE_COLLISION_EPSILON;
						var offSetPosition = this.FinalPosition + new Vector2(offset, 0);
						if (this.CollisionAtCandidatePosition(offSetPosition))
						{
							var remainingVerticalMovement = candidatePosition.Y - this.FinalPosition.Y;
							this.GetMovementCollisionResult(0, remainingVerticalMovement);
						}
						else
						{
							var remainingHorizontalMovement = candidatePosition.X - this.FinalPosition.X;
							this.GetMovementCollisionResult(remainingHorizontalMovement, 0);
						}
					}
					else
					{
						if (this.FinalPosition.X != priorPosition.X)
						{
							var remainingVerticalMovement = candidatePosition.Y - this.FinalPosition.Y;
							this.GetMovementCollisionResult(0, remainingVerticalMovement);
						}
						else
						{
							var remainingHorizontalMovement = candidatePosition.X - this.FinalPosition.X;
							this.GetMovementCollisionResult(remainingHorizontalMovement, 0);
						}
					}
				}

				return true;
			}

			return false;
		}

		/// <summary>
		/// Searches for the final position valid position between the current position and candidate position with a bisection search.
		/// </summary>
		/// <param name="candidatePosition">The candidate position.</param>
		/// <returns>The final position.</returns>
		private Vector2 BisectSearchForFinalPosition(Vector2 candidatePosition)
		{
			var attemptedHorizontalMovement = candidatePosition.X - this.FinalPosition.X;
			var attemptedVerticalMovement = candidatePosition.Y - this.FinalPosition.Y;

			if (attemptedHorizontalMovement == 0 && attemptedVerticalMovement == 0)
			{
				return this.FinalPosition;
			}

			int numberOfPossibleSteps;

			if (Math.Abs(attemptedHorizontalMovement) >= Math.Abs(attemptedVerticalMovement))
			{
				numberOfPossibleSteps = (int)Math.Abs(Math.Floor(attemptedHorizontalMovement / SimpleArea.DOUBLE_COLLISION_EPSILON));
			}
			else
			{
				numberOfPossibleSteps = (int)Math.Abs(Math.Floor(attemptedVerticalMovement / SimpleArea.DOUBLE_COLLISION_EPSILON));
			}

			if (Math.Abs(numberOfPossibleSteps) < 2)
			{
				return this.FinalPosition;
			}

			var horizontalStep = attemptedHorizontalMovement / numberOfPossibleSteps;
			var verticalStep = attemptedVerticalMovement / numberOfPossibleSteps;
			int low = 0, high = numberOfPossibleSteps, currentStep;
			Vector2 newCandidatePosition;
			Vector2 borderingNewCandidatePosition;
			bool currentStepCollision;
			bool borderingStepCollision;

			while (low < high)
			{
				currentStep = (low + high) / 2;
				newCandidatePosition = this.FinalPosition + new Vector2(horizontalStep * currentStep, verticalStep * currentStep);
				borderingNewCandidatePosition = this.FinalPosition + new Vector2(horizontalStep * (currentStep + 1), verticalStep * (currentStep + 1));

				currentStepCollision = this.CollisionAtCandidatePosition(newCandidatePosition);
				borderingStepCollision = this.CollisionAtCandidatePosition(borderingNewCandidatePosition);

				if (!currentStepCollision && !borderingStepCollision)
				{
					low = currentStep + 1;
				}
				else if (currentStepCollision)
				{
					high = currentStep;
				}
				else
				{
					return newCandidatePosition;
				}
			}

			return this.FinalPosition;
		}

		/// <summary>
		/// Checks the candidate position for collisions.
		/// </summary>
		/// <param name="candidatePosition">The current position.</param>
		/// <returns>A value indicating if there is a collision at the candidate position.</returns>
		private bool CollisionAtCandidatePosition(Vector2 candidatePosition)
		{
			var topRight = candidatePosition;
			if (this.MovingCollisionArea is OffsetCollisionArea offsetArea)
			{
				topRight += new Vector2(offsetArea.Area.HorizontalOffset, offsetArea.Area.VerticalOffset);
			}

			var bottomRight = topRight + new Vector2(this.MovingCollisionArea.Area.Width, this.MovingCollisionArea.Area.Height);
			for (var row = (int)candidatePosition.Y / Tile.TILE_DIMENSIONS; row <= Math.Ceiling(bottomRight.Y / Tile.TILE_DIMENSIONS); row++)
			{
				for (var col = (int)candidatePosition.X / Tile.TILE_DIMENSIONS; col <= Math.Ceiling(bottomRight.X / Tile.TILE_DIMENSIONS); col++)
				{
					if (this.TileMapLayer.TryGetTileCollision(row, col, out var tileCollisionArea) && tileCollisionArea.Intersects(this.MovingCollisionArea, out var intersectedMovementTerrainTypes, candidatePosition))
					{
						this.TerrainCollision = true;

						return true;
					}
				}
			}

			return false;
		}
	}
}
