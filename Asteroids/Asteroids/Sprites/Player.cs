using System;
using Asteroids.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.Sprites
{
	public class Player : Sprite, ICollidable
	{
		private float invulnerabilityTimer = 0f;
		private bool invulnerable = false;
		private float sinTimer = 0f;
		
		private float acceleration = 0.015f;
		private float thrust = 450f;//600f; TODO: Set this back since the low movement speed is for debug purposes only!
		private float friction = 1.0f;
		private float turnSpeed = 0.075f;
		
		private bool canShoot = true;
		
		private Vector2 velocity = Vector2.Zero;
		private Vector2 direction = Vector2.Zero;
		private Vector2 motion = Vector2.Zero;
		
		public int Health { get; set; }
		
		public bool IsDead
		{
			get
			{
				return Health <= 0;
			}
		}
		
		public Input Input { get; set; }
		public Score Score { get; set; }
		
		public Bullet Bullet { get; set; }
		
		public Player(Texture2D tex)
			: base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			var rot = Rotation * (180 / Math.PI);
			var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			
			// Check if Dead
			if(IsDead)
				return;

			//Console.WriteLine(invulnerabilityTimer);
			
			if(invulnerable)
			{
				sinTimer += delta / 5f;
				invulnerabilityTimer += delta;
				Scale += (float)Math.Sin((invulnerabilityTimer - 0.5f) * 10f) / 50f;
			}
			if(invulnerabilityTimer >= 5f)
			{
				Scale = 1f;
				sinTimer = 0f;
				invulnerabilityTimer = 0f;
				invulnerable = false;
			}

			// Input
			if(Keyboard.GetState().IsKeyDown(Input.Shoot) && canShoot)
			{
				canShoot = false;
				Shoot();
			}
			else if(!Keyboard.GetState().IsKeyDown(Input.Shoot))
			{
				canShoot = true;
			}

			if(Keyboard.GetState().IsKeyDown(Input.Thrust))
			{
				velocity.X -= thrust;
				velocity.Y -= thrust;
			}
			
			if(Keyboard.GetState().IsKeyDown(Input.TurnLeft))
			{
				Rotation -= turnSpeed;
			}
			else if(Keyboard.GetState().IsKeyDown(Input.TurnRight))
			{
				Rotation += turnSpeed;
			}
			
			// Find direction
			direction.X = -(float)Math.Sin(rot * 2 * Math.PI / 360);
			direction.Y = (float)Math.Cos(rot * 2 * Math.PI / 360);
			
			// Move
			motion = Vector2.Lerp(motion, velocity * Vector2.Normalize(direction), acceleration);
			Position += motion * delta; 

			// Apply Friction & Stop Player for exiting screen
			velocity = Vector2.Lerp(velocity, Vector2.Zero, friction);
			//Position = Vector2.Clamp(Position, new Vector2(0, 0), new Vector2(Game1.WorldWidth, Game1.WorldHeight));

			// Wrap Player Around World
			int padding = 15;
			if(Position.X <= -padding) Position = new Vector2(Game1.WorldWidth, Position.Y);
			if(Position.X >= Game1.WorldWidth + padding) Position = new Vector2(-padding, Position.Y);
			if(Position.Y <= -padding) Position = new Vector2(Position.X, Game1.WorldHeight);
			if(Position.Y >= Game1.WorldHeight + padding) Position = new Vector2(Position.Y, -padding);
			
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(IsDead)
				return;
			
			base.Draw(gameTime, spriteBatch);
		}

		public void OnCollide(Sprite other)
		{
			if(IsDead)
				return;

			if(invulnerable)
				return;
			
			if(other is Bullet && other.Parent is Player)
				return;
			
			Health--;
			BeginInvunerable();
		}
		
		private void Shoot()
		{
			if(Bullet.Clone() is Bullet bullet)
			{
				double rot = Rotation * (180 / Math.PI);
				int dist = 10;
				bullet.Position = new Vector2(this.Position.X + (float)(Math.Sin(rot * 2 * Math.PI / 360) * dist), this.Position.Y + (float)(Math.Cos(rot * 2 * Math.PI / 360) * -dist));
				bullet.Rotation = this.Rotation;
				bullet.Colour = this.Colour;
				bullet.Layer = 0.1f;
				bullet.LifeSpan = 1.5f;
				bullet.Velocity = -Vector2.Normalize(this.direction) * 250f;
				bullet.Parent = this;

				Children.Add(bullet);
			}
		}

		private void BeginInvunerable()
		{
			invulnerable = true;
		}
	}
}