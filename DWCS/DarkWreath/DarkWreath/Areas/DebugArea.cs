using DarkWreath.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Areas
{
	public class DebugArea : GameArea
	{
		public DebugArea()
		{
			LoadMap("maps/untitled");

			AddActor(EnemyFactory.GetCrab());
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
		}

		public override void Draw(GameTime time, SpriteBatch b)
		{
			base.Draw(time, b);
		}
	}
}
