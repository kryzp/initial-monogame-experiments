using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Pong.Managers;

namespace Pong.Sprites
{
    public class Ball : Sprite
    {
        private Vector2? startPosition = null;
        private float speed = 300f;
        
        public Ball(Texture2D texture)
            : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(startPosition == null)
            {
                startPosition = Position;
                Reset();
            }

            foreach(var sprite in MainGame.Sprites)
            {
                if(sprite == this) continue;

                if(this.Velocity.X > 0 && this.IsTouchingLeft(sprite) ||
                   this.Velocity.X < 0 && this.IsTouchingRight(sprite))
                    this.Velocity.X *= -1;
                if(this.Velocity.Y > 0 && this.IsTouchingUp(sprite) ||
                   this.Velocity.Y < 0 && this.IsTouchingDown(sprite))
                    this.Velocity.Y *= -1;
            }

            // Hit Top / Bottom
            if(Position.Y <= (int)(texture.Height / 2) ||
               Position.Y + (int)(texture.Height / 2) >= MainGame.ScreenHeight)
            {
                Velocity.Y *= -1;
            }
            
            // Hit Left (Point to Right)
            if(Position.X <= 0)
            {
                MainGame.ScoreManager.Right++;
                Reset();
            }
            
            // Hit Right (Point to Left)
            if(Position.X + (int)(texture.Width / 2) >= MainGame.ScreenWidth)
            {
                MainGame.ScoreManager.Left++;
                Reset();
            }
            
            Position += Velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void Reset()
        {
            var direction = MainGame.Random.Next(0, 4);
            switch(direction)
            {
                case 0:
                    Velocity = new Vector2(1, 1);
                    break;
                case 1:
                    Velocity = new Vector2(1, -1);
                    break;
                case 2:
                    Velocity = new Vector2(-1, -1);
                    break;
                case 3:
                    Velocity = new Vector2(-1, 1);
                    break;
                default:
                    Velocity = new Vector2(1, 0); // Just moves right if random fails
                    break;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}