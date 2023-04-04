using Microsoft.Xna.Framework.Input;

namespace ARPG.Models.Sprites.Input.Player
{
    public class PlayerInput
    {
        #region Movement
        
        public Keys MoveUp { get; set; }
        public Keys MoveDown { get; set; }
        
        public Keys MoveLeft { get; set; }
        public Keys MoveRight { get; set; }
        
        #endregion Movement
    }
}