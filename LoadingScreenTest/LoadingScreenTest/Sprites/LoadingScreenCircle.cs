using LoadingScreenTest.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadingScreenTest.Sprites
{
	public class LoadingScreenCircle : Sprite
	{
		public LoadingScreenCircle(Dictionary<string, Animation> anims)
			: base(anims)
		{
		}

		public override void Update(GameTime gameTime)
		{
			animationManager.Update(gameTime);
		}
	}
}
