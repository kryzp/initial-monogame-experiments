using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrappyBird.Sprites
{
	public class BottomPipe : Pipe
	{
		public BottomPipe(Texture2D tex) : base(tex)
		{
			Origin = new Vector2(texture.Width / 2f, 0f);
		}
	}
}
