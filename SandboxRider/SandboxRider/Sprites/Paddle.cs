using Microsoft.Xna.Framework.Graphics;
using SandboxRider.Models;

namespace SandboxRider.Sprites
{
    public class Paddle : Sprite
    {
        private float speed = 1f;
        
        public Input Input { get; set; }
        public Score Score { get; set; }

        public Paddle(Texture2D texture)
            : base(texture)
        {
        }
    }
}