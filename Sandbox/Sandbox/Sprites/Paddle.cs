using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sandbox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Sprites
{
	public class Paddle : Sprite
	{
		private float Speed { get; set; }
		private Input Input { get; set; }

		Paddle(Texture2D tex)
			: base(tex)
		{
		}

		public override void Update(GameTime gameTime)
		{
			if(Keyboard.GetState().IsKeyDown(Input.Up))
			{
				// move up
			}
			else if(Keyboard.GetState().IsKeyDown(Input.Down))
			{
				// move down
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);
		}
	}
}
