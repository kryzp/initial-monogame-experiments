using System;
using Asteroids.Sprites;
using Asteroids.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private StateBase currentState;
        private StateBase nextState;
        
        private RenderTarget2D nativeRenderTarget;
        
        public static Random Random;

        public static int WorldWidth = 320;
        public static int WorldHeight = 180;
        
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

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

            currentState = new StateDebug(this, Content);
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
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            GraphicsDevice.Clear(new Color(0, 0, 0));
            
            //==================================================================//
            
            currentState.Draw(gameTime, spriteBatch);

            //==================================================================//
            
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            
            //==================================================================//
            
            currentState.DrawGUI(gameTime, spriteBatch);

            //==================================================================//
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public void ChangeState(StateBase state)
        {
            nextState = state;
        }
    }
}