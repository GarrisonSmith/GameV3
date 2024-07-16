using DiscModels.Engine.Entities;
using DiscModels.Engine.Physics.Areas.interfaces;
using DiscModels.Engine.Physics.Collisions.interfaces;
using DiscModels.Engine.TileMapping;
using Engine.Entities.Base;
using Engine.TileMapping.Base;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Engine.Saving
{
	/// <summary>
	/// Represents a saving manager.
	/// </summary>
	public class SavingManager
	{
		/// <summary>
		/// Starts the load manager.
		/// </summary>
		/// <returns>The load manager.</returns>
		public static SavingManager StartSavingManager()
		{
			return Managers.SavingManager ?? new SavingManager();
		}

		/// <summary>
		/// Initializes a new instance of the SavingManager class.
		/// </summary>
		public SavingManager()
		{ 
		
		}

		/// <summary>
		/// Saves the tile map.
		/// </summary>
		/// <param name="tileMap">The tile map.</param>
		public void SaveTileMap(TileMap tileMap)
		{
			if (null == tileMap)
			{
				return;
			}

			var tileMapModel = tileMap.ToModel();
			var tileMapJson = SerializeToJson<TileMapModel>(tileMapModel);
			var tileMapPath = Path.Combine(Managers.Game.Content.RootDirectory, "TileMaps", tileMap.Name + ".json");
			File.WriteAllText(tileMapPath, tileMapJson);
		}

		/// <summary>
		/// Saves the entity.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public void SaveEntity(Entity entity)
		{
			if (null == entity)
			{
				return;
			}

			var entityModel = entity.ToModel();
			var entityJson = SerializeToJson<EntityModel<IAmAAreaModel, IAmACollisionAreaModel>>(entityModel);
			var entityPath = Path.Combine(Managers.Game.Content.RootDirectory, "Entities", entity.Name + ".json");
			File.WriteAllText(entityPath, entityJson);
		}

		/// <summary>
		/// Serializes the object to JSON.
		/// </summary>
		/// <typeparam name="T">The model type to be serialized to.</typeparam>
		/// <param name="param">The param.</param>
		/// <returns>The object JSON.</returns>
		private string SerializeToJson<T>(object param)
		{
			using var stream = new MemoryStream();
			var serializer = new DataContractJsonSerializer(typeof(T), DiscModels.DiskTypes.GetDiscTypes());
			serializer.WriteObject(stream, param);
			stream.Position = 0;
			using var reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}
	}
}
