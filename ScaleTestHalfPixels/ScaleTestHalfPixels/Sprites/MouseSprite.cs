using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleTestHalfPixels.Sprites
{
	public class MouseSprite : Collidable
	{
		private float speed = 750f;

		private Vector2 targetDirection;
		private Vector2 currentDirection;

		private int colcounter = 0;

		public MouseSprite(Texture2D tex) : base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			var mouse = Mouse.GetState();

			Vector2 diff = Vector2.Subtract(Position, new Vector2(mouse.X, mouse.Y));
			targetDirection = -Vector2.Normalize(diff);

			currentDirection.X = MathHelper.Lerp(currentDirection.X, targetDirection.X, 0.1f);
			currentDirection.Y = MathHelper.Lerp(currentDirection.Y, targetDirection.Y, 0.1f);

			Velocity = speed * currentDirection * (float)gameTime.ElapsedGameTime.TotalSeconds;

			base.Update(gameTime);
		}

		public override void OnCollide(Sprite other)
		{
			//colcounter++;
			//Console.WriteLine(colcounter + ". COLLISION DETECTED");

			while(CollisionArea.Intersects(other.CollisionArea))
			{
				Position -= Vector2.One * currentDirection;
			}

			//Position -= velocity;
		}
	}
}
