using System;
using System.Collections.Generic;
using Asteroids.Models;
using Microsoft.Xna.Framework;

namespace Asteroids.Sprites
{
	public class SmallAsteroid : Asteroid
	{
		public SmallAsteroid(Dictionary<string, Animation> anims)
			: base(anims)
		{
		}

		public override void Update(GameTime gameTime)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			animationManager.Update(gameTime);

			velocity = speed * direction * delta;
			Position += velocity;
			
			int padding = 10;
			if(Position.X <= -padding) Position = new Vector2(Game1.WorldWidth, Position.Y);
			if(Position.X >= Game1.WorldWidth + padding) Position = new Vector2(-padding, Position.Y);
			if(Position.Y <= -padding) Position = new Vector2(Position.X, Game1.WorldHeight);
			if(Position.Y >= Game1.WorldHeight + padding) Position = new Vector2(Position.X, -padding);
		}

		public override void Destroy()
		{
			IsRemoved = true;
		}
	}
}