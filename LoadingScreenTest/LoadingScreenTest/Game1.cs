using LoadingScreenTest.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LoadingScreenTest
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private LoadingScreenCircle loadingCirclePrefab;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            var loadingCircleTex = Content.Load<Texture2D>("loadingCircle");

            loadingCirclePrefab = new LoadingScreenCircle(new Dictionary<string, Models.Animation>()
            {
                {
                    "Pulse",
                    new Models.Animation(loadingCircleTex, 7, 1)
                    {
                        IsLooping = true,
                        FrameSpeed = 0.1f
                    }
                }
            })
            {
                Position = new Vector2(100, 100)
            };

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            loadingCirclePrefab.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
