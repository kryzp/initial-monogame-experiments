using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Spellpath.Menus
{
	public class InventoryMenu : ClickableMenu
	{
		List<ClickableTextureComponent> m_clickableTextureComponents = new List<ClickableTextureComponent>();

		public InventoryMenu()
			: base()
		{
		}

		public override void Update(GameTime time)
		{
			base.Update(time);
		}

		public override void Draw(GameTime time, SpriteBatch b)
		{
			base.Draw(time, b);
		}
	}
}
