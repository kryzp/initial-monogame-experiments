using System;
using System.Collections.Generic;
using System.Text;

namespace Spellpath
{
	public class PlayerSprite : AnimatedSprite
	{
		public PlayerSprite()
		{
		}

		public PlayerSprite(string fileName, int spriteWidth, int spriteHeight)
			: base(fileName, spriteWidth, spriteHeight)
		{
		}

		public PlayerSprite(string fileName, List<AnimationFrame> frames, int spriteWidth, int spriteHeight)
			: base(fileName, frames, spriteWidth, spriteHeight)
		{
		}

		public PlayerSprite(string fileName, int frames, int milliseconds, int spriteWidth, int spriteHeight)
			: base(fileName, frames, milliseconds, spriteWidth, spriteHeight)
		{
		}
	}
}
