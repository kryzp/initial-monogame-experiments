using Microsoft.Xna.Framework.Graphics;

namespace CollisionTest2.Entities.Util.States
{
    public class StateMachine
    {
        public IState CurrentState;

        public StateMachine(IState startState)
        {
            ChangeState(startState);
        }

        public void Update(float deltaTime)
        {
            CurrentState?.Update(deltaTime);
        }

        public void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            CurrentState?.Draw(deltaTime, spriteBatch);
        }
        
        public void ChangeState(IState state)
        {
            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Initialize();
        }
    }
}