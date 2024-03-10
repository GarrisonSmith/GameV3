using Engine.Core.Base;
using Engine.Drawing.Base;
using Engine.Entities.Base.interfaces;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Base.enums;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base;
using System;

namespace Engine.Entities.Base
{
	/// <summary>
	/// Represents a entity.
	/// </summary>
	public class Entity : UpdateableAnimatedContent, IAmAEntity
	{
		/// <summary>
		/// Gets the layer.
		/// </summary>
		public ushort Layer { get => this.TileMapLayer.Layer; }

		/// <summary>
		/// Gets or sets the move speed.
		/// </summary>
		public MoveSpeed MoveSpeed { get; set; }

		/// <summary>
		/// Gets or sets the orientation.
		/// </summary>
		public OrientationTypes Orientation { get; set; }

		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		public IAmACollisionArea CollisionArea { get; set; }

		/// <summary>
		/// Gets or sets the tile map layer.
		/// </summary>
		public TileMapLayer TileMapLayer { get; set; }

		/// <summary>
		/// Gets or sets the animations.
		/// The 0th index is the downward animation, the 1st index is the right animation,
		/// the 2nd index is the left animation, the 3rd index is the upward animation.
		/// </summary>
		public Animation[] Animations { get; set; }

		/// <summary>
		/// Initializes a new instance of the Entity class.
		/// </summary>
		/// <param name="updateOrder">A value indicating whether the content is updating.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="updatingActivated">The update order.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="orientation">The orientation.</param>
		/// <param name="moveSpeed">THe move speed.</param>
		/// <param name="position">The position.</param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="animations">The animations.</param>
		/// <param name="tileMapLayer">The tile map layer.</param>
		public Entity(bool updatingActivated, bool drawingActivated, ushort updateOrder, ushort drawOrder, OrientationTypes orientation, MoveSpeed moveSpeed, Position position, IAmAArea area, IAmACollisionArea collisionArea, Animation animation, Animation[] animations, TileMapLayer tileMapLayer)
			: base(updatingActivated, drawingActivated, updateOrder, drawOrder, position, area, animation)
		{
			this.Orientation = orientation;
			this.MoveSpeed = moveSpeed;
			this.CollisionArea = collisionArea;
			this.TileMapLayer = tileMapLayer;
			this.TileMapLayer.Entities.Add(this);
			this.Animations = animations;
			Managers.EntityManager.Entities.Add(this.Guid, this);
			Managers.EntityManager.ControlledEntity = new ControlledEntity(this);
		}

		/// <summary>
		/// Moves in the provided direction by the provided amount.
		/// </summary>
		/// <param name="directionRadians">The direction. A null value indicates no movement.</param>
		/// <param name="forced">A value indicating whether movement is forced.</param>
		public void Move(float? directionRadians, bool forced = false)
		{
			if (!directionRadians.HasValue)
			{
				this.Animation = this.Animations[(int)this.Orientation];
				this.Animation.IsPlaying = false;
				return;
			}

			var horizontalMovementAmount = this.MoveSpeed.GetHorizontalMovementAmount(directionRadians.Value);
			var verticalMovementAmount = this.MoveSpeed.GetVerticalMovementAmount(directionRadians.Value);
			if (horizontalMovementAmount == 0 && verticalMovementAmount == 0)
			{
				this.Animation = this.Animations[(int)this.Orientation];
				this.Animation.IsPlaying = false;
				return;
			}

			if (forced)
			{
				this.Position.Y += verticalMovementAmount;
				this.Position.X += horizontalMovementAmount;
			}
			else
			{
				var collisionInfo = new CollisionInformation(this, this.CollisionArea.Area.TopLeft, horizontalMovementAmount, verticalMovementAmount);
				//this.ProcessMovementTerrainTypesCollidedWith(collisionInfo);
				this.CollisionArea.Area.TopLeft = collisionInfo.FinalPosition;
			}

			if (Math.Abs(horizontalMovementAmount) >= Math.Abs(verticalMovementAmount) - .00001f)
			{
				if (horizontalMovementAmount >= 0)
				{
					this.Orientation = OrientationTypes.Right;
				}
				else
				{
					this.Orientation = OrientationTypes.Left;
				}
			}
			else
			{
				if (verticalMovementAmount >= 0)
				{
					this.Orientation = OrientationTypes.Downward;
				}
				else
				{
					this.Orientation = OrientationTypes.Upward;
				}
			}

			this.Animation = this.Animations[(int)this.Orientation];
			this.Animation.IsPlaying = true;
		}

		/// <summary>
		/// Process the movement terrain types collided with.
		/// </summary>
		/// <param name="collisionInformation">The collision information.</param>
		public void ProcessMovementTerrainTypesCollidedWith(CollisionInformation collisionInformation)
		{
			foreach (var movementCollisionType in collisionInformation.MovementTerrainTypesCollidedWith)
			{ 
				//TODO
			}
		}
	}
}
