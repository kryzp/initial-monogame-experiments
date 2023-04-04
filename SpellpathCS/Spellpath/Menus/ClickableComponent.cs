using Microsoft.Xna.Framework;
using Spellpath.Input;

namespace Spellpath.Menus
{
    public class ClickableComponent
    {
        public Rectangle BoundingBox;

        public ClickableComponent()
        {
            this.BoundingBox = Rectangle.Empty;
        }

        public ClickableComponent(Rectangle box)
        {
            this.BoundingBox = box;
        }

        public bool ContainsPoint(Vector2 point)
		{
            return BoundingBox.Contains(point);
		}

        public bool HoveredOver()
        {
            return ContainsPoint(InputManager.MouseScreenPosition());
        }

        public bool Clicked()
        {
            return HoveredOver() && InputManager.IsPressed(MouseButton.Left);
        }

        public bool Released()
        {
            return HoveredOver() && InputManager.IsReleased(MouseButton.Left);
        }
    }
}
