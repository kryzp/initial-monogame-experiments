using Breakout.States;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities.Sprites.Bricks
{
    public class Brick : Sprite, ICollidable
    {
        public Brick(Texture2D tex)
            : base(tex)
        {
        }

        public virtual void OnDestroy()
        {
            StatePlaying.Score += 5;
        }

        public void OnCollide(Sprite other)
        {
        }
    }
}