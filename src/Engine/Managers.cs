using Engine.Controls;
using Engine.Core;
using Engine.Debugging;
using Engine.Drawing;
using Engine.Entities;
using Engine.Loading;
using Engine.Loading.Configurations;
using Engine.Physics;
using Engine.Physics.Areas;
using Engine.TileMapping;
using Engine.TileMapping.Base;
using Engine.UI;
using Engine.View;
using Engine.View.CameraTasks;
using Microsoft.Xna.Framework;

namespace Engine
{
	/// <summary>
	/// Represents the managers.
	/// </summary>
	public static class Managers
	{
		/// <summary>
		/// Gets or sets the game.
		/// </summary>
		public static Game Game { get; private set; }

		/// <summary>
		/// Gets or sets the graphics device manager.
		/// </summary>
		public static GraphicsDeviceManager Graphics { get; private set; }

		/// <summary>
		/// Gets or sets the load manager.
		/// </summary>
		public static LoadManager LoadManager { get; private set; }

		/// <summary>
		/// Gets or sets the random manager.
		/// </summary>
		public static RandomManager RandomManager { get; private set; }

		/// <summary>
		/// Gets or sets the content manager.
		/// </summary>
		public static ContentManager ContentManager { get; private set; }

		/// <summary>
		/// Gets or sets the draw manager.
		/// </summary>
		public static DrawManager DrawManager { get; private set; }

		/// <summary>
		/// Gets or sets the tile manager.
		/// </summary>
		public static TileManager TileManager { get; private set; }

		/// <summary>
		/// Gets or sets the entity manager.
		/// </summary>
		public static EntityManager EntityManager { get; private set; }

		/// <summary>
		/// Gets or sets the control manager.
		/// </summary>
		public static ControlManager ControlManager { get; private set; }

		/// <summary>
		/// Gets or sets the camera manager.
		/// </summary>
		public static CameraManager CameraManager { get; private set; }

		/// <summary>
		/// Gets or sets the physics manager.
		/// </summary>
		public static PhysicsManager PhysicsManager { get; private set; }

		/// <summary>
		/// Gets or sets the debugging manager.
		/// </summary>
		public static DebuggingManager DebuggingManager { get; private set; }

		/// <summary>
		/// Gets or sets the UI manager.
		/// </summary>
		public static UiManager UiManager { get; private set; }

		/// <summary>
		/// Initializes the managers.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="graphics">The graphics device manager.</param>
		public static void Initialize(Game game, GraphicsDeviceManager graphics)
		{
			Managers.Game = game;
			Managers.Graphics = graphics;

			// Order of these starting matters.
			Managers.LoadManager = LoadManager.StartLoadManager(game);
			Managers.PhysicsManager = PhysicsManager.StartPhysicsManager(game);
			Managers.ControlManager = ControlManager.StartControlManager(game);
			Managers.DrawManager = DrawManager.StartDrawManager(game.GraphicsDevice);
			Managers.CameraManager = CameraManager.StartCameraManager(game, new Vector2(0, 0), new FreeMovementTask());
			Managers.ContentManager = ContentManager.StartContentManager(game);
			Managers.DebuggingManager = DebuggingManager.StartDebugger(game);
			Managers.RandomManager = RandomManager.StartRandomManager();
			Managers.TileManager = TileManager.StartTileManager();
			Managers.EntityManager = EntityManager.StartEntityManager();
			Managers.UiManager = UiManager.StartUiManager(game);

			// Add game components.
			Managers.PhysicsManager.UpdateOrder = 1;
			game.Components.Add(Managers.PhysicsManager);
			Managers.ControlManager.UpdateOrder = 2;
			game.Components.Add(Managers.ControlManager);
			Managers.CameraManager.UpdateOrder = 3;
			game.Components.Add(Managers.CameraManager);
			Managers.ContentManager.UpdateOrder = 4;
			game.Components.Add(Managers.ContentManager);
			Managers.UiManager.UpdateOrder = 5;
			game.Components.Add(Managers.UiManager);
			Managers.DebuggingManager.UpdateOrder = 6;
			game.Components.Add(Managers.DebuggingManager);
		}

		/// <summary>
		/// Loads the managers.
		/// </summary>
		public static void Load()
		{
			// Determines if debugging info will be shown.
			Managers.DebuggingManager.DebuggerEnabled = false;

			// Managers that need to load content from disk.
			Managers.DrawManager.Load();
			Managers.TileManager.Load();
			Managers.EntityManager.Load();
			
			// Setting the starting tile map.
			TileMap map = new(true, TileMapsConfig.AnimatedTestMap);
			map.Load();
			
			// Setting the playable character.
			Managers.CameraManager.Camera.CameraBounding = new SimpleArea(Managers.TileManager.ActiveTileMap.GetTileMapBounds());
			Managers.LoadManager.LoadEntity("character3");
			Managers.CameraManager.Pop();
			Managers.CameraManager.Push(new FollowAreaCenterTask(Managers.EntityManager.ControlledEntity.Entity));
		}
	}
}
