using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spellpath.Areas
{
    public class Forest : GameArea
    {
        public Forest()
        {
            LoadMap("maps/forest");
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(GameTime time, SpriteBatch b)
        {
            base.Draw(time, b);
        }
    }
}
