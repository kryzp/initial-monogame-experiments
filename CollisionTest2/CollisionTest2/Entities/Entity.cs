using System.Collections.Generic;
using CollisionTest2;
using Microsoft.Xna.Framework.Graphics;

namespace CollisionTest2.Entities
{
    public abstract class Entity : IComponent
    {
        public List<Entity> Children { get; private set; }
        public Entity Parent { get; private set; }
        
        public bool IsRemoved { get; set; }

        public Entity()
        {
            Children = new List<Entity>();
        }
        
        public abstract void Update(float deltaTime);
        public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
    }
}