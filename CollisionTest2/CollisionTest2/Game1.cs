using CollisionTest2.Entities.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ARPG.Meta;

namespace CollisionTest2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Player player;
        public Tree tree;

        public Camera camera;

		public static int WorldWidth = 320;
		public static int WorldHeight = 180;

		public static int ScreenWidth = 1280;
		public static int ScreenHeight = 720;

		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            camera = new Camera(GraphicsDevice.Viewport)
			{
				Position = new Vector2(WorldWidth / 2f, WorldHeight / 2f)
			};

			graphics.PreferredBackBufferWidth = ScreenWidth;
			graphics.PreferredBackBufferHeight = ScreenHeight;
			graphics.ApplyChanges();

			IsMouseVisible = true;
			base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Player(Content.Load<Texture2D>("bush"));
            tree = new Tree(Content.Load<Texture2D>("oak_tree"));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            player.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            tree.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            //camera.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
			GraphicsDevice.Clear(new Color(38, 96, 65));
			spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            player.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);
            tree.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
