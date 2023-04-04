using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownCollisions2.Models.Player;
using TopDownCollisions2.States;

namespace TopDownCollisions2.Entities.Sprites
{
    public class Player : Sprite, ICollidable
    {
        private static float MOVE_SPEED = 100f;

        private Vector2 movement;
        private Vector2 velocity;

        public PlayerInput Input { get; set; }

        public Player(Texture2D tex)
            : base(tex)
        {
        }

        public override void Update(float deltaTime)
        {
            bool KeyDown(Keys key) => Keyboard.GetState().IsKeyDown(key);

            if(KeyDown(Keys.A)) movement.X = -1;
            else if(KeyDown(Keys.D)) movement.X = 1;
            else movement.X = 0;
            if(KeyDown(Keys.W)) movement.Y = -1;
            else if(KeyDown(Keys.S)) movement.Y = 1;
            else movement.Y = 0;
            
            velocity = MOVE_SPEED * movement * deltaTime;

            HandleCollisions();
            
            Position += velocity;
        }

        public void OnCollide(Sprite other)
        {
            if(other is Solid)
            {
                while(Intersects(other, Vector2.Zero))
                {
                    Position -= new Vector2(Math.Sign(velocity.X), Math.Sign(velocity.Y));
                }
            }
        }

        private void HandleCollisions()
        {
	        foreach(Sprite solid in StatePlaying.Entities.Where(c => c is Solid))
	        {
                if(Intersects(solid, velocity))
                {
                    while(!Intersects(solid, new Vector2(Math.Sign(velocity.X), Math.Sign(velocity.Y))))
                    {
                        Position += new Vector2(Math.Sign(velocity.X), Math.Sign(velocity.Y));
                    }

                    velocity = Vector2.Zero;
                }
	        }

        }

        private void FuckingDontPlease(bool X)
        {
            Position -= velocity;
        }
    }
}