using CrappyBird.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrappyBird.Sprites
{
	public class Player : Sprite, ICollidable
	{
		private const float FLAP_FORCE = 782f;
		private const float GRAVITY = 50f;

		private float currentGravity = 0f;

		private Vector2 velocity = Vector2.Zero;

		private bool isDead = false;
		private bool canFlap = false;

		public PlayerInput Input { get; set; }
		public PlayerScore Score { get; set; }

		public Player(Texture2D tex) : base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

			var keyboard = Keyboard.GetState();

			if(keyboard.IsKeyDown(Input.Flap) && canFlap && !isDead)
			{
				canFlap = false;
				Flap();
			}

			if(!keyboard.IsKeyDown(Input.Flap))
			{
				canFlap = true;
			}

			currentGravity += GRAVITY;
			Position += new Vector2(0, currentGravity * delta);

			Position += velocity * delta;

			Rotation = MathHelper.Lerp(Rotation, MathHelper.ToRadians(90), 0.025f);
		}

		public void OnCollide(Sprite other)
		{
			if(other is Pipe)
			{
				Console.WriteLine("Player hit Pipe (and died)");
				isDead = true;
			}
		}

		private void Flap()
		{
			velocity.Y = -FLAP_FORCE;
			currentGravity = 0;
			Rotation = MathHelper.ToRadians(-50);
		}
	}
}
