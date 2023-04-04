using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ComponentArchitectureTest0.ECS.Components.Transform
{
	public class RotationComponent : Component
	{
		private float rotation; // <= Stored as radians

		public float RotationDegrees
		{
			get => MathHelper.ToDegrees(rotation);
			set => rotation = MathHelper.ToRadians(value);
		}

		public float RotationRadians
		{
			get => rotation;
			set => rotation = value;
		}

		public override void Create(params GraphicsResource[] resource)
		{
		}

		public override void Dispose()
		{
		}
	}
}
