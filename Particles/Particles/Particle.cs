using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Particles
{
	public class Particle
	{
		public const float TOP_SPEED = 125f;

		private Texture2D px;

		public Vector2 Position;
		public Vector2 Acceleration;
		public Vector2 Velocity;
		public Color Colour;

		public Particle(SpriteBatch sb, Color colour)
		{
			this.Colour = colour;

			px = new Texture2D(sb.GraphicsDevice, 1, 1);
			px.SetData(new Color[] { Colour });
		}

		public void Update()
		{
			Vector2 mouse = Mouse.GetState().Position.ToVector2();
			Vector2 dir = mouse - Position;

			dir.Normalize();
			dir *= 0.99f;
			
			Acceleration = dir;
			Velocity += Acceleration;

			if (Velocity.Length() > TOP_SPEED)
			{
				var normalizedVelocity = dir;
				normalizedVelocity.Normalize();

				Velocity = normalizedVelocity * Velocity.Length();
			}

			Position += Velocity;
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Draw(px, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
	}
}
