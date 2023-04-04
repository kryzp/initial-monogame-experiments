using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SandboxRider.Sprites;

namespace SandboxRider.States
{
    public class StatePlaying : StateBase
    {
        private SpriteFont font;

        private Paddle paddleLeft;
        private Paddle paddleRight;

        private Ball ball;

        private List<Sprite> sprites;
        
        public StatePlaying(Game1 game, ContentManager content)
            : base(game, content)
        {
        }

        public override void LoadContent()
        {
            sprites = new List<Sprite>();
            
            var paddleTex = content.Load<Texture2D>("Paddle");
            var ballTex = content.Load<Texture2D>("Ball");

            sprites.Add(new Paddle(paddleTex)
            {
                Colour = Color.Blue,
                Position = new Vector2(100, Game1.ScreenHeight / 2),
                Layer = 0.3f,
                Input = new Models.Input()
                {
                    Up = Keys.W,
                    Down = Keys.S
                },
                Score = new Models.Score()
                {
                    Value = 0
                }
            });
            
            sprites.Add(new Paddle(paddleTex)
            {
                Colour = Color.Orange,
                Position = new Vector2(Game1.ScreenWidth - 100, Game1.ScreenHeight / 2),
                Layer = 0.3f,
                Input = new Models.Input()
                {
                    Up = Keys.Up,
                    Down = Keys.Down
                },
                Score = new Models.Score()
                {
                    Value = 0
                }
            });
            
            sprites.Add(new Ball(ballTex)
            {
                Colour = Color.White,
                Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                Layer = 0.3f,
            });
        }

        public override void Update(GameTime gameTime)
        {
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.ChangeState(new StateMainMenu(game, content));
                
            foreach(var sprite in sprites) 
                sprite.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            var collidableSprites = sprites.Where(c => c is ICollidable);

            foreach(var A in collidableSprites)
            foreach(var B in collidableSprites)
            {
                if(A == B) continue;
                if(!A.CollisionArea.Intersects(B.CollisionArea)) continue;
                if(A.Intersects(B)) ((ICollidable) A).OnCollide(B);
            }

            int spriteCount = sprites.Count;
            for(int ii = 0; ii < spriteCount; ii++)
            {
                var sprite = sprites[ii];
                foreach(var child in sprite.Children)
                    sprites.Add(child);

                sprite.Children = new List<Sprite>();
            }

            for(int ii = 0; ii < sprites.Count; ii++)
            {
                if(sprites[ii].IsRemoved)
                {
                    sprites.RemoveAt(ii);
                    ii--;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);
            
            foreach(var sprite in sprites)
                sprite.Draw(gameTime, spriteBatch);
            
            spriteBatch.End();
        }
    }
}