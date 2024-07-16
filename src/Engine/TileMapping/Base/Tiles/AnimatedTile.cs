using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping;
using DiscModels.Engine.TileMapping.interfaces;
using Engine.Core.Base;
using Engine.Drawing.Base;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Base;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base.interfaces;

namespace Engine.TileMapping.Base.Tiles
{
	/// <summary>
	/// Represents a animated tile.
	/// </summary>
	public class AnimatedTile : UpdateableAnimatedContent, IAmATile
    {
		/// <summary>
		/// Gets or sets the collision area.
		/// </summary>
		public IAmACollisionArea CollisionArea { get; set; }

		/// <summary>
		/// Initializes a new instance of the Tile class.
		/// </summary>
		/// <param name="updatingActivated">A value indicating whether the content is updating.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="updateOrder">The update order.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="position">The position.</param>
		/// <param name="area">The area.</param>
		/// <param name="collision">The collision.</param>
		/// <param name="animatedTileModel">The animated tile model.</param>
		public AnimatedTile(bool updatingActivated, bool drawingActivated, ushort updateOrder, ushort drawOrder, Position position, IAmAArea area, IAmACollisionArea collision, AnimatedTileModel<IAmAAreaModel, IAmACollisionAreaModel> animatedTileModel)
			: base(updatingActivated, drawingActivated, updateOrder, drawOrder, position, area, new Animation(animatedTileModel.Animation))
		{
			this.CollisionArea = collision;
			Managers.TileManager.Tiles.Add(this.Guid, this);
		}

		/// <summary>
		/// Initializes a new instance of the Tile class.
		/// </summary>
		/// <param name="updatingActivated">A value indicating whether the content is updating.</param>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="updateOrder">The update order.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="position">The position.</param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="animation">The animation.</param>
		public AnimatedTile(bool updatingActivated, bool drawingActivated, ushort updateOrder, ushort drawOrder, Position position, IAmAArea area, IAmACollisionArea collisionArea, Animation animation)
            : base(updatingActivated, drawingActivated, updateOrder, drawOrder, position, area, animation)
        {
			this.CollisionArea = collisionArea;
			Managers.TileManager.Tiles.Add(this.Guid, this);
		}

        /// <summary>
        /// Deactivates the tile.
        /// </summary>
        /// <param name="deactivateTileDrawing">A value indicating whether to deactivate tile drawing.</param>
        /// <param name="deactivateTileUpdating">A value indicating whether to deactivate tile updating.</param>
        public void DeactivateTile(bool deactivateTileUpdating = true, bool deactivateTileDrawing = true)
        {
            this.DrawingActivated = !deactivateTileDrawing;
            this.UpdatingActivated = !deactivateTileUpdating;
        }

		/// <summary>
		/// Disposes the tile.
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			Managers.TileManager.Tiles.Remove(this.Guid);
			Managers.DebuggingManager.CollisionTextures.Remove(this);
		}

		/// <summary>
		/// Creates the corresponding model.
		/// </summary>
		/// <returns>The corresponding model.</returns>
		public IAmATileModel<IAmAAreaModel, IAmACollisionAreaModel> ToModel()
		{
			return new AnimatedTileModel<IAmAAreaModel, IAmACollisionAreaModel>
			{
				Position = this.Position.ToModel(),
				Animation = this.Animation.ToModel(),
				Area = Managers.PhysicsManager.GetAreaModel(this.Area),
				CollisionArea = Managers.PhysicsManager.GetCollisionAreaModel(this.CollisionArea)
			};
		}
	}
}
