namespace Engine.Physics.Base.interfaces
{
	/// <summary>
	/// Represents something with a position.
	/// </summary>
	public interface IHavePosition
    {
        /// <summary>
        /// Gets or sets the position. This is the top left point.
        /// </summary>
        Position Position { get; set; }
    }
}
