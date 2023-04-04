using Microsoft.Xna.Framework.Input;

namespace Asteroids.Models
{
    public class Input
    {
        public Keys Thrust { get; set; }
        public Keys Shoot { get; set; }
        
        public Keys TurnLeft { get; set; }
        public Keys TurnRight { get; set; }
    }
}