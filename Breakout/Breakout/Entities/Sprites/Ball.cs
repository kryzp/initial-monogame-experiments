using System;
using Breakout.Entities.Sprites.Bricks;
using Breakout.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities.Sprites
{
    public class Ball : Sprite, ICollidable
    {
        private float moveSpeed = 125f;

        private int rot;
        
        private Vector2 direction;
        private Vector2 velocity;

        public Ball(Texture2D tex)
            : base(tex)
        {
            rot = -Game1.Random.Next((180 + 270) / 2, (270 + 320) / 2);

            direction.X = (float)Math.Sin(rot * 2 * (Math.PI / 360));
            direction.Y = (float)Math.Cos(rot * 2 * (Math.PI / 360));
        }

        public override void Update(float deltaTime)
        {
            // Bouncing Behaviour
            if(Position.X <= (texture.Width / 2f) || Position.X >= Game1.WorldWidth - (texture.Width / 2f))
                direction.X *= -1;
            if(Position.Y <= (texture.Height / 2f) || Position.Y >= Game1.WorldHeight - (texture.Height / 2f))
                direction.Y *= -1;

            // Update Position
            velocity = direction * moveSpeed;
            Position += velocity * deltaTime;
            
            if(Position.Y >= Game1.WorldHeight - (texture.Height / 2f))
            {
                IsRemoved = true;
            }
        }

        public void OnCollide(Sprite other)
        {
            if(other is Paddle)
            {
                moveSpeed += 10f;
                moveSpeed = Math.Min(moveSpeed, 350f); // Make sure moveSpeed doesn't go over 200f

                rot = -Game1.Random.Next((180 + 270) / 2, (270 + 320) / 2);

                direction.X = (float)Math.Cos(rot * 2 * (Math.PI / 360));
                direction.Y = (float)Math.Sin(rot * 2 * (Math.PI / 360));
                
                direction.Y *= -1;
            }

            if(other is Brick)
            {
                direction.Y *= -1;
                ((Brick)other).OnDestroy();
                other.IsRemoved = true;
            }
        }
    }
}