using Engine.Core.Base;
using Engine.Drawing.Base;
using Engine.Physics.Areas.interfaces;
using Engine.Physics.Collisions.interfaces;
using Engine.TileMapping.Base.interfaces;

namespace Engine.TileMapping.Base.Tiles
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public class Tile : DrawableContent, IAmATile
    {
        /// <summary>
        /// Gets the tile dimensions.
        /// </summary>
        public static int TILE_DIMENSIONS { get => 64; }

        /// <summary>
        /// Gets or sets the collision area.
        /// </summary>
		public IAmACollisionArea CollisionArea { get; set; }

		/// <summary>
		/// Initializes a new instance of the Tile class.
		/// </summary>
		/// <param name="drawingActivated">A value indicating whether the content is drawing.</param>
		/// <param name="drawOrder">The draw order.</param>
		/// <param name="area">The area.</param>
		/// <param name="collisionArea">The collision area.</param>
		/// <param name="drawData">The draw data.</param>
		public Tile(bool drawingActivated, ushort drawOrder, IAmAArea area, IAmACollisionArea collisionArea, DrawData drawData)
            : base(drawingActivated, drawOrder, area, drawData)
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
        }
	}
}
