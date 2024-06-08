using Engine.Loading.Configurations;
using Engine.Physics.Base;
using Engine.View.CameraTasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;

		//private readonly Effect testEffect;

		public Game1()
		{
			_graphics = new GraphicsDeviceManager(this);
			_graphics.PreferredBackBufferHeight = 1000;
			_graphics.PreferredBackBufferWidth = 1500;
			_graphics.ApplyChanges();
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			//this.testEffect = Content.Load<Effect>("testEffect");
		}

		protected override void Initialize()
		{
			// TODO: Add your initialization logic here

			Managers.Initialize(this, _graphics);

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// TODO: use this.Content to load your game content here
			Managers.Load();

			var pos = new Position(64, 64);
			//var button = new Button(true, 1, new SimpleArea(pos, 125, 32), Color.Aqua, Color.Crimson, 6, "Change Color", Color.Black, Managers.DrawManager.SpriteFonts[FontsConfig.MonoBold12]);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			if (Keyboard.GetState().IsKeyDown(Keys.C))
			{
				Managers.CameraManager.Camera.Zoom = 64;
			}

			if (Keyboard.GetState().IsKeyDown(Keys.T) && Managers.CameraManager.Tasks.Count == 1)
			{
				Managers.CameraManager.Push(new PanToWithZoomingTask(1, new Vector2(64, 64), new MoveSpeed(6)));
			}

			if (Keyboard.GetState().IsKeyDown(Keys.G))
			{
				var tileMap = Managers.TileManager.ActiveTileMap;
				Managers.LoadManager.SaveTileMap(tileMap);
			}

			if (Keyboard.GetState().IsKeyDown(Keys.H))
			{
				Managers.LoadManager.LoadTileMap(TileMapsConfig.AnimatedTestMap);
			}

			if (Keyboard.GetState().IsKeyDown(Keys.J))
			{
				Managers.TileManager.ActiveTileMap?.Dispose();
				Managers.DrawManager.TextureByName = new();
			}

			Managers.EntityManager.ControlledEntity?.Entity.Move(Managers.ControlManager.ControlState.DirectionRadians);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.CornflowerBlue);

			//Draws the scene.
			Managers.DrawManager.Begin(null, Managers.CameraManager.Camera.CameraMatrix);

			base.Draw(gameTime);

			Managers.DrawManager.End();

			//Draws the UI.
			Managers.DrawManager.Begin();

			Managers.ContentManager.DrawOverlay(gameTime);

			Managers.DrawManager.End();
		}
	}
}