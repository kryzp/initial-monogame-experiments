using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARPG.Util.Collisions;
using Microsoft.Xna.Framework.Graphics;

namespace CollisionTest2.Entities.Sprites
{
	public class Tree : Sprite, ICollidable, ISolid
	{
		public Tree(Texture2D tex) : base(tex)
		{
		}

		public void OnCollide(Sprite other)
		{
		}
	}
}
