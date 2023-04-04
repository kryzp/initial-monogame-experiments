using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Managers;
using Pong.Models;
using Pong.Sprites;

namespace Pong
{
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Paddle leftPaddle;
        private Paddle rightPaddle;
        private Ball ball;

        public static bool GameOver = false;

        public static List<Sprite> Sprites;
        public static Random Random;
        public static ScoreManager ScoreManager;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = @"Content\bin";
        }
        
        protected override void Initialize()
        {
            Random = new Random();
            Sprites = new List<Sprite>();

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            ScoreManager = new ScoreManager(Content.Load<SpriteFont>("scorefont"));

            ball = new Ball(Content.Load<Texture2D>("ball"))
            {
                Position = new Vector2(ScreenWidth / 2, ScreenHeight / 2),
            };

            leftPaddle = new Paddle(Content.Load<Texture2D>("paddle"))
            {
                Position = new Vector2(100, ScreenHeight / 2),
                Colour = Color.Blue,
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S
                }
            };
            
            rightPaddle = new Paddle(Content.Load<Texture2D>("paddle"))
            {
                Position = new Vector2(ScreenWidth - 100, ScreenHeight / 2),
                Colour = Color.Orange,
                Input = new Input()
                {
                    Up = Keys.Up,
                    Down = Keys.Down
                }
            };
            
            Sprites.Add(ball);
            Sprites.Add(leftPaddle);
            Sprites.Add(rightPaddle);
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            if(GameOver) return;
            
            foreach(var sprite in Sprites)
            {
                sprite.Update(gameTime);
            }

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(13, 13, 13));

            spriteBatch.Begin();
            
            ScoreManager.Draw(gameTime, spriteBatch);
            foreach(var sprite in Sprites)
            {
                sprite.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}