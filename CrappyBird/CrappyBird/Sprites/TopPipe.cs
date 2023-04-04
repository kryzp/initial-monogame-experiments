using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CrappyBird.Sprites
{
	public class TopPipe : Pipe
	{
		public TopPipe(Texture2D tex) : base(tex)
		{
			Origin = new Vector2(texture.Width / 2f, texture.Height);
		}
	}
}
