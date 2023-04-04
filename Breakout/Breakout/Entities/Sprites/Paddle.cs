using System;
using Breakout.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout.Entities.Sprites
{
    public class Paddle : Sprite, ICollidable
    {
        private const float MOVE_SPEED = 220f;

        private Vector2 velocity = Vector2.Zero;
        private Vector2 movement = Vector2.Zero;
        
        public PlayerInput Input { get; set; }
        
        public Paddle(Texture2D tex)
            : base(tex)
        {
        }

        public override void Update(float deltaTime)
        {
            var keyboard = Keyboard.GetState();

            if(keyboard.IsKeyDown(Input.MoveLeft) || keyboard.IsKeyDown(Input.AltMoveLeft))
            {
                movement.X = -1f;
            }
            else if(keyboard.IsKeyDown(Input.MoveRight) || keyboard.IsKeyDown(Input.AltMoveRight))
            {
                movement.X = 1f;
            }
            else
            {
                movement.X = 0f;
            }

            velocity = movement * MOVE_SPEED;
            Position += velocity * deltaTime;
            
            Vector2 pos = Position;
            pos.X = MathHelper.Clamp(pos.X, texture.Width / 2f, Game1.WorldWidth - (texture.Width / 2f));
            Position = pos;
        }

        public void OnCollide(Sprite other)
        {
        }
    }
}