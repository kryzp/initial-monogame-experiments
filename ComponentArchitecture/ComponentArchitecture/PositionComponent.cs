using System;

namespace ComponentArchitecture
{
	public class PositionComponent : Component
	{
		public float X, Y;

		public override void Update()
		{
			Console.WriteLine("{ " + X + ", " + Y + " }");
		}
	}
}
