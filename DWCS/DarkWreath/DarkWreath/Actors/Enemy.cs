using DarkWreath.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Actors
{
	public class Enemy : AliveActor
	{
		public EnemyAI AI;
		
		public Enemy()
		{
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			
			AI?.Update(this, gameTime);
		}

		public override void OnCollisionHit(Hit hit)
		{
			base.OnCollisionHit(hit);

			if (hit.Type == HitType.Actor)
				Transform.Position += (Transform.Position - hit.Other.Actor.Transform.Position) * 0.2f;
		}
	}

	public abstract class EnemyAI
	{
		public abstract void Update(Enemy enemy, GameTime gameTime);
	}

	public static class EnemyFactory
	{
		public static Enemy GetCrab()
		{
			return new Enemy()
			{
				AI = new CrabEnemyAI(),
				Sprite = new AnimatedSprite(Game1.Content.Load<Texture2D>("actors/enemy/crabbe")),
				Collider = new Collider(new RectangleF(0f, 0f, 16f, 16f)),
				Transform =
				{
					Origin = new Vector2(8f, 16f),
					Scale = Vector2.One * 4f
				}
			};
		}
	}
}
