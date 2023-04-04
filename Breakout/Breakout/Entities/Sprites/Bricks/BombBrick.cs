using System.Linq;
using Breakout.States;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities.Sprites.Bricks
{
    public class BombBrick : Brick
    {
        public BombBrick(Texture2D tex)
            : base(tex)
        {
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            foreach(var brick in StatePlaying.Entities.Where(c => c is Brick))
            {
                StatePlaying.Score += 5;
                brick.IsRemoved = true;
            }
        }
    }
}