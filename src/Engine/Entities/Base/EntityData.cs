using System;

namespace Engine.Entities.Base
{
	/// <summary>
	/// Represents a entity data.
	/// </summary>
	public class EntityData
	{
		/// <summary>
		/// Gets or sets the guid.
		/// </summary>
		public Guid Guid { get; set; }

		/// <summary>
		/// Gets or sets the entity name.
		/// </summary>
		public string EntityName { get; set; }

		/// <summary>
		/// Initializes a new instance of the EntityData class.
		/// </summary>
		/// <param name="entityName">The entity name.</param>
		public EntityData(string entityName)
		{
			this.Guid = Guid.NewGuid();
			this.EntityName = entityName;
			Managers.EntityManager.EntityData.Add(this.Guid, this);
			Managers.EntityManager.EntityDataByEntityName.Add(this.EntityName, this);
		}
	}
}
