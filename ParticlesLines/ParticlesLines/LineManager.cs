using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ParticlesLines
{
	public class LineManager
	{
		private readonly float LINE_RANGE = 160f;
		private List<FloatingCircle> circles;

		public LineManager(List<FloatingCircle> circles)
		{
			this.circles = circles;
		}

		public void Update(float deltaTime)
		{
		}

		public void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
			foreach(var a in circles)
			{
				foreach(var b in circles)
				{
					float dist = Distance(a.Position, b.Position);

					if(dist <= LINE_RANGE)
					{
						float d = 1f - (dist / LINE_RANGE);
						DrawTools.DrawLine(
							spriteBatch,
							a.Position,
							b.Position,
							Color.White * (d / 2f),
							1
						);
					}
				}
			}
		}

		private float Distance(Vector2 p1, Vector2 p2)
		{
			return((float)(Math.Sqrt(
				(Math.Pow((p1.X - p2.X), 2) +
				(Math.Pow((p1.Y - p2.Y), 2))
			))));
		}
	}
}
