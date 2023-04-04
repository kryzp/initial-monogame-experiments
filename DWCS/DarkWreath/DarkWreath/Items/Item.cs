using DarkWreath.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Items
{
	public abstract class Item
	{
		public virtual void Update(GameTime gameTime, Player player)
		{
		}

		public virtual void Draw(SpriteBatch b, Player player)
		{
		}

		public virtual void DrawUI(SpriteBatch b, Player player)
		{
		}
	}
}
