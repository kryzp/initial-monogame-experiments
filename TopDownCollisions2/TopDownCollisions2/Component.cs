using Microsoft.Xna.Framework.Graphics;

namespace TopDownCollisions2
{
    public abstract class Component
    {
        public abstract void Update(float deltaTime);
        public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
    }
}