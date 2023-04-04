using System.Collections.Generic;
using ComponentArchitectureTest0.ECS.Systems;
using ComponentArchitectureTest0.ECS.Entities;
using Microsoft.Xna.Framework.Graphics;
using ComponentArchitectureTest0.ECS.Components;
using System.Linq;
using ComponentArchitectureTest0.ECS.Components.Tuples;

namespace ComponentArchitectureTest0.ECS
{
	public class World
	{
		private static List<ComponentSet> components;
		private static List<Entity> entities;
		private static List<EntitySystem> systems;

		public World()
		{
			entities = new List<Entity>();
			components = new List<ComponentSet>();
			systems = new List<EntitySystem>();
		}

		public void Update(float deltaTime)
		{
			foreach (var system in systems)
			{
				system.Update(deltaTime);
			}
		}

		public void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
			foreach (var system in systems)
			{
				system.Draw(deltaTime, spriteBatch);
			}
		}

		#region Entity

		public Entity CreateEntity()
		{
			Entity e = new Entity { ID = entities.Count };
			entities.Add(e);
			components.Add(new ComponentSet());
			return e;
		}

		public void AddComponent(Entity entity, Component component)
		{
			components[entity.ID].Components.Add(component);
		}

		#endregion

		#region Component

		public ComponentSet GetEntityComponentSet(Entity entity)
		{
			return (components[entity.ID]);
		}

		public static List<ComponentSet> GetComponentSets()
		{
			return components;
		}

		#endregion

		#region System

		public void AddSystem(EntitySystem system)
		{
			systems.Add(system);
		}

		public void RemoveSystem(EntitySystem system)
		{
			systems.Remove(system);
		}

		#endregion
	}
}
