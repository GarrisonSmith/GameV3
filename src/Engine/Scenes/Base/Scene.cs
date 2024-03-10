using Engine.Entities.Base.interfaces;
using Engine.TileMapping.Base;
using System.Collections.Generic;

namespace Engine.Scenes.Base
{
	/// <summary>
	/// Represents a scene.
	/// </summary>
	public class Scene
	{
		/// <summary>
		/// Gets or sets the entities.
		/// </summary>
		public List<IAmAEntity> Entities 
		{
			get
			{ 
				var entities = new List<IAmAEntity>();
				foreach (var tileMapLayer in this.TileMap.Layers.Values)
				{
					entities.AddRange(tileMapLayer.Entities);
				}

				return entities;
			}
		}

		/// <summary>
		/// Gets or sets the tile map.
		/// </summary>
		public TileMap TileMap { get; set; }
	}
}
