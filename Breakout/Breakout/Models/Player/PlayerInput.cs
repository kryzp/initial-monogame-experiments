using Microsoft.Xna.Framework.Input;

namespace Breakout.Models
{
    public class PlayerInput
    {
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        
        public Keys AltMoveLeft { get; set; }
        public Keys AltMoveRight { get; set; }
    }
}