using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweptAABB
{
	public class Box
	{
		public Rectangle Rectangle
		{
			get
			{
				return new Rectangle(
					(int)Position.X,
					(int)Position.Y,
					Texture.Width,
					Texture.Height
				);
			}
		}

		public Texture2D Texture;
		public Vector2 Velocity;
		public Vector2 Position;

		public Box(Texture2D tex)
		{
			Texture = tex;
		}
	}
}
