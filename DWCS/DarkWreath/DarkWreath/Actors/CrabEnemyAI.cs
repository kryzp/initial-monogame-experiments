using Microsoft.Xna.Framework;

namespace DarkWreath.Actors
{
	public class CrabEnemyAI : EnemyAI
	{
		public override void Update(Enemy crab, GameTime gameTime)
		{
			Vector2 delta =
				0.1f * Time.Delta *
				(
					Game1.Player.Transform.Position +
					new Vector2(0f, 10f) -
					crab.Transform.Position
				)
				.Normalized();

			crab.Move(delta);
		}
	}
}
