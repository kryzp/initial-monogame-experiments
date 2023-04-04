using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Models;

namespace Pong.Sprites
{
    public class Paddle : Sprite
    {
        private float speed = 350f;

        public Input Input { get; set; }
        
        public Color Colour { get; set; }

        public Paddle(Texture2D texture)
            : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Input.Up)) Velocity.Y = -1;
            else if(Keyboard.GetState().IsKeyDown(Input.Down)) Velocity.Y = 1;
            else Velocity = Vector2.Zero;

            Position += Velocity * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = Vector2.Clamp(Position,
                new Vector2(Position.X, (int)(texture.Height / 2)),
                new Vector2(Position.X, MainGame.ScreenHeight - (int)(texture.Height / 2)));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, null, Colour, 0f, Origin, 1f, SpriteEffects.None, 0f);
        }
    }
}