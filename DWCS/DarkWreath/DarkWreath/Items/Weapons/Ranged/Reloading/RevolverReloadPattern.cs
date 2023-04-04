using System;
using System.Collections.Generic;
using DarkWreath.Actors;
using DarkWreath.MousePattern;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Items.Weapons.Ranged.Reloading
{
	public class RevolverReloadPattern : ReloadPattern
	{
		public override void SetSegments()
		{
			Segments = new List<PatternSegment>()
			{
				new LinearMovementPattern(-RELOAD_DISTANCE, 0f),
				new RotationPattern(RotationDirection.Clockwise, 36f, /*24f*/36f, new Vector2(0f, (MathF.Exp(-1f) - MathF.Exp(-2f)) * 120f)),
				new LinearMovementPattern(+RELOAD_DISTANCE, 0f)
			};
		}
		
		private const float RELOAD_DISTANCE = 70f;
		private const float RELOAD_ROTATION = 360f;
		
		private Texture2D reloadTexture;
		
		private float targetChamberRotation;
		private float chamberRotationIntermediate;
		private float chamberRotation;
		private Vector2 chamberPosition;
		
		public RevolverReloadPattern(Texture2D revolverTexture)
			: base()
		{
			reloadTexture = revolverTexture;
			
			chamberRotation = 0f;
			chamberRotationIntermediate = 0f;
			targetChamberRotation = 0f;
		}

		public override void Started()
		{
			chamberRotation = 0f;
			chamberRotationIntermediate = 0f;
			targetChamberRotation = 0f;
		}

		public override void Finished()
		{
			base.Finished();

			chamberRotation = 0f;
			chamberRotationIntermediate = 0f;
			targetChamberRotation = 0f;
		}

		public override void Update(GameTime gameTime, Player player)
		{
			base.Update(gameTime, player);

			// chamber rotation
			{
				chamberRotation = Util.Spring(
					chamberRotation,
					targetChamberRotation,
					16f,
					0.2f,
					ref chamberRotationIntermediate
				);
			}

			// chamber position
			{
				chamberPosition = FirstReloadPosition;

				float progress = CurrentSegment.Progress();

				// linear movement out of chassis
				if (CurrentSegmentIndex == 0)
				{
					chamberPosition.X += -MathHelper.Clamp(progress * RELOAD_DISTANCE, 0f, RELOAD_DISTANCE);
					chamberPosition.Y += (MathF.Exp(MathF.Max(0f, progress) - 2f) - MathF.Exp(-2f)) * 120f;

					targetChamberRotation = progress / 2f;
				}

				// rotation of chamber
				else if (CurrentSegmentIndex == 1)
				{
					chamberPosition.X += -RELOAD_DISTANCE;
					chamberPosition.Y += (MathF.Exp(-1f) - MathF.Exp(-2f)) * 120f;

					targetChamberRotation = MathHelper.ToRadians(
						progress * RELOAD_ROTATION
					) + 0.5f;
				}

				// linear movement back to chassis
				else if (CurrentSegmentIndex == 2)
				{
					chamberPosition.X += MathHelper.Clamp(progress * RELOAD_DISTANCE, 0f, RELOAD_DISTANCE) -  RELOAD_DISTANCE;
					chamberPosition.Y += ((MathF.Exp(-1f) - MathF.Exp(-2f)) -
					                      (MathF.Exp(MathF.Max(0f, progress) - 2f) - MathF.Exp(-2f))) * 120f;

					targetChamberRotation = -1f * progress / 2f + 0.5f + MathHelper.ToRadians(RELOAD_ROTATION);
				}
			}
		}

		public override void Draw(SpriteBatch b, Player player)
		{
			base.Draw(b, player);
			
			DrawMainChassis(b);
			DrawChamber(b);
		}

		private void DrawMainChassis(SpriteBatch b)
		{
			b.Draw(
				reloadTexture,
				FirstReloadPosition,
				new Rectangle(
					0,
					64,
					64,
					192
				),
				Color.White,
				0f,
				new Vector2(
					32f,
					40f
				),
				2f,
				SpriteEffects.None,
				0.5f
			);
		}

		private void DrawChamber(SpriteBatch b)
		{
			Transform chamberTransform = new Transform();
			{
				chamberTransform.Position = chamberPosition;
				chamberTransform.RotationRad = chamberRotation;
				chamberTransform.Origin = Vector2.One * 32f * 2f;
			}

			b.Draw(
				reloadTexture,
				chamberPosition,
				new Rectangle(0, 0, 64, 64),
				Color.White,
				chamberRotation,
				new Vector2(32f, 32f),
				2f,
				SpriteEffects.None,
				0.45f
			);

			for (int i = 0; i < RangedWeapon.CurrentAmmoInClip; i++)
			{
				Transform bulletTransform = new Transform();
				{
					bulletTransform.Origin = Vector2.One * 8f * 2f;

					var bpos = Vector2.Zero;
					
					switch (i)
					{
						case 0:
							bpos = new Vector2(32f, 12f);
							break;
						
						case 1:
							bpos = new Vector2(49f, 22f);
							break;
						
						case 2:
							bpos = new Vector2(49f, 42f);
							break;
						
						case 3:
							bpos = new Vector2(32f, 52f);
							break;
						
						case 4:
							bpos = new Vector2(15f, 42f);
							break;
						
						case 5:
							bpos = new Vector2(15f, 22f);
							break;
					}

					bulletTransform.Position = bpos * 2f;
				}

				Matrix finalMatrix = bulletTransform.Matrix * chamberTransform.Matrix;
				Vector3 pos;
				Quaternion rot;
				finalMatrix.Decompose(out _, out rot, out pos);
				Vector2 direction = Vector2.Transform(Vector2.UnitX, rot);
				float rotZ = MathF.Atan2(direction.Y, direction.X);

				b.Draw(
					reloadTexture,
					new Vector2(pos.X, pos.Y),
					new Rectangle(64, 0, 16, 16),
					Color.White,
					rotZ,
					Vector2.Zero,
					2f,
					SpriteEffects.None,
					0.46f
				);
			}
		}
	}
}
