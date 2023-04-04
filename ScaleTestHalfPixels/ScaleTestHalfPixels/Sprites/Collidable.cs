using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleTestHalfPixels.Sprites
{
	public class Collidable : Sprite
	{
		public Vector2 Velocity = Vector2.Zero;

		public Collidable(Texture2D tex) : base(tex)
		{
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach(var sprite in Game1.Sprites)
			{
				if(sprite == this)
					continue;

				if(Intersects(sprite, new Vector2(Velocity.X, 0)))
				{
					while(!Intersects(sprite, new Vector2(Math.Sign(Velocity.X), 0)))
					{
						Position += new Vector2(Math.Sign(Velocity.X), 0);
					}

					Velocity.X = 0f;
				}

				if(Intersects(sprite, new Vector2(0, Velocity.Y)))
				{
					while(!Intersects(sprite, new Vector2(0, Math.Sign(Velocity.Y))))
					{
						Position += new Vector2(0, Math.Sign(Velocity.Y));
					}

					Velocity.Y = 0f;
				}
			}

			Position += Velocity;
		}

		public virtual void OnCollide(Sprite other)
		{
			Console.WriteLine("Collision Detected!");
		}
	}
}
