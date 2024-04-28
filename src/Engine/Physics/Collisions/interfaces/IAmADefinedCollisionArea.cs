using Engine.Physics.Areas.interfaces;
using System.Collections;

namespace Engine.Physics.Collisions.interfaces
{
	public interface IAmADefinedCollisionArea : IAmACollisionArea, IHaveArea
	{       
		/// <summary>
		/// Gets the collision area.
		/// </summary>
		new IAmADefinedArea Area { get; }

		/// <summary>
		/// Gets the movement terrain types.
		/// </summary>
		IEnumerable MovementTerrainTypes { get; }
	}
}
