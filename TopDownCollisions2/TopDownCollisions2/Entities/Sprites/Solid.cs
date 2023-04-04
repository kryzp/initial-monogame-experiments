using Microsoft.Xna.Framework.Graphics;

namespace TopDownCollisions2.Entities.Sprites
{
    public class Solid : Sprite, ICollidable
    {
        public Solid(Texture2D tex)
            : base(tex)
        {
        }
        
        public void OnCollide(Sprite other)
        {
        }
    }
}