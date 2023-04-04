using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ParticlesLines
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private MouseState previousMouseState;
        private Texture2D floatingCircleTex;
        
        private List<FloatingCircle> circles;
        private LineManager lineManager;

        public static Random Random;

        public static int Width = 1280;
        public static int Height = 720;

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
            Random = new Random();

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
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
            circles = new List<FloatingCircle>();
            lineManager = new LineManager(circles);

            floatingCircleTex = Content.Load<Texture2D>("circle");
		}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(dt >= 0 && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                dt *= -1;
            }

			if(Mouse.GetState().LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed)
			{
                CreatePoints(90, new Vector2(Mouse.GetState().X, Mouse.GetState().Y));
			}

			foreach(var circle in circles)
			{
				circle.Update(dt);
			}

            lineManager.Update(dt);
            previousMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(13, 13, 13));
            spriteBatch.Begin();

			foreach(var circle in circles)
			{
				circle.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);
			}

            lineManager.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);
            spriteBatch.End();
			base.Draw(gameTime);
        }

        private void CreatePoints(int amount, Vector2 position)
        {
			float dirIncrement = 360f / amount;
			Console.WriteLine(dirIncrement);
			for(int ii = 0; ii < amount; ii++)
			{
				float dir = dirIncrement * ii;

				FloatingCircle circle = new FloatingCircle(floatingCircleTex)
				{
					Position = position
                };

				Vector2 direction = circle.Direction;

				direction.X = -(float)Math.Sin(dir * 2 * Math.PI / 360);
				direction.Y = (float)Math.Cos(dir * 2 * Math.PI / 360);
				direction.Normalize();

				circle.Direction = direction;
				circles.Add(circle);
			}
		}
    }
}
