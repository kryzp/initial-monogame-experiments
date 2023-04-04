using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrappyBird.Sprites;
using Microsoft.Xna.Framework.Graphics;

namespace CrappyBird.Managers
{
	public class PipePairManager
	{
		private const float SPAWN_THRESHOLD = 1.25f;
		private float timer = 0f;

		private List<Sprite> sprites;
		private Texture2D topPipeTex;
		private Texture2D bottomPipeTex;

		public bool CanSpawn = false;

		public PipePairManager(List<Sprite> sprites, Texture2D topPipeTex, Texture2D bottomPipeTex)
		{
			this.sprites = sprites;
			this.topPipeTex = topPipeTex;
			this.bottomPipeTex = bottomPipeTex;
		}

		public void Update(GameTime gameTime)
		{
			float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
			timer += delta;

			CanSpawn = false;

			if(timer >= SPAWN_THRESHOLD)
			{
				timer = 0f;
				CanSpawn = true;
			}
		}

		public PipePair getPipePair()
		{
			return new PipePair(sprites, topPipeTex, bottomPipeTex);
		}
	}
}
