using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECS1.ComponentArchitecture.Components.Transform;
using Microsoft.Xna.Framework;

namespace ECS1.ComponentArchitecture.Components.Visual
{
	public class SpriteComponent : EntityComponent
	{
		private Texture2D texture;

		private TransformComponent transform;

		public float Layer { get; set; }
		public Vector2 Origin { get; set; }

		public Color Colour { get; set; }

		public Rectangle Rectangle
		{
			get
			{
				if(texture != null)
				{
					return new Rectangle(
						(int)(transform.Position.X - (Origin.X * transform.Scale.X)),
						(int)(transform.Position.Y - (Origin.Y * transform.Scale.Y)),
						(int)(texture.Width * transform.Scale.X),
						(int)(texture.Height * transform.Scale.Y)
					);
				}

				throw new Exception("Null Texture");
			}
		}

		public SpriteComponent(Texture2D texture, TransformComponent transform)
		{
			this.texture = texture;
			this.transform = transform;

			Layer = 0.5f;
			Origin = Vector2.Zero;
			Colour = Color.White;
		}

		public override void Update(float deltaTime)
		{
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
			if(texture != null)
			{
				spriteBatch.Draw(
					texture,
					new Vector2(
						transform.Position.X,
						transform.Position.Y
					),
					null,
					Colour,
					transform.Rotation.Radians,
					Origin,
					new Vector2(
						transform.Scale.X,
						transform.Scale.Y
					),
					SpriteEffects.None,
					Layer
				);
			}
		}
	}
}
