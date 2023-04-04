using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout.States
{
    public class StateMainMenu : StateBase
    {
        private SpriteFont mainFont;

        private float sinTimer;
        private float fontSize;
        private float fontRot;

        private bool change;

        private Color col = Color.White;
        
        public StateMainMenu(Game1 game, ContentManager content)
            : base(game, content)
        {
        }

        public override void LoadContent()
        {
            mainFont = Content.Load<SpriteFont>("fonts/pixellari");
        }

        public override void UnloadContent()
        {
        }

        public override void Update(float deltaTime)
        {
            var keyboard = Keyboard.GetState();

            if(keyboard.IsKeyDown(Keys.Space))
            {
                change = true;
            }

            if(col == new Color(27, 31, 33))
            {
                Game.ChangeState(new StatePlaying(Game, Content));
            }
        }

        public override void PostUpdate(float deltaTime)
        {
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
        }

        public override void DrawGUI(float deltaTime, SpriteBatch spriteBatch)
        {
            string text = "Press SPACE to Play";
            float length = mainFont.MeasureString(text).X;
            float height = mainFont.MeasureString(text).Y;

            sinTimer++;
            
            spriteBatch.DrawString(
                mainFont,
                text,
                new Vector2(
                    Game1.ScreenWidth / 2f,
                    Game1.ScreenHeight / 2f
                ),
                col,
                fontRot,
                new Vector2(
                    length / 2f,
                    height / 2f
                ), 
                fontSize,
                SpriteEffects.None,
                0.85f
            );

            if(change)
            {
                float lerpSpeed = 0.15f;
                
                col.R = (byte)MathHelper.Lerp(col.R, 27, lerpSpeed);
                col.G = (byte)MathHelper.Lerp(col.G, 31, lerpSpeed);
                col.B = (byte)MathHelper.Lerp(col.B, 33, lerpSpeed);
            }
            
            fontSize = (float)Math.Sin(sinTimer / 10) / 10f + 2f;
            fontRot = (float)Math.Cos(sinTimer / 10) / 20f;
        }
    }
}