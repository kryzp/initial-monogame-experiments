using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CRTShaderTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RenderTarget2D crtRenderTarget;
        private Effect crtEffect;

        private Texture2D background;

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
			crtRenderTarget = new RenderTarget2D(GraphicsDevice, ScreenWidth, ScreenHeight);

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
            spriteBatch = new SpriteBatch(GraphicsDevice);

            crtEffect = Content.Load<Effect>("crt_effect");
            background = Content.Load<Texture2D>("forest");

            crtEffect.Parameters["resolution"].SetValue(new Vector2(ScreenWidth, ScreenHeight));
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
            crtEffect.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.SetRenderTarget(crtRenderTarget);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(
                background,
                new Vector2(
                    ScreenWidth / 2f,
                    ScreenHeight / 2f
                ),
                null,
                Color.White,
                0f,
                new Vector2(
                    background.Width / 2,
                    background.Height / 2
                ),
                1.3f,
                SpriteEffects.None,
                0.5f
            );
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, effect: crtEffect);
            spriteBatch.Draw(crtRenderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
