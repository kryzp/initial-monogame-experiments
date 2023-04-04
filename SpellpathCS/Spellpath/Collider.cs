using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using System;
using System.Collections.Generic;
using Spellpath.Actors;

namespace Spellpath
{
	public struct Hit
	{
		public Collider Other;
		public Vector2 Pushout;
		public bool Solid;

		public float GetDistanceFromOtherSquared(Collider me, bool centered = false)
		{
			if (centered)
				return Vector2.DistanceSquared(me.GetWorldBounds().Center, Other.GetWorldBounds().Center);
			else
				return Vector2.DistanceSquared(me.GetWorldBounds().TopLeft, Other.GetWorldBounds().TopLeft);
		}
	}

	public class Collider
	{
		private RectangleF worldBounds;
		private List<Vector2> axis;

		public Polygon Polygon;
		public Polygon WorldPolygon;

		public Actor Actor;
		public Transform Transform;

        public Collider()
        {
            Transform = new Transform();
			Polygon = new Polygon(new List<Vector2>());
			WorldPolygon = new Polygon(new List<Vector2>());
            worldBounds = new RectangleF();
            axis = new List<Vector2>();
        }

		public Collider(Actor parent)
			: this()
		{
			this.Actor = parent;
		}

        public Collider(Collider other)
		{
			this.worldBounds = other.worldBounds;
			this.axis = new List<Vector2>(other.axis);

			this.Polygon = new Polygon(other.Polygon.Vertices);
			this.WorldPolygon = new Polygon(other.WorldPolygon.Vertices);

			this.Actor = other.Actor;
			this.Transform = new Transform(other.Transform);
		}

		public Collider(Polygon polygon)
			: this()
		{
			MakePolygon(polygon);
		}

		public Collider(List<Vector2> vertices)
			: this()
		{
			MakePolygon(vertices);
		}

		public Collider(RectangleF rect)
			: this()
		{
			MakeRect(rect);
		}

		public Collider(float x, float y, float w, float h)
			: this()
		{
			MakeRect(x, y, w, h);
		}

		public void MakePolygon(Polygon polygon)
		{
			this.Polygon = new Polygon(polygon.Vertices);
			this.WorldPolygon = new Polygon(polygon.Vertices);

			for (int i = 0; i < polygon.Vertices.Length; i++)
				this.axis.Add(Vector2.Zero);

			UpdateWorldBounds();
		}

		public void MakePolygon(List<Vector2> vertices)
		{
			this.Polygon = new Polygon(vertices);
			this.WorldPolygon = new Polygon(vertices);

			for (int i = 0; i < vertices.Count; i++)
				this.axis.Add(Vector2.Zero);

			UpdateWorldBounds();
		}

		public void MakeRect(RectangleF rect)
		{
			MakeRect(rect.X, rect.Y, rect.Width, rect.Height);
		}

		public void MakeRect(float x, float y, float w, float h)
		{
			MakePolygon(new Polygon(new List<Vector2>()
			{
				new Vector2(0, 0),
				new Vector2(w, 0),
				new Vector2(w, h),
				new Vector2(0, h)
			}));

			Transform.Position = new Vector2(x, y);
		}

		public Collider GetOffset(Vector2 offset)
		{
			Collider collider = new Collider(this);
			collider.Transform.Position += offset;
			return collider;
		}

		public RectangleF GetWorldBounds()
		{
			UpdateWorldBounds();
			return worldBounds;
		}

		public bool Overlaps(Collider collider, out Vector2 pushout)
		{
			return Collider.ConvexToConvex(this, collider, out pushout);
		}

		public Matrix GetFullTransformMatrix(bool centered = false)
		{
			Matrix result = Transform.GetMatrix();
			if (Actor != null) result *= Actor.Transform.GetMatrix();
			return result;
		}

		private static bool AxisOverlaps(Collider a, Collider b, Vector2 axis, ref float amount)
		{
			float min0 = 0f, max0 = 0f;
			float min1 = 0f, max1 = 0f;

			a.Project(axis, ref min0, ref max0);
			b.Project(axis, ref min1, ref max1);

			if (MathF.Abs(min1 - max0) < MathF.Abs(max1 - min0))
				amount = min1 - max0;
			else
				amount = max1 - min0;

			return (
				min0 < max1 &&
				max0 > min1
			);
		}

		private void Project(Vector2 axis, ref float min, ref float max)
		{
			WorldPolygon.Project(axis, ref min, ref max);
		}

		private void UpdateWorldBounds()
		{
			int vertexCount = Polygon.Vertices.Length;

			Matrix mat = Transform.GetMatrix();

			if (Actor != null)
				mat *= Actor.Transform.GetMatrix();

			// Update Axis and Points
			{
				Vector2 firstVertex = WorldPolygon.Vertices[0];

				for (int i = 0; i < vertexCount; i++)
				{
					Vector2 currentVertex = WorldPolygon.Vertices[i];

					Vector2 nextVertex = firstVertex;
					if (i+1 < vertexCount)
						nextVertex = WorldPolygon.Vertices[i+1];

					WorldPolygon.Vertices[i] = Vector2.Transform(Polygon.Vertices[i], mat);

					axis[i] = (nextVertex - currentVertex).NormalizedCopy();
					axis[i] = new Vector2(-axis[i].Y, axis[i].X);
				}
			}

			// Update World Bounds
			{
				float minx = Single.MaxValue;
				float maxx = -Single.MaxValue;
				float miny = Single.MaxValue;
				float maxy = -Single.MaxValue;

				for (int i = 0; i < vertexCount; i++)
				{
					float xx = WorldPolygon.Vertices[i].X;
					float yy = WorldPolygon.Vertices[i].Y;
					minx = MathF.Min(minx, xx);
					maxx = MathF.Max(maxx, xx);
					miny = MathF.Min(miny, yy);
					maxy = MathF.Max(maxy, yy);
				}

				worldBounds.X = minx;
				worldBounds.Y = miny;
				worldBounds.Width = maxx - minx;
				worldBounds.Height = maxy - miny;
			}
		}

		private static bool ConvexToConvex(Collider a, Collider b, out Vector2 pushout)
		{
			pushout = Vector2.Zero;

			a.UpdateWorldBounds();
			b.UpdateWorldBounds();

			if (!a.worldBounds.Intersects(b.worldBounds))
				return false;

			float length = Single.MaxValue;

			for (int i = 0; i < a.axis.Count; i++)
			{
				var axis = a.axis[i];
				float amount = 0.0f;

				if (!Collider.AxisOverlaps(a, b, axis, ref amount))
					return false;
				
				if (MathF.Abs(amount) < length)
				{
					pushout = axis * amount;
					length = MathF.Abs(amount);
				}
			}

			for (int i = 0; i < b.axis.Count; i++)
			{
				var axis = b.axis[i];
				float amount = 0.0f;

				if (!Collider.AxisOverlaps(a, b, axis, ref amount))
					return false;

				if (MathF.Abs(amount) < length)
				{
					pushout = axis * amount;
					length = MathF.Abs(amount);
				}
			}

			return true;
		}
	}
}
