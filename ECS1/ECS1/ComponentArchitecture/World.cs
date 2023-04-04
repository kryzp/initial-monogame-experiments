using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS1.ComponentArchitecture
{
	public class World
	{
		private static Dictionary<int, Entity> entities = new Dictionary<int, Entity>();

		public void Update(float deltaTime)
		{
			foreach (Entity entity in entities.Values)
			{
				entity.Update(deltaTime);
			}
		}

		public void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
			foreach (Entity entity in entities.Values)
			{
				entity.Draw(deltaTime, spriteBatch);
			}
		}

		public static Entity GetEntityViaID(int id)
		{
			return (entities[id]);
		}

		public void AddEntity(Entity entity)
		{
			entities.Add(entities.Count, entity);
		}
	}
}
