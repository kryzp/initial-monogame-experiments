using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweptAABB
{
	public class Player : ICollidable
	{
		public Box Collider;

		public Vector2 Position
		{
			get => Collider.Position;
			set => Collider.Position = value;
		}

		public Player(Texture2D tex)
		{
			Collider = new Box(tex);
		}

		public void Update(float deltaTime)
		{
			var kb = Keyboard.GetState();

			Collider.Velocity = Vector2.Zero;

			if(kb.IsKeyDown(Keys.W))
			{
				Collider.Velocity.Y = -1;
			}

			if(kb.IsKeyDown(Keys.S))
			{
				Collider.Velocity.Y = 1;
			}

			if(kb.IsKeyDown(Keys.D))
			{
				Collider.Velocity.X = 1;
			}

			if(kb.IsKeyDown(Keys.A))
			{
				Collider.Velocity.X = -1;
			}

			Position += Collider.Velocity;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Collider.Texture, Collider.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.6f);
		}

		public void OnCollide()
		{
		}
	}
}
