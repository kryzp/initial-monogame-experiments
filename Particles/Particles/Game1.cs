using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Particles
{
	public class Game1 : Game
	{
		public const int PARTICLE_COUNT = 10000;

		private GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;

		public Random RNG = new Random();
		public List<Particle> Particles = new List<Particle>();

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			for (int i = 0; i < PARTICLE_COUNT; i++)
			{
				Particles.Add(new Particle(spriteBatch, Color.White)
				{
					Position = new Vector2(RNG.Next(0, 1280), RNG.Next(0, 720)),
					Velocity = Vector2.Zero,
				});
			}
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			Particles.ForEach((Particle p) => { p.Update(); });

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.Black);

			spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
			Particles.ForEach((Particle p) => { p.Draw(spriteBatch); });
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
