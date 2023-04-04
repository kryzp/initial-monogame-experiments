using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace ParticlesLines
{
	public class FloatingCircle
	{
		private readonly float MOVE_SPEED = 120f;

		private Texture2D texture;

		private Vector2 position;
		private Vector2 origin;
		private float scale;
		private Vector2 direction;
		private Vector2 velocity;

		public Vector2 Position
		{
			get => position;
			set => position = value;
		}

		public Vector2 Direction
		{
			get => direction;
			set => direction = value;
		}

		public FloatingCircle(Texture2D tex)
		{
			texture = tex;
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			scale = 1 / 148f;

			position = new Vector2(Game1.Random.Next(0, Game1.Width), Game1.Random.Next(0, Game1.Height));

			float dir = Game1.Random.Next(0, 359);

			direction.X = -(float)Math.Sin(dir * 2 * Math.PI / 360);
			direction.Y = (float)Math.Cos(dir * 2 * Math.PI / 360);
			direction.Normalize();
		}

		public void Update(float deltaTime)
		{
			velocity = direction * MOVE_SPEED * deltaTime;
			if(position.X <= texture.Width / 2 * scale)
				direction.X *= -1;
				//position.X = Game1.Width + texture.Width * scale;
			else if(position.X >= Game1.Width - texture.Width / 2 * scale)
				direction.X *= -1;
				//position.X = -texture.Width * scale;
			if(position.Y <= texture.Height / 2 * scale)
				direction.Y *= -1;
				//position.Y = Game1.Height + texture.Height * scale;
			else if(position.Y >= Game1.Height + texture.Height / 2 * scale)
				direction.Y *= -1;
				//position.Y = -texture.Height * scale;
			position += velocity;
		}

		public void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, position, null, Color.White * 0.7f, 0f, origin, scale, SpriteEffects.None, 0.5f);
		}
	}
}
