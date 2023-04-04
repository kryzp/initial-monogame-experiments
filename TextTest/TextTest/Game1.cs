using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TextTest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RenderTarget2D nativeRenderTarget;

        public SpriteFont testFont;
        public Vector2 testPosition = Vector2.Zero;
        public Vector2 testTargetPosition = Vector2.Zero;

        public static int WorldWidth = 320;
        public static int WorldHeight = 180;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, WorldWidth, WorldHeight);

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            testFont = Content.Load<SpriteFont>("fonts/arial");

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var mouse = Mouse.GetState();
            if(mouse.LeftButton == ButtonState.Pressed ||
               mouse.RightButton == ButtonState.Pressed)
            {
                testTargetPosition = new Vector2(mouse.X / (ScreenWidth / WorldWidth), mouse.Y / (ScreenWidth / WorldWidth));
            }

            testPosition.X = MathHelper.Lerp(testPosition.X, testTargetPosition.X, 0.2f);
            testPosition.Y = MathHelper.Lerp(testPosition.Y, testTargetPosition.Y, 0.2f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            //=======================================================
            // Draw World
            //-------------------------------------------------------

            spriteBatch.DrawString(testFont, "Hello, World!", testPosition, Color.White);

            //=======================================================
            
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);

            //=======================================================
            // Draw GUI
            //-------------------------------------------------------

            spriteBatch.DrawString(testFont, "Hello,World", testPosition, Color.White);

            //=======================================================

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
