using System;

namespace ComponentArchitecture
{
	public class VelocityComponent : Component
	{
		private PositionComponent p;

		public float X, Y;
		public float XA, YA;

		public override void Init()
		{
			p = (PositionComponent)Owner.GetComponent<PositionComponent>();
		}

		public override void Update()
		{
			p.X += X;
			p.Y += Y;
			X += XA;
			Y += YA;
		}
	}
}
