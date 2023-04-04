using System;
using DarkWreath.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.MousePattern
{
	public class LinearMovementPattern : PatternSegment
	{
		private readonly float angleLeeway; // amount in radians the mouse can deviate from the direction
		private readonly float lengthLeeway; // amount in pixels the mouse can deviate from the length
		
		public readonly Vector2 To;

		public LinearMovementPattern(float x, float y, float angleLeeway = MathHelper.Tau / 8f, float lengthLeeway = 48f)
		{
			this.To = new Vector2(x, y);

			this.angleLeeway = angleLeeway;
			this.lengthLeeway = lengthLeeway;
		}

		public override Vector2 GetEndPosition()
		{
			return this.To;
		}

		public override bool Update()
		{
			Vector2 delta = MousePosition - StartPosition;
			
			float toAngle = To.Angle();
			float toLength = To.Length();

			// :: TODO ::
			// angleLeeway doesnt work propely as it doesnt work for when the two sides are at 180deg so it flips all the way around the circle so it doesnt work
			// :: TODO ::
			
			float toAngleMax = toAngle + (angleLeeway / 2f);
			float toAngleMin = toAngle - (angleLeeway / 2f);
			bool inAngleRange = toAngleMin <= delta.Angle() && delta.Angle() <= toAngleMax;
			
			float toLengthMax = toLength + (lengthLeeway / 2f);
			float toLengthMin = toLength - (lengthLeeway / 2f);
			bool inLengthRange = toLengthMin <= delta.Length() && delta.Length() <= toLengthMax;

			return inAngleRange && inLengthRange;
		}

		public override void DebugDraw(SpriteBatch b)
		{
			b.DrawLine(StartPosition, To + StartPosition, Color.White, 0.8f, 1f);
		}

		public override float Progress()
		{
			return 1f - MathF.Abs((To - MousePosition + StartPosition).Length() / To.Length());
		}
	}
}
