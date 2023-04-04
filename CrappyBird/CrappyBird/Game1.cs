using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using CrappyBird.States;

namespace CrappyBird
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private StateBase currentState;
        private StateBase nextState;

        public static Random Random;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public static int WorldScale = 1;

        public static int WorldWidth = ScreenWidth / WorldScale;
        public static int WorldHeight = ScreenHeight / WorldScale;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Random = new Random();

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            currentState = new StatePlaying(this, Content);
            currentState.LoadContent();
            nextState = null;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if(nextState != null)
            {
                currentState = nextState;
                currentState.LoadContent();

                nextState = null;
            }

            currentState.Update(gameTime);
            currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            //======================[DRAW GAME]======================//

            currentState.Draw(gameTime, spriteBatch);

            //=======================================================//

            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            //======================[DRAW GUI]=======================//

            currentState.DrawGUI(gameTime, spriteBatch);

            //=======================================================//

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ChangeState(StateBase state)
        {
            nextState = state;
        }
    }
}
