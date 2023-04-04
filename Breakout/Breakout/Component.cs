using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout
{
    public abstract class Component
    {
        public abstract void Update(float deltaTime);
        public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
    }
}