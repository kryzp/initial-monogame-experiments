using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameMirror
{
    public class Mirror
    {
        public const float REFLECTIVITY = 0.65f;
        public static readonly Point POSITION = new Point(50, 26*4);

        public Mirror()
        {
        }

        public void draw(SpriteBatch b)
        {
            b.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            b.Draw(Game1.mirrorSprite, POSITION.ToVector2(), null, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
            b.End();

            var scissorRectBackup = b.GraphicsDevice.ScissorRectangle;
            b.GraphicsDevice.ScissorRectangle = new Rectangle(POSITION.X + 16, POSITION.Y + 16, 72, 48);

            b.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, rasterizerState: new RasterizerState() { ScissorTestEnable = true });
            Game1.cube.draw(b, new Vector2(0, -68), REFLECTIVITY);
            b.End();

            b.GraphicsDevice.ScissorRectangle = scissorRectBackup;
        }
    }
}
