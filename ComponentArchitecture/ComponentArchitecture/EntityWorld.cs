using System;
using System.Collections.Generic;

namespace ComponentArchitecture
{
	public class EntityWorld
	{
		private List<Entity> m_entities = new List<Entity>();

		public void Update()
		{
			foreach (Entity e in m_entities)
			{
				e.Update();
			}
		}

		public void Draw()
		{
			foreach (Entity e in m_entities)
			{
				e.Draw();
			}
		}

		public void Refresh()
		{
			m_entities.RemoveAll(e => !e.IsActive);
		}

		public Entity CreateEntity()
		{
			Entity entity = new Entity();
			m_entities.Add(entity);
			return entity;
		}

		public void RemoveEntity(UInt32 id)
		{
			m_entities.RemoveAll(e => e.ID == id);
		}
	}
}
