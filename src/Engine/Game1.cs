using Engine.Physics.Base;
using Engine.View.CameraTasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;

		//private readonly Effect testEffect;aaaa

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

			Managers.EntityManager.ControlledEntity.Entity.Move(Managers.ControlManager.ControlState.DirectionRadians);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here
			Managers.DrawManager.Begin(null, Managers.CameraManager.Camera.CameraMatrix);

			Managers.ContentManager.Draw(gameTime);
			base.Draw(gameTime);

			Managers.DrawManager.End();
		}
	}
}