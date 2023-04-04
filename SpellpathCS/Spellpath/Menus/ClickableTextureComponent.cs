using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spellpath.Menus
{
    public class ClickableTextureComponent : ClickableComponent
    {
        public Texture2D Texture;
        public float Scale;
        public Rectangle SourceRect;

        public ClickableTextureComponent()
            : base()
        {
            this.Texture = null;
            this.SourceRect = Rectangle.Empty;
            this.Scale = 4f;
        }

        public ClickableTextureComponent(Texture2D texture, Rectangle box, Rectangle sourceRect)
            : base(box)
        {
            this.Texture = texture;
            this.BoundingBox = box;
            this.SourceRect = sourceRect;
            this.Scale = 4f;
        }

        public void Draw(SpriteBatch b, Color colour, float layerDepth)
        {
            if (Texture != null)
            {
                b.Draw(
                    Texture,
                    new Vector2(
                        BoundingBox.X + (BoundingBox.Width / 2) * Scale,
                        BoundingBox.Y + (BoundingBox.Height / 2) * Scale
                    ),
                    SourceRect,
                    colour,
                    0.0f,
                    new Vector2(
                        SourceRect.Width / 2,
                        SourceRect.Height / 2
                    ),
                    Vector2.One * Scale,
                    SpriteEffects.None,
                    layerDepth
                );
            }
        }
    }
}
