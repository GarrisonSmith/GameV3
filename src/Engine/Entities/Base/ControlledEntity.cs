using Engine.Entities.Base.interfaces;

namespace Engine.Entities.Base
{
	/// <summary>
	/// Represents a controlled entity.
	/// </summary>
	public class ControlledEntity
	{
		/// <summary>
		/// Gets or sets the entity.
		/// </summary>
		public IAmAEntity Entity { get; set; }


		/// <summary>
		/// Initializes a new instance of the ControlledEntity class.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public ControlledEntity(IAmAEntity entity)
		{ 
			this.Entity = entity;
		}
	}
}
