namespace Engine.Physics.Base.interfaces
{
	/// <summary>
	/// Represents something that can move.
	/// </summary>
	public interface ICanMove
	{
		/// <summary>
		/// Gets the move speed.
		/// </summary>
		MoveSpeed MoveSpeed { get; }

		/// <summary>
		/// Moves in the provided direction by the provided amount.
		/// </summary>
		/// <param name="directionRadians">The direction. A null value indicates no movement.</param>
		/// <param name="forced">A value indicating whether movement is forced.</param>
		void Move(float? directionRadians, bool forced = false);
	}
}
