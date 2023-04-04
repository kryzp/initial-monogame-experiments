using ComponentArchitectureTest0.ECS.Components;
using ComponentArchitectureTest0.ECS.Components.Tuples;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ComponentArchitectureTest0.ECS.Systems
{
	public abstract class EntitySystem
	{
		public EntitySystem()
		{
		}

		public abstract void Update(float deltaTime);
		public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
	}
}
