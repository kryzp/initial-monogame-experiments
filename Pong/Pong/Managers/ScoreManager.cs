using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Pong.Managers
{
    public class ScoreManager
    {
        public int Left;
        public int Right;

        private readonly SpriteFont font;

        public ScoreManager(SpriteFont font)
        {
            this.font = font;
            
            this.Left = 0;
            this.Right = 0;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // TODO: Score Manager Throws fatal ERROR! Fix otherwise player left/right has no way of telling their score!
            //spriteBatch.DrawString(font, Left.ToString(), new Vector2((int)(MainGame.ScreenWidth * 1/4), 100), Color.White);
            //spriteBatch.DrawString(font, Right.ToString(), new Vector2((int)(MainGame.ScreenWidth *  3/4), 100), Color.White);
        }
    }
}