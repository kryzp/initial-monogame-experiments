using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath
{
	public class TextureRegion
	{
		public Texture2D Texture;
		public Rectangle Region;

		public TextureRegion()
		{
			this.Texture = null;
			this.Region = Rectangle.Empty;
		}

		public TextureRegion(Texture2D texture)
		{
			this.Texture = texture;
			this.Region = new Rectangle(0, 0, texture.Width, texture.Height);
		}

		public TextureRegion(Texture2D texture, Rectangle region)
		{
			this.Texture = texture;
			this.Region = region;
		}
	}
}
