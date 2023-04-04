using ECS1.ComponentArchitecture.Components;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ECS1.ComponentArchitecture
{
	public class Entity
	{
		private List<EntityComponent> components = new List<EntityComponent>();

		public void Update(float deltaTime)
		{
			foreach (EntityComponent component in components)
			{
				component.Update(deltaTime);
			}
		}

		public void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
			foreach (EntityComponent component in components)
			{
				component.Draw(deltaTime, spriteBatch);
			}
		}

		public void AddComponent(EntityComponent component)
		{
			components.Add(component);
		}

		public void RemoveComponent(EntityComponent component)
		{
			components.Remove(component);
		}

		public void RemoveAllComponents(EntityComponent component)
		{
			components.RemoveAll(c => c.GetType() == component.GetType());
		}

		// Still experimenting with generic functions.. not too different from C++ templates right?
		public EntityComponent GetComponent<T>(T component)
		{
			return (components.Find(c => c.GetType() == component.GetType()));
		}
	}
}
