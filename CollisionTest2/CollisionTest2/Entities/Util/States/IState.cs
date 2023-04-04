using Microsoft.Xna.Framework.Graphics;

namespace CollisionTest2.Entities.Util.States
{
    public interface IState
    {
        void Initialize();
        void Exit();
        
        void Update(float deltaTime);
        void Draw(float deltaTime, SpriteBatch spriteBatch);
    }
}