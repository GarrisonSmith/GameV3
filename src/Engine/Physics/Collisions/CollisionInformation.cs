using Engine.Entities.Base.interfaces;
using Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Collisions.enums;
using Engine.TileMapping.Base.Tiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.Physics.Base
{
	/// <summary>
	/// Represents information about a collision.
	/// </summary>
	public class CollisionInformation
	{
		private float? largestXCollision;
		private float? smallestXCollision;
		private float? largestYCollision;
		private float? smallestYCollision;

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
		/// Gets a value indicating whether a horizontal collision has occurred.
		/// </summary>
		public bool HorizontalCollision { get => this.LargestXCollision.HasValue || this.SmallestXCollision.HasValue; }

		/// <summary>
		/// Gets a value indicating whether a vertical collision has occurred.
		/// </summary>
		public bool VerticalCollision { get => this.LargestYCollision.HasValue || this.SmallestYCollision.HasValue; }

		/// <summary>
		/// Gets or sets the largest X collision.
		/// </summary>
		public float? LargestXCollision
		{
			get => this.largestXCollision;
			set
			{
				if (!value.HasValue || !this.LargestXCollision.HasValue || value > this.LargestXCollision)
				{
					this.largestXCollision = value;
				}

				if (value >= this.LargestXCollision && this.CornerCollision.HasValue && this.CornerCollision.Value.X == this.LargestXCollision)
				{
					if (this.LargestYCollision == this.CornerCollision.Value.Y)
					{
						this.largestYCollision = null;
					}
					else if (this.SmallestYCollision == this.CornerCollision.Value.Y)
					{
						this.smallestYCollision = null;
					}

					this.CornerCollision = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the smallest X collision.
		/// </summary>
		public float? SmallestXCollision
		{
			get => this.smallestXCollision;
			set
			{
				if (!value.HasValue || !this.SmallestXCollision.HasValue || value < this.SmallestXCollision)
				{
					this.smallestXCollision = value;
				}

				if (value <= this.SmallestXCollision && this.CornerCollision.HasValue && this.CornerCollision.Value.X == this.SmallestXCollision)
				{
					if (this.LargestYCollision == this.CornerCollision.Value.Y)
					{
						this.largestYCollision = null;
					}
					else if (this.SmallestYCollision == this.CornerCollision.Value.Y)
					{
						this.smallestYCollision = null;
					}

					this.CornerCollision = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the largest Y collision.
		/// </summary>
		public float? LargestYCollision
		{
			get => this.largestYCollision;
			set
			{
				if (!value.HasValue || !this.LargestYCollision.HasValue || value > this.LargestYCollision)
				{
					this.largestYCollision = value;
				}

				if (value >= this.LargestYCollision && this.CornerCollision.HasValue && this.CornerCollision.Value.Y == this.LargestYCollision)
				{
					if (this.LargestXCollision == this.CornerCollision.Value.X)
					{
						this.largestXCollision = null;
					}
					else if (this.SmallestXCollision == this.CornerCollision.Value.X)
					{
						this.smallestXCollision = null;
					}

					this.CornerCollision = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the smallest Y collision.
		/// </summary>
		public float? SmallestYCollision
		{
			get => this.smallestYCollision;
			set
			{
				if (!value.HasValue || !this.SmallestYCollision.HasValue || value < SmallestYCollision)
				{
					this.smallestYCollision = value;
				}

				if (value <= this.SmallestYCollision && this.CornerCollision.HasValue && this.CornerCollision.Value.Y == this.SmallestYCollision)
				{
					if (this.LargestXCollision == this.CornerCollision.Value.X)
					{
						this.largestXCollision = null;
					}
					else if (this.SmallestXCollision == this.CornerCollision.Value.X)
					{
						this.smallestXCollision = null;
					}

					this.CornerCollision = null;
				}
			}
		}

		/// <summary>
		/// Gets or sets the corner collision.
		/// </summary>
		private Vector2? CornerCollision { get; set; }

		/// <summary>
		/// Gets or sets the final position.
		/// </summary>
		public Vector2 FinalPosition { get; set; }

		/// <summary>
		/// Gets or sets the moving entity.
		/// </summary>
		public IAmAEntity MovingEntity { get; set; }

		/// <summary>
		/// Gets or sets the movement terrain types that were collided with.
		/// </summary>
		public List<MovementTerrainTypes> MovementTerrainTypesCollidedWith { get; private set; }

		/// <summary>
		/// Initializes a new instance of the CollisionInformation class.
		/// </summary>
		/// <param name="movingEntity">The moving entity.</param>
		/// <param name="startingPosition">The starting position.</param>
		/// <param name="totalHorizontalMovement">The total horizontal movement.</param>
		/// <param name="totalVerticalMovement">The total vertical movement.</param>
		public CollisionInformation(IAmAEntity movingEntity, Vector2 startingPosition, float totalHorizontalMovement, float totalVerticalMovement)
		{
			this.MovingEntity = movingEntity;
			this.TerrainCollision = false;
			this.EventCollision = false;
			this.EntityCollision = false;
			this.MovementTerrainTypesCollidedWith = new List<MovementTerrainTypes>();
			this.FinalPosition = startingPosition;
			this.FinalPosition = this.GetMovementCollisionResult(startingPosition, totalHorizontalMovement, totalVerticalMovement);
		}

		/// <summary>
		/// Adds the distinct movement terrain type.
		/// </summary>
		/// <param name="movementTerrainType">The movement terrain type.</param>
		public void AddMovementTerrainTypeIfDistinct(MovementTerrainTypes movementTerrainType)
		{
			if (!this.MovementTerrainTypesCollidedWith.Contains(movementTerrainType))
			{
				this.MovementTerrainTypesCollidedWith.Add(movementTerrainType);
			}
		}

		/// <summary>
		/// Adds the distinct movement terrain types.
		/// </summary>
		/// <param name="movementTerrainTypes">The movement terrain types.</param>
		public void AddMovementTerrainTypesIfDistinct(List<MovementTerrainTypes> movementTerrainTypes)
		{
			foreach (var movementTerrainType in movementTerrainTypes)
			{
				if (!this.MovementTerrainTypesCollidedWith.Contains(movementTerrainType))
				{
					this.MovementTerrainTypesCollidedWith.Add(movementTerrainType);
				}
			}
		}

		/// <summary>
		/// Updates the collision bounds.
		/// </summary>
		/// <param name="area">The area.</param>
		/// <param name="candidatePositionTopLeft">The candidate position top left.</param>
		public void UpdateCollisionBounds(IAmAArea area, Vector2 candidatePositionTopLeft)
		{
			if (area is ComplexArea complexArea)
			{
				foreach (var subArea in complexArea.OffsetAreas)
				{
					this.UpdateCollisionBounds(subArea, candidatePositionTopLeft);
				}

				return;
			}

			//get movement amounts
			var horizontalMovement = candidatePositionTopLeft.X - this.MovingEntity.CollisionArea.Area.X;
			var verticalMovement = candidatePositionTopLeft.Y - this.MovingEntity.CollisionArea.Area.Y;
			bool horizontalMovementOkay = true;
			bool verticalMovementOkay = true;

			//check horizontal movement
			if (horizontalMovement != 0)
			{
				var horizontalCandidatePosition = new Vector2(this.MovingEntity.CollisionArea.Area.X + horizontalMovement, this.MovingEntity.CollisionArea.Area.Y);
				horizontalMovementOkay = !area.Intersects(this.MovingEntity.CollisionArea.Area, horizontalCandidatePosition);
			}

			//check vertical movement
			if (verticalMovement != 0)
			{
				var verticalCandidatePosition = new Vector2(this.MovingEntity.CollisionArea.Area.X, this.MovingEntity.CollisionArea.Area.Y + verticalMovement);
				verticalMovementOkay = !area.Intersects(this.MovingEntity.CollisionArea.Area, verticalCandidatePosition);
			}

			//apply logic based on results
			if (horizontalMovementOkay && !verticalMovementOkay)
			{
				if (this.MovingEntity.CollisionArea.Area.BottomRight.Y <= area.TopLeft.Y)
				{
					//moving area is above area
					this.SmallestYCollision = area.TopLeft.Y;
				}
				else if (this.MovingEntity.CollisionArea.Area.TopLeft.Y >= area.BottomRight.Y)
				{
					//moving area is below area
					this.LargestYCollision = area.BottomRight.Y;
				}
			}
			else if (!horizontalMovementOkay && verticalMovementOkay)
			{
				if (this.MovingEntity.CollisionArea.Area.BottomRight.X <= area.TopLeft.X)
				{
					//moving area is right of area
					this.SmallestXCollision = area.TopLeft.X;
				}
				else if (this.MovingEntity.CollisionArea.Area.TopLeft.X >= area.BottomRight.X)
				{
					//moving area is left of area
					this.LargestXCollision = area.BottomRight.X;
				}
			}
			else if (horizontalMovementOkay && verticalMovementOkay)
			{
				//Since we know a intersection occurs on the final position for this method to execute we can just apply both a horizontal and vertical collision.
				//This is for when a rectangle is moving perfectly diagonal toward the area. 
				float? existingX = null;
				float? existingY = null;

				if (this.SmallestXCollision.HasValue)
				{
					existingX = this.SmallestXCollision.Value;
				}
				else if (this.LargestXCollision.HasValue)
				{
					existingX = this.LargestXCollision.Value;
				}

				if (this.SmallestYCollision.HasValue)
				{
					existingY = this.SmallestYCollision.Value;
				}
				else if (this.LargestYCollision.HasValue)
				{
					existingY = this.LargestYCollision.Value;
				}

				var cornerCollision = new Vector2();
				if (this.MovingEntity.CollisionArea.Area.BottomRight.X <= area.TopLeft.X)
				{
					//moving area is right of area
					this.SmallestXCollision = area.TopLeft.X;
					cornerCollision.X = area.TopLeft.X;
				}
				else if (this.MovingEntity.CollisionArea.Area.TopLeft.X >= area.BottomRight.X)
				{
					//moving area is left of area
					this.LargestXCollision = area.BottomRight.X;
					cornerCollision.X = area.BottomRight.X;
				}

				if (this.MovingEntity.CollisionArea.Area.BottomRight.Y <= area.TopLeft.Y)
				{
					//moving area is above area
					this.SmallestYCollision = area.TopLeft.Y;
					cornerCollision.Y = area.TopLeft.Y;
				}
				else if (this.MovingEntity.CollisionArea.Area.TopLeft.Y >= area.BottomRight.Y)
				{
					//moving area is below area
					this.LargestYCollision = area.BottomRight.Y;
					cornerCollision.Y = area.BottomRight.Y;
				}

				if (existingX.HasValue && (this.SmallestXCollision == existingX || this.LargestXCollision == existingX) && 
					!(existingY.HasValue && (this.SmallestYCollision == existingY || this.LargestYCollision == existingY)))
				{
					this.SmallestYCollision = null;
					this.LargestYCollision = null;

					return;
				}

				if (existingY.HasValue && (this.SmallestYCollision == existingY || this.LargestYCollision == existingY) && 
					!(existingX.HasValue && (this.SmallestXCollision == existingX || this.LargestXCollision == existingX)))
				{
					this.SmallestXCollision = null;
					this.LargestXCollision = null;

					return;
				}

				this.CornerCollision = cornerCollision;
			}
			else if (!horizontalMovementOkay && !verticalMovementOkay)
			{
				//this condition should not be possible as a rectangle can only collide with another rectangle on one side.
			}
		}

		/// <summary>
		/// Get the movement collision result.
		/// </summary>
		/// <param name="startingPosition">The starting position.</param>
		/// <param name="totalHorizontalMovement">The total horizontal movement.</param>
		/// <param name="totalVerticalMovement">The total vertical movement.</param>
		/// <returns></returns>
		private Vector2 GetMovementCollisionResult(Vector2 startingPosition, float totalHorizontalMovement, float totalVerticalMovement)
		{
			var horizontalStep = totalHorizontalMovement / (float)Math.Sqrt(Math.Pow(totalHorizontalMovement, 2) + Math.Pow(totalVerticalMovement, 2));
			var verticalStep = totalVerticalMovement / (float)Math.Sqrt(Math.Pow(totalHorizontalMovement, 2) + Math.Pow(totalVerticalMovement, 2));
			var currentPosition = startingPosition;
			Vector2 candidatePositionTopLeft;
			Vector2 candidatePositionBottomRight;

			//Checks the positions between the current position and the destination position.
			int numberOfSteps = 1;
			while ((totalHorizontalMovement != 0 && Math.Abs(horizontalStep * numberOfSteps) < Math.Abs(totalHorizontalMovement)) ||
				   (totalVerticalMovement != 0 && Math.Abs(verticalStep * numberOfSteps) < Math.Abs(totalVerticalMovement)))
			{
				candidatePositionTopLeft = new Vector2(currentPosition.X, currentPosition.Y);
				if (horizontalStep != 0 && !this.HorizontalCollision)
				{
					candidatePositionTopLeft.X += horizontalStep;
				}

				if (verticalStep != 0 && !this.VerticalCollision)
				{
					candidatePositionTopLeft.Y += verticalStep;
				}

				candidatePositionBottomRight = new Vector2(candidatePositionTopLeft.X + this.MovingEntity.CollisionArea.Area.Width, candidatePositionTopLeft.Y + this.MovingEntity.CollisionArea.Area.Height);
				currentPosition = this.CheckCandidatePosition(currentPosition, candidatePositionTopLeft, candidatePositionBottomRight);

				//Check further vertical movement if only horizontal collision.
				if (this.HorizontalCollision && !this.VerticalCollision && totalHorizontalMovement != 0 && Math.Abs(totalVerticalMovement - verticalStep * numberOfSteps) > SimpleArea.COLLISION_EPSILON)
				{
					var subVerticalCollisionArea = new CollisionInformation(this.MovingEntity, currentPosition, 0, totalVerticalMovement - verticalStep * numberOfSteps);
					this.SmallestYCollision = subVerticalCollisionArea.SmallestYCollision;
					this.LargestYCollision = subVerticalCollisionArea.LargestYCollision;
					currentPosition = subVerticalCollisionArea.FinalPosition;
				}

				//Check further horizontal movement if only vertical collision.
				if (!this.HorizontalCollision && this.VerticalCollision && totalVerticalMovement != 0 && Math.Abs(totalHorizontalMovement - horizontalStep * numberOfSteps) > SimpleArea.COLLISION_EPSILON)
				{
					var subHorizontalCollisionArea = new CollisionInformation(this.MovingEntity, currentPosition, totalHorizontalMovement - horizontalStep * numberOfSteps, 0);
					this.SmallestXCollision = subHorizontalCollisionArea.SmallestXCollision;
					this.LargestXCollision = subHorizontalCollisionArea.LargestXCollision;
					currentPosition = subHorizontalCollisionArea.FinalPosition;
				}

				if (this.MovementCollision)
				{
					return currentPosition;
				}

				numberOfSteps++;
			}

			//Checks the final position. We need to do this separately from the loop above the step increments don't always result in the final position cleanly.
			candidatePositionTopLeft = new Vector2(this.MovingEntity.CollisionArea.Area.X + totalHorizontalMovement, this.MovingEntity.CollisionArea.Area.Y + totalVerticalMovement);
			candidatePositionBottomRight = new Vector2(candidatePositionTopLeft.X + this.MovingEntity.CollisionArea.Area.Width, candidatePositionTopLeft.Y + this.MovingEntity.CollisionArea.Area.Height);
			currentPosition = this.CheckCandidatePosition(currentPosition, candidatePositionTopLeft, candidatePositionBottomRight);

			//Check further vertical movement if only horizontal collision.
			if (this.HorizontalCollision && totalHorizontalMovement != 0 && !this.VerticalCollision && totalVerticalMovement != 0)
			{
				var subVerticalCollisionArea = new CollisionInformation(this.MovingEntity, currentPosition, 0, totalVerticalMovement - verticalStep * numberOfSteps);
				this.SmallestYCollision = subVerticalCollisionArea.SmallestYCollision;
				this.LargestYCollision = subVerticalCollisionArea.LargestYCollision;
				currentPosition = subVerticalCollisionArea.FinalPosition;

				return currentPosition;
			}

			//Check further horizontal movement if only vertical collision.
			if (!this.HorizontalCollision && this.VerticalCollision && totalVerticalMovement != 0 && totalHorizontalMovement != 0)
			{
				var subHorizontalCollisionArea = new CollisionInformation(this.MovingEntity, currentPosition, totalHorizontalMovement - horizontalStep * numberOfSteps, 0);
				this.SmallestXCollision = subHorizontalCollisionArea.SmallestXCollision;
				this.LargestXCollision = subHorizontalCollisionArea.LargestXCollision;
				currentPosition = subHorizontalCollisionArea.FinalPosition;

				return currentPosition;
			}

			if (this.MovementCollision)
			{
				return currentPosition;
			}

			return candidatePositionTopLeft;
		}

		/// <summary>
		/// Checks the candidate position.
		/// </summary>
		/// <param name="currentPosition">The current position.</param>
		/// <param name="candidatePositionTopLeft">The candidate top left position.</param>
		/// <param name="candidatePositionBottomRight">the candidate bottom right position.</param>
		/// <returns>The new final position.</returns>
		private Vector2 CheckCandidatePosition(Vector2 currentPosition, Vector2 candidatePositionTopLeft, Vector2 candidatePositionBottomRight)
		{
			for (var row = (int)candidatePositionTopLeft.Y / Tile.TILE_DIMENSIONS; row <= Math.Ceiling(candidatePositionBottomRight.Y / Tile.TILE_DIMENSIONS); row++)
			{
				for (var col = (int)candidatePositionTopLeft.X / Tile.TILE_DIMENSIONS; col <= Math.Ceiling(candidatePositionBottomRight.X / Tile.TILE_DIMENSIONS); col++)
				{
					if (this.MovingEntity.TileMapLayer.TryGetTileCollision(row, col, out var tileCollisionArea))
					{
						tileCollisionArea.GetCollisionInformation(this.MovingEntity, candidatePositionTopLeft, this);
					}
				}
			}

			if (this.HorizontalCollision)
			{
				currentPosition.X = this.GetHorizontalFlushCollision(candidatePositionTopLeft.X);
			}
			else
			{
				currentPosition.X = candidatePositionTopLeft.X;
			}

			if (this.VerticalCollision)
			{
				currentPosition.Y = this.GetVerticalFlushCollision(candidatePositionTopLeft.Y);
			}
			else
			{
				currentPosition.Y = candidatePositionTopLeft.Y;
			}

			return currentPosition;
		}

		/// <summary>
		/// Gets the horizontal flush collision.
		/// </summary>
		/// <param name="candidatePositionTopLeftX">The candidate position top left x.</param>
		/// <returns>The flush collision x value.</returns>
		private float GetHorizontalFlushCollision(float candidatePositionTopLeftX)
		{
			if (this.SmallestXCollision.HasValue)
			{
				//Colliding with something to the right
				candidatePositionTopLeftX = this.SmallestXCollision.Value - this.MovingEntity.CollisionArea.Area.Width - SimpleArea.DOUBLE_COLLISION_EPSILON;
			}
			else if (this.LargestXCollision.HasValue)
			{
				//Colliding with something to the left
				candidatePositionTopLeftX = this.LargestXCollision.Value + SimpleArea.DOUBLE_COLLISION_EPSILON;
			}

			return candidatePositionTopLeftX;
		}

		/// <summary>
		/// Gets the vertical flush collision.
		/// </summary>
		/// <param name="candidatePositionTopLeftY">The candidate position top left y.</param>
		/// <returns>The flush collision y value.</returns>
		private float GetVerticalFlushCollision(float candidatePositionTopLeftY)
		{
			if (this.SmallestYCollision.HasValue)
			{
				//Colliding with something below
				candidatePositionTopLeftY = this.SmallestYCollision.Value - this.MovingEntity.CollisionArea.Area.Height - SimpleArea.DOUBLE_COLLISION_EPSILON;
			}
			else if (this.LargestYCollision.HasValue)
			{
				//Colliding with something above
				candidatePositionTopLeftY = this.LargestYCollision.Value + SimpleArea.DOUBLE_COLLISION_EPSILON;
			}

			return candidatePositionTopLeftY;
		}
	}
}
