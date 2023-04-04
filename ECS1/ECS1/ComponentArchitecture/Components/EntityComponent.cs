using Microsoft.Xna.Framework.Graphics;

namespace ECS1.ComponentArchitecture.Components
{
	public abstract class EntityComponent
	{
		public abstract void Update(float deltaTime);
		public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
	}
}
