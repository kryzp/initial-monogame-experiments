using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrappyBird.Sprites
{
	public class Pipe : Sprite, ICollidable
	{
		private const float PIPE_SPEED = 200f;

		public Pipe(Texture2D tex) : base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Position -= new Vector2(PIPE_SPEED * delta, 0);

			if(Position.X <= -100)
				IsRemoved = true;
		}
		public void OnCollide(Sprite other)
		{
		}
	}
}
