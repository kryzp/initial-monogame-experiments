using Breakout.States;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities.Sprites.Bricks
{
    public class ScoreBrick : Brick
    {
        public ScoreBrick(Texture2D tex)
            : base(tex)
        {
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StatePlaying.Score += 100;
        }
    }
}