using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SandboxRider.States;

namespace SandboxRider
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Random Random;
        
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 1080;

        private StateBase currentState;
        private StateBase nextState;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content\bin";
        }
        
        protected override void Initialize()
        {
            Random = new Random();

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

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
            GraphicsDevice.Clear(new Color(13, 13, 13));
            currentState.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }

        public void ChangeState(StateBase state)
        {
            nextState = state;
        }
    }
}