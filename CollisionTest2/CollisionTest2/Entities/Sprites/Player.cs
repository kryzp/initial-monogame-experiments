using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionTest2.Entities.Sprites
{
	public class Player : Sprite, ICollidable
	{
		public Player(Texture2D tex) : base(tex)
		{
		}

		public override void Update(float deltaTime)
		{
			var pos = Position;

			if(Keyboard.GetState().IsKeyDown(Keys.W))
			{
				pos.Y -= 1;
			}
			if(Keyboard.GetState().IsKeyDown(Keys.S))
			{
				pos.Y += 1;
			}
			if(Keyboard.GetState().IsKeyDown(Keys.D))
			{
				pos.X += 1;
			}
			if(Keyboard.GetState().IsKeyDown(Keys.A))
			{
				pos.X -= 1;
			}

			Position = pos;
		}

		public void OnCollide(Sprite other)
		{
		}
	}
}
