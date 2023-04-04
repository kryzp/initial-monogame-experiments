using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spellpath.Actors;
using System;
using System.Collections;
using Spellpath.Input;

namespace Spellpath.Tools
{
	public class ElectrodeRifle : Tool
	{
		private Vector2 m_initialPosition;
		private float m_currTheta = 0f;

		private int m_selectedMode = 0;
		private float[] m_circleLerps = new float[7];

		private float m_animationYOffset;
		private float m_animationAlpha;

		public Texture2D ElectrodeRifleTexture;
		public float ChargedPercentage = 0f;

		public ElectrodeRifle()
		{
			ElectrodeRifleTexture = Game1.Content.Load<Texture2D>("weapons\\electrode_rifle");
		}

		// B-ZAAAAAP
		public override void DoFunction(Player player)
		{
			base.DoFunction(player);
		}

		private IEnumerator DoFunctionDelayed(Player player)
		{
			yield return new WaitForSeconds(5f);
			System.Diagnostics.Debug.WriteLine("E");
			DoFunction(player);
		}

		public override void Update(Player player, GameTime time)
		{
			base.Update(player, time);

			// responsible for releasing charged up electricity
			if (InputManager.IsReleased(MouseButton.Left))
			{
				player.StartCoroutine(DoFunctionDelayed(player));
				DoFunction(player);
			}

			// responsible for charging electricity
			else if (InputManager.IsDown(MouseButton.Left))
			{
			}

			// set initial reference position
			else if (InputManager.IsPressed(MouseButton.Right))
			{
				m_initialPosition = InputManager.MouseScreenPosition();
				UpdateCirclePositionsInstant();
			}

			// responsible for dial logic / tuning / angle
			else if (InputManager.IsDown(MouseButton.Right))
			{
				m_animationAlpha = MathHelper.Lerp(m_animationAlpha, 1f, 0.25f);
				m_animationYOffset = MathHelper.Lerp(m_animationYOffset, 0f, 0.25f);

				UpdateDialPosition();
				UpdateCirclePositions();
			}
			else
			{
				m_animationAlpha = 0f;
				m_animationYOffset = 10f;
			}
		}
		
		// responsible for drawing the dial gui
		public override void DrawUI(Player player, GameTime time, SpriteBatch b, float layerDepth)
		{
			base.DrawUI(player, time, b, layerDepth);

			if (InputManager.IsDown(MouseButton.Right))
			{
				// draw dial
				b.Draw(
					ElectrodeRifleTexture,
					m_initialPosition + new Vector2(0f, m_animationYOffset),
					new Rectangle(0, 0, 19, 20),
					Color.White * m_animationAlpha,
					0f,
					new Vector2(9.5f, 9.5f),
					Vector2.One * 8f,
					SpriteEffects.None,
					layerDepth
				);
				
				// draw pointer
				b.Draw(
					ElectrodeRifleTexture,
					m_initialPosition + new Vector2(0f, m_animationYOffset),
					new Rectangle(20, 5, 3, 14),
					Color.White * m_animationAlpha,
					m_currTheta + MathF.PI/2f,
					new Vector2(1.5f, 8.5f),
					Vector2.One * 8f,
					SpriteEffects.None,
					layerDepth
				);

				// draw indicator circles
				for (int i = 0; i < 7; i++)
				{
					float theta = i * MathF.PI / 6f;

					Vector2 circleOffsetVector = new Vector2(
						MathF.Cos(theta),
						-MathF.Sin(theta)
					) * 90f * (1f + (m_circleLerps[i] / 8f));

					b.Draw(
						ElectrodeRifleTexture,
						m_initialPosition + circleOffsetVector,
						new Rectangle(19, 0, 2, 2),
						((m_selectedMode != i) ? Color.White : new Color(133, 234, 226, 255)) * m_animationAlpha,
						0f,
						new Vector2(1, 1),
						Vector2.One * 8f * ((m_circleLerps[i] + 1f) / 2f),
						SpriteEffects.None,
						layerDepth
					);
				}
			}
		}

		private void UpdateDialPosition()
		{
			var dDir = InputManager.MouseScreenPosition() - m_initialPosition;

			if (dDir != Vector2.Zero)
			{
				const float epsilon = 0.00005f;
				
				float dDirTheta = MathF.Atan2(dDir.Y, dDir.X);
				m_selectedMode = MathHelper.Clamp((int)MathF.Round(dDirTheta / -MathF.PI * 6), 0, 6);
				float snappedAngle = m_selectedMode * (MathF.PI / 6f);
				if (dDirTheta > 0f)
				{
					if (dDirTheta > MathF.PI / 2f)
					{
						snappedAngle = MathF.PI - epsilon; // small value to prevent it getting stuck when snapped angle is completely 0 or PI
						m_selectedMode = 6;
					}
					else if (dDirTheta <= MathF.PI / 2f)
					{
						snappedAngle = epsilon; // small value to prevent it getting stuck when snapped angle is completely 0 or PI
						m_selectedMode = 0;
					}
				}
				Vector2 currDir = new Vector2(MathF.Cos(m_currTheta), MathF.Sin(m_currTheta));
				currDir = Vector2.Lerp(currDir, new Vector2(MathF.Cos(snappedAngle), -MathF.Sin(snappedAngle)), 0.25f);
				m_currTheta = MathF.Atan2(currDir.Y, currDir.X);
			}
		}

		private void UpdateCirclePositions(float speed = 0.25f)
		{
			for (int i = 0; i < 7; i++)
			{
				m_circleLerps[i] = MathHelper.Lerp(
					m_circleLerps[i],
					(m_selectedMode == i) ? 1f : 0f,
					speed
				);
			}
		}

		private void UpdateCirclePositionsInstant()
		{
			UpdateCirclePositions(1f);
		}
	}
}
