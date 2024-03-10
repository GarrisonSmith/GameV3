using Engine.Loading.Base.interfaces;
using Engine.View.CameraTasks;
using Engine.View.CameraTasks.interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.View
{
	/// <summary>
	/// Represents a camera manager. 
	/// </summary>
	public class CameraManager : GameComponent, ICanBeLoaded
	{
		/// <summary>
		/// Start the camera manager.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="cameraStartingLocation">The camera starting location.</param>
		/// <param name="bottomItem">The bottom item.</param>
		/// <returns>The camera manager.</returns>
		public static CameraManager StartCameraManager(Game game, Vector2 cameraStartingLocation, FreeMovementTask bottomItem)
		{
			return Managers.CameraManager ?? new CameraManager(game, cameraStartingLocation, bottomItem);
		}

		/// <summary>
		/// Start the camera manager.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="camera">The camera.</param>
		/// <param name="bottomItem">The bottom item.</param>
		/// <returns>The camera manager.</returns>
		public static CameraManager StartCameraManager(Game game, FollowAreaCenterTask bottomItem)
		{
			return Managers.CameraManager ?? new CameraManager(game, bottomItem);
		}

		private ICameraTask bottomItem;

		/// <summary>
		/// Gets or sets a value indicating whether this has been loaded.
		/// </summary>
		public bool IsLoaded { get; set; }

		/// <summary>
		/// Gets or sets the bottom task in the camera manager.
		/// </summary>
		public ICameraTask BottomItem
		{
			get => bottomItem;

			set
			{
				if (value is FollowAreaCenterTask || value is FreeMovementTask)
				{
					bottomItem = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the camera.
		/// </summary>
		public Camera Camera { get; set; }

		/// <summary>
		/// Gets the camera task stack. 
		/// </summary>
		public Stack<ICameraTask> Tasks { get; }

		/// <summary>
		/// Initializes a new instance of the CameraManager class.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="bottomItem">The bottom task in the stack.</param>
		private CameraManager(Game game, FollowAreaCenterTask bottomItem) : base(game)
		{
			this.BottomItem = bottomItem;
			this.Camera = Camera.StartCamera(game, bottomItem.AreaToFollow.Area.Center);
			this.BottomItem.Camera = this.Camera; //this is a little smelly.
			this.Tasks = new Stack<ICameraTask>();
			this.Tasks.Push(bottomItem);
		}

		/// <summary>
		/// Initializes a new instance of the CameraManager class.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="cameraStartingLocation">The camera starting location.</param>
		/// <param name="bottomItem">The bottom task in the stack.</param>
		private CameraManager(Game game, Vector2 cameraStartingLocation, FreeMovementTask bottomItem) : base(game)
		{
			this.BottomItem = bottomItem;
			this.Camera = Camera.StartCamera(game, cameraStartingLocation);
			this.BottomItem.Camera = this.Camera; //this is a little smelly.
			this.Tasks = new Stack<ICameraTask>();
			this.Tasks.Push(bottomItem);
		}

		/// <summary>
		/// Initializes the content.
		/// </summary>
		public override void Initialize()
		{

		}

		/// <summary>
		/// Pushes a task to the camera task stack.
		/// </summary>
		/// <param name="task">The task to be added.</param>
		public void Push(ICameraTask task)
		{
			task.StartTask();
			Tasks.Push(task);
		}

		/// <summary>
		/// Pops the top task in the camera task stack.
		/// </summary>
		/// <returns>The top task in the camera task stack.</returns>
		public ICameraTask Pop()
		{
			return Tasks.Pop();
		}

		/// <summary>
		/// Peeks the top task in the camera task stack.
		/// </summary>
		/// <returns>The top task in the camera task stack.</returns>
		public ICameraTask Peek()
		{
			return Tasks.Peek();
		}

		/// <summary>
		/// Updates the camera manager by progressing the top task and popping it from camera task stack if the task is complete. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			if (Tasks.Peek().ProgressTask(gameTime))
			{
				Tasks.Pop();
				Tasks.Peek().StartTask();
			}
		}

		/// <summary>
		/// Loads data.
		/// </summary>
		public void Load()
		{
			this.IsLoaded = true;
		}
	}
}
