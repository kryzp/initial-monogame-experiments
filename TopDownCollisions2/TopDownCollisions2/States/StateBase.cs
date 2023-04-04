using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownCollisions2.States
{
    public abstract class StateBase
    {
        protected Game1 Game;
        protected ContentManager Content;

        public StateBase(Game1 game, ContentManager content)
        {
            Game = game;
            Content = content;
        }

        public abstract void LoadContent();
        public abstract void UnloadContent();

        public abstract void Update(float deltaTime);
        public abstract void PostUpdate(float deltaTime);

        public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
        public abstract void DrawGUI(float deltaTime, SpriteBatch spriteBatch);
    }
}