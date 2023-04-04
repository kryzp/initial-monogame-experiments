using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CrappyBird.Sprites
{
	public class PipePair
	{
		public TopPipe TopPipe;
		public BottomPipe BottomPipe;

		public PipePair(List<Sprite> sprites, Texture2D topPipeTex, Texture2D bottomPipeTex)
		{
			int space = 175;

			int padding = 100;
			int yPos = Game1.Random.Next(padding, Game1.WorldHeight - padding);


			TopPipe = new TopPipe(topPipeTex)
			{
				Position = new Vector2(Game1.WorldWidth + 100, yPos - (space / 2f))
			};

			BottomPipe = new BottomPipe(bottomPipeTex)
			{
				Position = new Vector2(Game1.WorldWidth + 100, yPos + (space / 2f))
			};

			sprites.Add(TopPipe);
			sprites.Add(BottomPipe);
		}

		public void Update(GameTime gameTime)
		{
			TopPipe.Update(gameTime);
			BottomPipe.Update(gameTime);
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			TopPipe.Draw(gameTime, spriteBatch);
			BottomPipe.Draw(gameTime, spriteBatch);
		}
	}
}
