using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionTest.Sprites
{
	public class MouseFollower : Sprite, ICollidable
	{
		private const float MOVE_SPEED = 100f;
		private static float DIAG_SPEED = (float)(MOVE_SPEED * (Math.Sqrt(2) / 2));

		private float currentSpeed = MOVE_SPEED;
		
		private Vector2 targetDirection;
		private Vector2 currentDirection;
		
		private Vector2 velocity;
		private Vector2 movement;
		
		public MouseFollower(Texture2D tex)
			: base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			var keyboard = Keyboard.GetState();
			var mouse = Mouse.GetState();

			if(keyboard.IsKeyDown(Keys.D)) movement.X = 1;
			else if(keyboard.IsKeyDown(Keys.A)) movement.X = -1;
			else movement.X = 0;
			if(keyboard.IsKeyDown(Keys.W)) movement.Y = -1;
			else if(keyboard.IsKeyDown(Keys.S)) movement.Y = 1;
			else movement.Y = 0;

			if(velocity.X != 0 && velocity.Y != 0)
				currentSpeed = DIAG_SPEED;
			else
				currentSpeed = MOVE_SPEED;

			velocity = currentSpeed * movement * (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			foreach(var sprite in Game1.Sprites)
			{
				if(sprite == this)
					continue;

				if(Intersects(sprite, new Vector2(velocity.X, 0)))
				{
					while(!Intersects(sprite, new Vector2(Math.Sign(velocity.X), 0)))
					{
						Position += new Vector2(Math.Sign(velocity.X), 0);
					}

					velocity.X = 0f;
				}

				if(Intersects(sprite, new Vector2(0, velocity.Y)))
				{
					while(!Intersects(sprite, new Vector2(0, Math.Sign(velocity.Y))))
					{
						Position += new Vector2(0, Math.Sign(velocity.Y));
					}

					velocity.Y = 0f;
				}
			}
			
			Position += velocity;
		}

		public void OnCollide(Sprite other)
		{
			while(CollisionArea.Intersects(other.CollisionArea))
			{
				Position -= Vector2.One * movement;
			}
		}
	}
}