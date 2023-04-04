using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweptAABB
{
	public class Obstacle : ICollidable
	{
		public Box Collider;

		public Vector2 Position
		{
			get => Collider.Position;
			set => Collider.Position = value;
		}

		public Obstacle(Texture2D tex)
		{
			Collider = new Box(tex);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(Collider.Texture, Collider.Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.5f);
		}

		public void OnCollide()
		{
		}
	}
}
