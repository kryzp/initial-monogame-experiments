using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities.Sprites.Bricks
{
    public class TimeBrick : Brick
    {
        public TimeBrick(Texture2D tex)
            : base(tex)
        {
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Game1.TimeScale = -1f;
        }
    }
}