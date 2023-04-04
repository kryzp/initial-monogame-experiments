using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComponentArchitectureTest0.ECS.Components.Physics
{
	public class VelocityComponent : Component
	{
		public float X { get; set; }
		public float Y { get; set; }

		public float XA { get; set; }
		public float YA { get; set; }

		public override void Create(params GraphicsResource[] resource)
		{
		}

		public override void Dispose()
		{
		}
	}
}
