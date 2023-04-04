using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.MousePattern
{
	public enum RotationDirection
	{
		Clockwise,
		AntiClockwise
	};
	
	public class RotationPattern : PatternSegment
	{
		class RotationPoint
		{
			public Vector2 Position;
			public bool Hit;

			public RotationPoint(Vector2 pos, bool hit)
			{
				this.Position = pos;
				this.Hit = hit;
			}
		}
		
		public const int ACCURACY = 80;
		
		// (position, has been hit)
		private readonly List<RotationPoint> points = new List<RotationPoint>();
		private int currentPointIndex;
		private RotationPoint currentPoint => points[currentPointIndex];
		private readonly float leeway;

		public readonly Vector2 Centre;
		public readonly RotationDirection Direction;
		public readonly float Radius;

		public RotationPattern(RotationDirection dir, float radius, float leeway = 48f)
			: this(dir, radius, leeway, Vector2.Zero)
		{
		}
		
		public RotationPattern(RotationDirection dir, float radius, float leeway, Vector2 centre)
		{
			this.Direction = dir;
			this.Radius = radius;
			this.Centre = centre;
			
			this.leeway = leeway;

			this.currentPointIndex = 0;
			GeneratePoints();
		}

		// TODO :: ADDING / SUBTRACTING CENTRES MAY BE THE WRONG WAY ROUND HAVENT TESTED THIS :: TODO
		
		public override void DebugDraw(SpriteBatch b)
		{
			int i = 0;
			
			foreach (var point in points)
			{
				i++;
				
				b.Draw(
					Game1.WhiteCircle,
					point.Position + StartPosition + Centre,
					null,
					(point.Hit ? Color.Red : Color.White) * ((points.Count - i) / (float)points.Count),
					0f,
					new Vector2(64, 64),
					(leeway * 2f) / 128f,
					SpriteEffects.None,
					0.8f
				);
			}
		}

		public override Vector2 GetEndPosition()
		{
			return Vector2.Zero;
		}

		public override bool Update()
		{
			var relativePosition = MousePosition - StartPosition - Centre;
			var delta = currentPoint.Position - relativePosition;
			
			if (delta.Length() <= leeway)
			{
				currentPoint.Hit = true;
				currentPointIndex++;
			}
			
			foreach (var point in points)
			{
				if (!point.Hit)
					return false;
			}

			return true;
		}

		public override void Reset()
		{
			currentPointIndex = 0;
			points.ForEach(x => x.Hit = false);
		}

		private void GeneratePoints()
		{
			float dtheta = MathHelper.Tau / (float)ACCURACY;
			float to = MathHelper.Tau;
			float start = 0f;

			if (Direction == RotationDirection.AntiClockwise)
			{
				dtheta *= -1f;
				to = 0f;
				start = MathHelper.Tau;
			}

			for (float theta = start; (Direction == RotationDirection.Clockwise) ? (theta < to) : (theta > to); theta += dtheta)
			{
				Vector2 position = new Vector2(
					MathF.Cos(theta - MathHelper.PiOver2) * Radius,
					MathF.Sin(theta - MathHelper.PiOver2) * Radius
				);

				points.Add(new RotationPoint(position, false));
			}
		}

		public override float Progress()
		{
			return (float)currentPointIndex / (float)points.Count;
		}
	}
}
