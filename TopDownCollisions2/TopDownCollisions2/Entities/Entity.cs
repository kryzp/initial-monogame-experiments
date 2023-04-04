using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownCollisions2.Entities
{
    public class Entity : Component, ICloneable
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

        public object Clone()
        {
            var entity = this.MemberwiseClone() as Entity;
            return entity ?? throw new Exception("Failed to return Clone.");
        }
    }
}