using System.Collections.Generic;
using DarkWreath.Items.Weapons.Ranged;
using DarkWreath.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Actors
{
	public class Projectile : Actor
	{
		private List<AliveActor> alreadyHit;
		
		public AmmoTypes Type;
		public ProjectileAI AI;
		public Trail Trail;
		public Vector2 Velocity;

		public Projectile()
		{
			alreadyHit = new List<AliveActor>();
		}

		private static int counter = 0;
		
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			
			Sprite.AnimateForward(gameTime);
			Transform.RotationRad = Velocity.Angle() + MathHelper.PiOver2;
			
			Trail.AddPartile(Transform.Position);
			Trail.Update(gameTime);
			
			AI.Update(this, gameTime);

			Move(Velocity * Time.Delta, Velocity.LengthSquared() > 640f ? 512f : 256f);
		}

		public override void Draw(GameTime gameTime, SpriteBatch b)
		{
			base.Draw(gameTime, b);
			Trail.Draw(b, 0.3f);
		}

		public override void OnCollisionHit(Hit hit)
		{
			if (hit.Other.Actor is AliveActor aActor && hit.Type == HitType.Actor)
			{
				if (alreadyHit.Contains(aActor))
					return;
						
				System.Diagnostics.Debug.WriteLine(counter.ToString());

				counter++;
				
				alreadyHit.Add(aActor);
			}
			else
			{
				Velocity = Vector2.Zero;
			}
		}
	}

	public abstract class ProjectileAI
	{
		public abstract void Update(Projectile projectile, GameTime gameTime);
	}

	public class DefaultProjectileAI : ProjectileAI
	{
		public override void Update(Projectile projectile, GameTime gameTime)
		{
		}
	}

	public static class ProjectileFactory
	{
		public static Projectile GetRevolverBullet()
		{
			return new Projectile
			{
				Type = AmmoTypes.Magnum_357,
				AI = new DefaultProjectileAI(),
				Collider = new Collider(new RectangleF(0f, -8f, 1f, 16f)),
				Trail = new Trail()
				{
					Colour = new Color(250, 252, 217),
					FadeTime = 7.5f
				},
				Sprite = new AnimatedSprite(Game1.ProjectileSheet, new List<AnimationFrame>()
				{
					new AnimationFrame(new Rectangle(0, 0, 1, 2), 100, false),
					new AnimationFrame(new Rectangle(1, 0, 1, 2), 100, false)
				}),
				Transform =
				{
					Origin = new Vector2(0.5f, 1f)
				}
			};
		}

		/*
		public static Projectile GetArrow()
		{
			return new Projectile()
			{
				Type = AmmoTypes.Arrow
			};
		}
		*/
	}
}
