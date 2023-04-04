using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownMovement.Sprites
{
	public class Wall : Sprite, ICollidable
	{
		public Wall(Texture2D tex)
			: base(tex)
		{
			Origin = new Vector2(0, 0);
		}

		public void OnCollide(Sprite other)
		{
		}
	}
}