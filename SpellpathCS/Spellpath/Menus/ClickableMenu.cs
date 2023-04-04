using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Spellpath.Menus
{
    public abstract class ClickableMenu
    {
        public ClickableMenu()
		{
		}

        public virtual void Update(GameTime time) { }
        public virtual void Draw(GameTime time, SpriteBatch b) { }
    }
}
