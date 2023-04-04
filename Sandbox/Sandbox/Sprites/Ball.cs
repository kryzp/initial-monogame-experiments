using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Sprites
{
	public class Ball : Sprite, ICollidable
	{
		private Vector2 velocity = Vector2.Zero;

		Ball(Texture2D tex)
			: base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			// Ball hits top / bottom
			if(Position.Y <= 0 || Position.Y >= Game1.ScreenHeight)
			{
				velocity.Y *= -1;
			}

			// Ball hits left / right
			if(Position.X <= 0 || Position.X >= Game1.ScreenWidth)
			{
				// #TODO: Game Over
				velocity.X *= -1;
			}

			// Update Position
			Position += velocity;
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}

		public void OnCollide(Sprite sprite)
		{
			if(sprite is Paddle)
			{
				velocity.X *= -1;
			}
		}
	}
}
