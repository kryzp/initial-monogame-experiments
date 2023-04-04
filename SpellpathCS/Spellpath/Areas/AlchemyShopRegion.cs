using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spellpath.Areas
{
    // alchemy shop exterior
    public class AlchemyShopRegion : GameArea
    {
        public Texture2D AlchemyShopTextures;

        public AlchemyShopRegion()
        {
            LoadMap("maps/alchemy_shop_region");

            AlchemyShopTextures = Game1.Content.Load<Texture2D>("buildings\\player_houses");
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(GameTime time, SpriteBatch b)
        {
            DrawAlchemyShop(b);

            base.Draw(time, b);
        }

        private void DrawAlchemyShop(SpriteBatch b)
        {
            Rectangle sourceRect = new Rectangle(0, 0, AlchemyShopTextures.Width, AlchemyShopTextures.Height);

            b.Draw(
                AlchemyShopTextures,
                new Vector2(0, 0),
                sourceRect,
                Color.White,
                0f,
                new Vector2(sourceRect.Width / 2f, sourceRect.Height),
                Vector2.One,
                SpriteEffects.None,
                0.0f
            );
        }
    }
}
