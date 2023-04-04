using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxRider.Managers;

namespace SandboxRider.Sprites
{
    public class Ball : Sprite, ICollidable
    {
        private ScoreManager scoreManager;
        
        private Vector2 velocity = Vector2.Zero;
        private bool once = false;

        public Ball(Texture2D texture)
            : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            // Ball hits top or bottom
            if(Position.Y <= 0 || Position.Y >= Game1.ScreenHeight)
            {
                velocity.Y *= -1;
            }
            
            // Ball hits left or right
            if(Position.X <= 0)
            {
                if(once)
                {
                    once = false;
                    scoreManager.RightScore += 1;
                }
            }
            else if(Position.X >= Game1.ScreenWidth)
            {
                if(once)
                {
                    once = false;
                    scoreManager.LeftScore += 1;
                }
            }
            
            // Update Position
            Position = new Vector2(Position.X + velocity.X, Position.Y + velocity.Y);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public void OnCollide(Sprite sprite)
        {
            // Ball hits paddle
            if(sprite is Paddle)
            {
                velocity.X *= -1;
            }
        }
    }
}