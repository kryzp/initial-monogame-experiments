using System;
using System.Linq;
using System.Net.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownMovement.Models;

namespace TopDownMovement.Sprites
{
	public class Player : Sprite, ICollidable
	{
		private const float MOVE_SPEED = 400f;
		private float DIAG_SPEED = (float)(MOVE_SPEED * (Math.Sqrt(2) / 2));
		private float currentSpeed = 0f;

		private Vector2 movement = Vector2.Zero;
		private Vector2 velocity = Vector2.Zero;
		
		public PlayerInput Input { get; set; }
		
		public Player(Texture2D tex)
			: base(tex)
		{
			Origin = new Vector2(texture.Width / 2f, texture.Height);
		}

		public override void Update(GameTime gameTime)
		{
			// Basic Reference Variables
			var keyboard = Keyboard.GetState();
			var mouse = Mouse.GetState();

			// Handle Input
			if(keyboard.IsKeyDown(Input.Right)) movement.X = 1;
			else if(keyboard.IsKeyDown(Input.Left)) movement.X = -1;
			else movement.X = 0;
			if(keyboard.IsKeyDown(Input.Up)) movement.Y = -1;
			else if(keyboard.IsKeyDown(Input.Down)) movement.Y = 1;
			else movement.Y = 0;
			
			// Handle Diagonal Velocity
			if(velocity.X != 0 && velocity.Y != 0)
				currentSpeed = DIAG_SPEED;
			else
				currentSpeed = MOVE_SPEED;
			
			velocity = movement * currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

			HandleCollisions();

			// Snap Position
			if(velocity == Vector2.Zero) Position = new Vector2((int)Position.X, (int)Position.Y);
			
			// Update Position
			Position += velocity;
		}

		private void HandleCollisions()
		{
			foreach(var sprite in Game1.Sprites.Where(c => c is Wall))
			{
				if(sprite == this)
					continue;

				if(Intersects(sprite, new Vector2(velocity.X, 0)))
				{
					while(!Intersects(sprite, new Vector2(Math.Sign(velocity.X), 0)))
						Position += new Vector2(Math.Sign(velocity.X), 0);
					
					velocity.X = 0f;
				}

				if(Intersects(sprite, new Vector2(0, velocity.Y)))
				{
					while(!Intersects(sprite, new Vector2(0, Math.Sign(velocity.Y))))
						Position += new Vector2(0, Math.Sign(velocity.Y));
					
					velocity.Y = 0f;
				}
			}
		}

		public void OnCollide(Sprite other)
		{
			while(CollisionArea.Intersects(other.CollisionArea))
			{
				Position -= new Vector2(Math.Sign(velocity.X), Math.Sign(velocity.Y));
			}
		}
	}
}