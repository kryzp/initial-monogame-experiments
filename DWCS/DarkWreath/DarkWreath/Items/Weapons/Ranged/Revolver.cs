using DarkWreath.Actors;
using DarkWreath.Items.Weapons.Ranged.Reloading;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Items.Weapons.Ranged
{
	public class Revolver : RangedWeapon
	{
		public const float BULLET_SPEED = 25f;
		
		private Texture2D revolverTexture;
		
		public Revolver()
			: base()
		{
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			
			revolverTexture = Game1.Content.Load<Texture2D>("items/weapons/ranged/revolver");

			this.RechamberTime = 40f;
			this.MaxAmmoInClip = 6;
			this.AmmoType = AmmoTypes.Magnum_357;
			this.ReloadPattern = new RevolverReloadPattern(revolverTexture);
		}

		public override void Fire(Player player)
		{
			base.Fire(player);

			var bullet = SpawnProjectile(player, ProjectileFactory.GetRevolverBullet());
			{
				bullet.Velocity = DirectionToMouse(player) * BULLET_SPEED;
			}

			//Game1.ScreenShake(64f);
		}
	}
}
