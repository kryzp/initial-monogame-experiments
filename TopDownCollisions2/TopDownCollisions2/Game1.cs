using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownCollisions2.States;

namespace TopDownCollisions2
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private RenderTarget2D nativeRenderTarget;

        private StateBase currentState;
        private StateBase nextState;
        
        public static Random Random;

        public static int WorldWidth = 320;
        public static int WorldHeight= 180;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public static float TimeScale = 1f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content\bin";
        }

        protected override void Initialize()
        {
            Random = new Random();
            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, WorldWidth, WorldHeight);

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
            currentState.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            delta *= TimeScale;
            
            if(nextState != null)
            {
                currentState = nextState;
                currentState.LoadContent();

                nextState = null;
            }

            currentState.Update(delta);
            currentState.PostUpdate(delta);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            delta *= TimeScale;
            
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            
            //================================[DRAW GAME]================================//
            
            currentState.Draw(delta, spriteBatch);
            
            //===========================================================================//
            
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);
            
            //================================[DRAW GUI]=================================//
            
            currentState.DrawGUI(delta, spriteBatch);
            
            //===========================================================================//
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void ChangeState(StateBase state)
        {
            nextState = state;
        }
    }
}