using System;
using Breakout.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout
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
        public static int WorldHeight = 180;
        
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
            
            currentState = new StateMainMenu(this, Content);
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

            //TimeScale = MathHelper.Lerp(TimeScale, 1f, 0.015f);
            
            if(Keyboard.GetState().IsKeyDown(Keys.P))
            {
                TimeScale = MathHelper.Lerp(TimeScale, 0.25f, 0.2f);
            }
            else
            {
                TimeScale = MathHelper.Lerp(TimeScale, 1f, 0.2f);
            }
            
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
            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            GraphicsDevice.Clear(new Color(21, 25, 30));
            
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            //================================[DRAW GAME]=================================//
            
            currentState.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);
            
            //============================================================================//
            
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);

            //================================[DRAW GUI]==================================//
            
            currentState.DrawGUI((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);
            
            //============================================================================//
            
            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public void ChangeState(StateBase state)
        {
            nextState = state;
        }
    }
}