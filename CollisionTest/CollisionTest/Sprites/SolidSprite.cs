using Microsoft.Xna.Framework.Graphics;

namespace CollisionTest.Sprites
{
	public class SolidSprite : Sprite, ICollidable
	{
		public SolidSprite(Texture2D tex)
			: base(tex)
		{
		}

		public void OnCollide(Sprite other)
		{
		}
	}
}