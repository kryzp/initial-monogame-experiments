using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities
{
    public class Entity : Component
    {
        public bool IsRemoved { get; set; }

        public List<Entity> Children;
        public Entity Parent;

        public Entity()
        {
            Children = new List<Entity>();
        }
        
        public override void Update(float deltaTime)
        {
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
        }
    }
}