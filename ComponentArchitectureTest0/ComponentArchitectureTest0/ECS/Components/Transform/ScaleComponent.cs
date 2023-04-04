using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComponentArchitectureTest0.ECS.Components.Transform
{
	public class ScaleComponent : Component
	{
		public float X { get; set; }
		public float Y { get; set; }

		public override void Create(params GraphicsResource[] resource)
		{
		}

		public override void Dispose()
		{
		}
	}
}
