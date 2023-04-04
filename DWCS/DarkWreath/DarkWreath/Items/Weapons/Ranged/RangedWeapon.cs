using System;
using DarkWreath.Actors;
using DarkWreath.Input;
using DarkWreath.Items.Weapons.Ranged.Reloading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Items.Weapons.Ranged
{
	public abstract class RangedWeapon : Weapon
	{
		public static readonly Color LASER_COLOR_BASE = new Color(255, 20, 50, 200);
		public static readonly Color LASER_COLOR_FIRE = new Color(222, 175, 10, 240);

		public const float LASER_THICKNESS_BASE = 0.5f;
		public const float LASER_THICKNESS_FIRE = 1.5f;

		public const float LASER_ALPHA_BASE = 0.65f;
		public const float LASET_ALPHA_FIRE = 1.25f;
		
		private float shootTimer = 0f;
		private float laserTimer = 0f;
		private float laserThickness = LASER_THICKNESS_BASE;
		private float laserBaseAlpha = LASER_ALPHA_BASE;
		private Color laserColour = LASER_COLOR_BASE;

		protected int pCurrentAmmoInClip;

		// v DEFAULTS v //
		public float RechamberTime;
		public int MaxAmmoInClip;
		public AmmoTypes AmmoType;
		public ReloadPattern ReloadPattern;
		// ^ DEFAULTS ^ //

		public static bool AimingDownSights => Game1.PlayerInput.GetState().AimDownSights;
		public bool Reloading => ReloadPattern.Reloading;
		public int CurrentAmmoInClip => pCurrentAmmoInClip;
		
		public RangedWeapon()
		{
			SetDefaults();
			ReloadPattern.RangedWeapon = this;
		}

		public virtual void SetDefaults()
		{
		}
		
		public override void Update(GameTime gameTime, Player player)
		{
			base.Update(gameTime, player);

			laserColour = Color.Lerp(laserColour, LASER_COLOR_BASE, 0.025f);
			laserThickness = MathHelper.Lerp(laserThickness, LASER_THICKNESS_BASE, 0.05f);
			laserBaseAlpha = MathHelper.Lerp(laserBaseAlpha, LASER_ALPHA_BASE, 0.05f);

			shootTimer = MathF.Max(0f, shootTimer - Time.Delta);

			if (AimingDownSights)
				laserTimer += Time.Delta;
			else
				laserTimer = 0f;
			
			ReloadPattern.Update(gameTime, player);
			
			if (Game1.PlayerInput.GetState().FireButtonPressed &&
			    AimingDownSights &&
			    !Reloading)
			{
				if (pCurrentAmmoInClip > 0 && shootTimer <= 0f)
				{
					laserColour = LASER_COLOR_FIRE;
					laserThickness = LASER_THICKNESS_FIRE;
					laserBaseAlpha = LASET_ALPHA_FIRE;

					shootTimer = RechamberTime;

					pCurrentAmmoInClip--;
					
					Fire(player);
				}
				else
				{
					FireWithoutAmmo(player);
				}
			}
		}

		public override void Draw(SpriteBatch b, Player player)
		{
			base.Draw(b, player);

			if (AimingDownSights)
				DrawAimingLaser(b, player);

//			b.DrawString(Game1.DebugFont, "PRG:" + ReloadPattern.CurrentReloadSegment.Progress(), new Vector2(10f, 10f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
//			b.DrawString(Game1.DebugFont, "JFR:" + justFinishedReloading, new Vector2(10f, 10f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
//			b.DrawString(Game1.DebugFont, "REL:" + Reloading, new Vector2(10f, 40f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
//			b.DrawString(Game1.DebugFont, "TAC:" + player.AmmoCounts[AmmoType], new Vector2(10.0f, 70.0f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
//			b.DrawString(Game1.DebugFont, "AIC:" + pCurrentAmmoInClip, new Vector2(10f, 100f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
//			b.DrawString(Game1.DebugFont, "DTL:" + (InputManager.MouseScreenPosition() - currentReloadSegmentStartingPosition).Length(), new Vector2(10f, 130f), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
		}

		public override void DrawUI(SpriteBatch b, Player player)
		{
			base.DrawUI(b, player);
			
			if (Reloading)
				ReloadPattern.Draw(b, player);
		}

		public void Reload(Player player)
		{
			if (!player.HasAmmoOfType(AmmoType))
				OnReloadWithoutAmmo(player);

			pCurrentAmmoInClip = MathHelper.Clamp(player.AmmoCounts[AmmoType], 0, MaxAmmoInClip);
			player.AmmoCounts[AmmoType] = MathHelper.Max(player.AmmoCounts[AmmoType] - pCurrentAmmoInClip, 0);
		}

		protected virtual void OnReloadWithoutAmmo(Player player)
		{
		}

		protected virtual void DrawAimingLaser(SpriteBatch b, Player player)
		{
			const float TIME_SECONDS = .5f;

			float colourFadeIn = Util.Sigmoid((14.50865724f / (TIME_SECONDS * 500f)) * laserTimer) * laserBaseAlpha;
			float theta = AngleToMouse(player);
			
			b.DrawLineGradient(player.GetHeldItemPosition(), (Game1.WINDOW_WIDTH + 1f) / 4f, theta, laserColour * colourFadeIn, 0.5f, laserThickness);
		}
		
		protected Vector2 DirectionToMouse(Player player)
		{
			return (InputManager.MouseWorldPosition(Game1.MainCamera) - player.GetHeldItemPosition()).Normalized();
		}

		protected float AngleToMouse(Player player)
		{
			return DirectionToMouse(player).Angle();
		}

		protected Projectile SpawnProjectile(Player player, Projectile projectile)
		{
			projectile.Transform.Position = player.GetHeldItemPosition() + GetBulletSpawnPosition();
			player.CurrentArea.AddActor(projectile);
			return projectile;
		}
		
		public virtual void Fire(Player player)
		{
		}
		
		public virtual void FireWithoutAmmo(Player player)
		{
		}

		public virtual Vector2 GetBulletSpawnPosition()
		{
			return Vector2.Zero;
		}
	}
}
