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
	public class TestSprite : Collidable
	{
		public TestSprite(Texture2D tex)
			: base(tex)
		{
		}

		public override void OnCollide(Sprite other)
		{
		}
	}
}
