using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DarkWreath
{
	public class Game1 : Game
	{
		public const int WINDOW_WIDTH = 1920;
		public const int WINDOW_HEIGHT = 1080;
		public const int FRAMES_PER_SECOND = 144;
		
		private static GraphicsDeviceManager m_graphics;
		private static SpriteBatch m_spriteBatch;

		public static new GraphicsDevice GraphicsDevice;

		public static new ContentManager Content;
		public static ContentManager MapContent;

		public Game1()
		{
			m_graphics = new GraphicsDeviceManager(this);

			System.IServiceProvider provider = base.Content.ServiceProvider;
			{
				Content = new ContentManager(provider);
				Content.RootDirectory = "Content";

				MapContent = new ContentManager(provider);
				MapContent.RootDirectory = "Content";
			}

			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			//Window.TextInput += HandleTextInput;

			IsMouseVisible = true;
			IsFixedTimeStep = true;
			TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000f / (float)FRAMES_PER_SECOND);

			Game1.GraphicsDevice = base.GraphicsDevice;

			m_graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			m_graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
			m_graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			m_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}