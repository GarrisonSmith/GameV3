using DiscModels.Engine.Entities;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using Engine.Core.Base;
using Engine.Drawing.Base;
using Engine.Entities.Base.interfaces;
using Engine.Physics.Areas;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Base.enums;
using Engine.Physics.Collisions.interfaces;
using Engine.Saving.Base.interfaces;
using Engine.TileMapping.Base;
using System;
using System.Linq;

namespace Engine.Entities.Base
{
	/// <summary>
	/// Represents a entity.
	/// </summary>
	public class Entity : UpdateableAnimatedContent, IAmAEntity, ICanBeSaved<EntityModel<IAmAAreaModel, IAmACollisionAreaModel>>
	{
		/// <summary>
		/// Gets or sets the layer.
		/// </summary>
		public ushort Layer { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name { get; set; }

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
		/// Gets the tile map layer.
		/// </summary>
		public TileMapLayer TileMapLayer { get => Managers.TileManager?.ActiveTileMap?.Layers[this.Layer]; }

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
		/// <param name="position"></param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="entityModel">The entity model.</param>
		public Entity(bool updatingActivated, bool drawingActivated, ushort updateOrder, ushort drawOrder, Position position, IAmAArea area, IAmACollisionArea collisionArea, EntityModel<IAmAAreaModel, IAmACollisionAreaModel> entityModel)
			: base(updatingActivated, drawingActivated, updateOrder, drawOrder, position, area, new Animation(entityModel.Animations[entityModel.Orientation]))
		{
			this.Layer = entityModel.Layer;
			this.Name = entityModel.Name;
			this.Orientation = (OrientationTypes)entityModel.Orientation;
			this.MoveSpeed = new MoveSpeed(entityModel.MoveSpeed);
			this.CollisionArea = collisionArea;
			this.Animations = entityModel.Animations.Select(x => new Animation(x)).ToArray();
			Managers.EntityManager.Entities.Add(this.Guid, this);
			Managers.EntityManager.ControlledEntity = new ControlledEntity(this);
		}

		/// <summary>
		/// Initializes a new instance of the Entity class.
		/// </summary>
		/// <param name="updateOrder">A value indicating whether the content is updating.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="updatingActivated">The update order.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="orientation">The orientation.</param>
		/// <param name="moveSpeed">THe move speed.</param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="animations">The animations.</param>
		/// <param name="layer">The layer.</param>
		public Entity(bool updatingActivated, bool drawingActivated, ushort updateOrder, ushort drawOrder, OrientationTypes orientation, MoveSpeed moveSpeed, Position position, IAmAArea area, IAmACollisionArea collisionArea, Animation animation, Animation[] animations, ushort layer, string name)
			: base(updatingActivated, drawingActivated, updateOrder, drawOrder, position, area, animation)
		{
			this.Layer = layer;
			this.Name = name;
			this.Orientation = orientation;
			this.MoveSpeed = moveSpeed;
			this.CollisionArea = collisionArea;
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
				this.Animation = this.Animations[(ushort)this.Orientation];
				this.Animation.IsPlaying = false;

				return;
			}

			var horizontalMovementAmount = this.MoveSpeed.GetHorizontalMovementAmount(directionRadians.Value);
			var verticalMovementAmount = this.MoveSpeed.GetVerticalMovementAmount(directionRadians.Value);

			if (horizontalMovementAmount == 0 && verticalMovementAmount == 0)
			{
				this.Animation = this.Animations[(ushort)this.Orientation];
				this.Animation.IsPlaying = false;

				return;
			}

			var realHorizontalMovementAmount = horizontalMovementAmount;
			var realVerticalMovementAmount = verticalMovementAmount;

			if (forced || this.TileMapLayer == null)
			{
				this.Position.Y += verticalMovementAmount;
				this.Position.X += horizontalMovementAmount;
			}
			else
			{
				var collisionInfo = new CollisionInformation(this.CollisionArea, this.TileMapLayer, this.Position.Coordinates, directionRadians.Value, horizontalMovementAmount, verticalMovementAmount);
				realHorizontalMovementAmount = collisionInfo.FinalPosition.X - this.Position.Coordinates.X;
				realVerticalMovementAmount = collisionInfo.FinalPosition.Y - this.Position.Coordinates.Y;
				this.Position.Coordinates = collisionInfo.FinalPosition;
			}

			if (realHorizontalMovementAmount == 0 && realVerticalMovementAmount == 0)
			{
				this.Animation = this.Animations[(ushort)this.Orientation];
				this.Animation.IsPlaying = false;

				return;
			}

			if (Math.Abs(realHorizontalMovementAmount) >= Math.Abs(realVerticalMovementAmount) - SimpleArea.COLLISION_EPSILON)
			{
				if (realHorizontalMovementAmount >= 0)
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
				if (realVerticalMovementAmount >= 0)
				{
					this.Orientation = OrientationTypes.Downward;
				}
				else
				{
					this.Orientation = OrientationTypes.Upward;
				}
			}

			this.Animation = this.Animations[(ushort)this.Orientation];
			this.Animation.IsPlaying = true;
		}

		/// <summary>
		/// Disposes the entity.
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			this.MoveSpeed.Dispose();
			Managers.EntityManager.Entities.Remove(this.Guid);
			Managers.DebuggingManager.CollisionTextures.Remove(this);
		}

		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		public EntityModel<IAmAAreaModel, IAmACollisionAreaModel> ToModel()
		{
			return new EntityModel<IAmAAreaModel, IAmACollisionAreaModel>
			{
				Layer = this.Layer,
				Name = this.Name,
				Orientation = (ushort)this.Orientation,
				MoveSpeed = this.MoveSpeed.ToModel(),
				Position = this.Position.ToModel(),
				Animations = this.Animations.Select(x => x.ToModel()).ToArray(),
				Area = Managers.PhysicsManager.GetAreaModel(this.Area),
				CollisionArea = Managers.PhysicsManager.GetCollisionAreaModel(this.CollisionArea)
			};
		}
	}
}
