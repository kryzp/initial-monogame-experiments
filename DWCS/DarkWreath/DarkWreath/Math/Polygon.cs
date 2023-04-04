using System;
using Microsoft.Xna.Framework;

namespace DarkWreath.Math
{
	public class Polygon
	{
		public Vector2[] Vertices;

		public Polygon()
		{
		}

		public Polygon(Vector2[] vertices)
		{
			this.Vertices = new Vector2[vertices.Length];

			for (int i = 0; i < vertices.Length; i++)
				this.Vertices[i] = vertices[i];
		}

		public static bool AxisOverlaps(Polygon a, Polygon b, Vector2 axis, ref float amount)
		{
			float minA = 0f, maxA = 0f;
			float minB = 0f, maxB = 0f;

			a.Project(axis, ref minA, ref maxA);
			b.Project(axis, ref minB, ref maxB);

			if (MathF.Abs(minB - maxA) < MathF.Abs(maxB - minA))
				amount = minB - maxA;
			else
				amount = maxB - minA;
			
			return (
				(minA < maxB) &&
				(maxA > minB)
			);
		}

		public void Project(Vector2 axis, ref float min, ref float max)
		{
			float dot = Vector2.Dot(Vertices[0], axis);
			min = dot;
			max = dot;

			for (int i = 0; i < Vertices.Length; i++)
			{
				dot = Vector2.Dot(Vertices[i], axis);
				min = MathF.Min(dot, min);
				max = MathF.Max(dot, max);
			}
		}

		public void ForeachVertex(Action<int, Vector2, Vector2> fn)
		{
			for (int i = 0; i < Vertices.Length; i++)
			{
				Vector2 curr = Vertices[i];
				Vector2 next = Vertices[((i + 1) >= Vertices.Length) ? 0 : (i + 1)];
				fn(i, curr, next);
			}
		}
	}
}
