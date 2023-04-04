using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SandboxRider.Controls
{
    public class Button : Component
    {
        #region Fields

        private MouseState currentMouse;
        private MouseState prevMouse;

        private Texture2D texture;
        private SpriteFont font;
        
        private bool isHovering;
        
        #endregion
        
        #region Properties

        public EventHandler Click;
        public bool Clicked { get; private set; }
        public float Layer { get; set; }

        public Vector2 Origin
        {
            get
            {
                return new Vector2(texture.Width / 2, texture.Height / 2);
            }
        }
        
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X - ((int)Origin.X), (int)Position.Y - ((int)Origin.Y), texture.Width, texture.Height);
            }
        }

        public string Text;

        #endregion
        
        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            this.texture = texture;
            this.font = font;
            PenColour = Color.Black;
        }

        public override void Update(GameTime gameTime)
        {
            prevMouse = currentMouse;
            currentMouse = Mouse.GetState();
            
            var mouseRect = new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);

            isHovering = false;

            if(mouseRect.Intersects(Rectangle))
            {
                isHovering = true;

                if(currentMouse.LeftButton == ButtonState.Released && prevMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if(isHovering)
                colour = Color.Gray;
            
            spriteBatch.Draw(texture, Position, null, colour, 0f, Origin, 1f, SpriteEffects.None, Layer);

            if(!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (font.MeasureString(Text).Y / 2);
                
                spriteBatch.DrawString(font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer + 0.01f);
            }
        }
        
        #endregion
    }
}