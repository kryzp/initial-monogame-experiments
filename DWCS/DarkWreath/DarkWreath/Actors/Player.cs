using System.Collections.Generic;
using DarkWreath.Items;
using DarkWreath.Items.Weapons.Ranged;
using DarkWreath.Math;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Actors
{
	public class Player : AliveActor
	{
		public const float MOVE_SPEED = 0.2f;
		
		public Item CurrentItem { get; set; }
		public Vector2 HeldItemPosition;

		public Dictionary<AmmoTypes, int> AmmoCounts { get; set; }

		public Player()
		{
			AmmoCounts = new Dictionary<AmmoTypes, int>();
			
			Sprite = new AnimatedSprite(Game1.Content.Load<Texture2D>("temp"));
			Sprite.SetCurrentAnimation(new List<AnimationFrame>()
			{
				new AnimationFrame(Rectangle.Empty, 0, false)
			});

			Transform.Origin = new Vector2(8, 32f);

			CurrentItem = new Revolver();
			//CurrentItem = new Crossbow();
			AddAmmoOfType(AmmoTypes.Magnum_357, 100);
			AddAmmoOfType(AmmoTypes.Arrow, 50);

			Collider = new Collider(new RectangleF(0f, 24f, 16f, 8f));
			
			HeldItemPosition = new Vector2(0f, -10f);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			var inp = Game1.PlayerInput.GetState();
			Vector2 delta = inp.Movement;
			
			//if (delta != Vector2.Zero)
			//	Sprite.AnimateForward(gameTime);
			
			CurrentItem?.Update(gameTime, this);

			Move(delta * Time.Delta * MOVE_SPEED);
		}

		public override void Draw(GameTime gameTime, SpriteBatch b)
		{
			base.Draw(gameTime, b);
			
			CurrentItem?.Draw(b, this);
		}

		public override void DrawUI(GameTime gameTime, SpriteBatch b)
		{
			base.DrawUI(gameTime, b);

			CurrentItem?.DrawUI(b, this);
		}

		public void AddAmmoOfType(AmmoTypes type, int count = 1)
		{
			if (!AmmoCounts.ContainsKey(type))
				AmmoCounts.Add(type, 0);

			AmmoCounts[type] += count;
		}
		
		public bool HasAmmoOfType(AmmoTypes type)
		{
			if (!AmmoCounts.ContainsKey(type))
				return false;

			return AmmoCounts[type] > 0;
		}

		public Vector2 GetHeldItemPosition()
		{
			return HeldItemPosition + Transform.Position;
		}
	}
}
