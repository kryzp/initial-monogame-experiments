using Microsoft.Xna.Framework.Graphics;

namespace ECS1.ComponentArchitecture.Components.Transform
{
	public class PositionComponent : EntityComponent
	{
		public float X { get; set; }
		public float Y { get; set; }

		public override void Update(float deltaTime)
		{
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}
	}
}
