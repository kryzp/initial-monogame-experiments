using System;
using System.Collections.Generic;
using Asteroids.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Sprites
{
	public class Asteroid : Sprite, ICollidable
	{
		protected float speed = 50f;

		protected Vector2 velocity = Vector2.Zero;
		protected Vector2 direction = Vector2.Zero;

		public SmallAsteroid SmallAsteroid { get; set; }

		public Asteroid(Dictionary<string, Animation> anims)
			: base(anims)
		{
			animationManager.CurrentAnimation.CurrentFrame = Game1.Random.Next(4);
			
			#region Set Direction
			
			Vector2 difference = Vector2.Subtract(Position,
				new Vector2(
					Game1.Random.Next(-Game1.WorldWidth, Game1.WorldWidth),
					Game1.Random.Next(-Game1.WorldHeight, Game1.WorldHeight)
				));
			direction = Vector2.Normalize(difference);
			
			#endregion
		}

		public override void Update(GameTime gameTime)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			animationManager.Update(gameTime);

			velocity = speed * direction * delta;
			Position += velocity;
			
			int padding = 15;
			if(Position.X <= -padding) Position = new Vector2(Game1.WorldWidth, Position.Y);
			if(Position.X >= Game1.WorldWidth + padding) Position = new Vector2(-padding, Position.Y);
			if(Position.Y <= -padding) Position = new Vector2(Position.X, Game1.WorldHeight);
			if(Position.Y >= Game1.WorldHeight + padding) Position = new Vector2(Position.Y, -padding);
		}

		public virtual void Destroy()
		{
			for(int ii = 0; ii <= 3; ii++)
				InstanceSmallAsteroid();

			IsRemoved = true;
		}
		
		public void OnCollide(Sprite other)
		{
		}

		private void InstanceSmallAsteroid()
		{
			if(SmallAsteroid.Clone() is SmallAsteroid smallAsteroid)
			{
				smallAsteroid.Position = Position;
				smallAsteroid.Colour = Colour;
				smallAsteroid.Layer = Layer;

				smallAsteroid.animationManager.CurrentAnimation.CurrentFrame = Game1.Random.Next(4);
				
				Vector2 diff = Vector2.Subtract(
					Position,
					new Vector2(
						Game1.Random.Next(-Game1.WorldWidth, Game1.WorldWidth),
						Game1.Random.Next(-Game1.WorldHeight, Game1.WorldHeight)
					)
				);
				smallAsteroid.direction = Vector2.Normalize(diff);
				
				smallAsteroid.Parent = this;
				Children.Add(smallAsteroid);
			}
		}
	}
}