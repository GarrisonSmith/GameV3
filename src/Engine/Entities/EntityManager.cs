using Engine.Entities.Base;
using Engine.Entities.Base.interfaces;
using Engine.Loading.Base.interfaces;
using System;
using System.Collections.Generic;

namespace Engine.Entities
{
	/// <summary>
	/// Represents a entity manager.
	/// </summary>
	public class EntityManager : ICanBeLoaded
	{
		/// <summary>
		/// Start the entity manager.
		/// </summary>
		/// <returns>The entity manager.</returns>
		public static EntityManager StartEntityManager()
		{
			return Managers.EntityManager ?? new EntityManager();
		}

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the controlled entity.
		/// </summary>
		public ControlledEntity ControlledEntity { get; set; }

		/// <summary>
		/// Gets or sets the entities.
		/// </summary>
		public Dictionary<Guid, IAmAEntity> Entities { get; set; }

		/// <summary>
		/// Gets or sets the entity data.
		/// </summary>
		public Dictionary<Guid, EntityData> EntityData { get; set; }

		/// <summary>
		/// Gets or sets the entity data by entity name.
		/// </summary>
		public Dictionary<string, EntityData> EntityDataByEntityName { get; set; }

		/// <summary>
		/// Initializes a new instance of the EntityManager class.
		/// </summary>
		public EntityManager()
		{
			this.Entities = new();
			this.EntityData = new();
			this.EntityDataByEntityName = new();
			this.IsLoaded = false;
		}

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			Managers.LoadManager.LoadEntityManager();
			this.IsLoaded = true;
		}
	}
}
