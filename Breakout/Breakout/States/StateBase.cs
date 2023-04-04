using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.States
{
    public abstract class StateBase
    {
        protected Game1 Game;
        protected ContentManager Content;
        
        public StateBase(Game1 game, ContentManager content)
        {
            this.Game = game;
            this.Content = content;
        }

        public abstract void LoadContent();
        public abstract void UnloadContent();

        public abstract void Update(float deltaTime);
        public abstract void PostUpdate(float deltaTime);

        public abstract void Draw(float deltaTime, SpriteBatch spriteBatch);
        public abstract void DrawGUI(float deltaTime, SpriteBatch spriteBatch);
    }
}