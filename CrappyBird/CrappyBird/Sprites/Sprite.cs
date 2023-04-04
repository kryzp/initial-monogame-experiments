using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace CrappyBird.Sprites
{
	public class Sprite : IComponent, ICloneable
	{
		protected Texture2D texture;

		protected float layer { get; set; }
		protected Vector2 origin { get; set; }

		protected Vector2 position { get; set; }
		protected float rotation { get; set; }
		protected float scale { get; set; }

		public bool IsRemoved { get; set; }

		public float Layer
		{
			get => layer;
			set => layer = value;
		}

		public Vector2 Origin
		{
			get => origin;
			set => origin = value;
		}

		public Vector2 Position
		{
			get => position;
			set => position = value;
		}

		public float Rotation
		{
			get => rotation;
			set => rotation = value;
		}

		public float Scale
		{
			get => scale;
			set => scale = value;
		}

		public Matrix Transform =>
			Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
			Matrix.CreateRotationZ(Rotation) *
			Matrix.CreateTranslation(new Vector3(Position, 0));

		public Rectangle Rectangle
		{
			get
			{
				int xOffset = 0;
				int yOffset = 0;

				if(texture != null)
				{
					if(texture.Width % 2 == 1)
						xOffset = 1;
					if(texture.Height % 2 == 1)
						yOffset = 1;

					return new Rectangle(
						((int)Position.X - (int)Origin.X * (int)Scale) + xOffset,
						((int)Position.Y - (int)Origin.Y * (int)Scale) + yOffset,
						texture.Width * (int)Scale,
						texture.Height * (int)Scale
					);
				}

				throw new Exception("Unknown Texture. ");
			}
		}

		public Rectangle CollisionArea => this.Rectangle;


		public List<Sprite> Children;
		public Sprite Parent;

		public Color Colour { get; set; }
		public Color[] TextureData;

		public Sprite(Texture2D tex)
		{
			texture = tex;
			Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

			Children = new List<Sprite>();
			Scale = 4f * Game1.WorldScale;

			Colour = Color.White;
			TextureData = new Color[texture.Width * texture.Height];
			texture.GetData(TextureData);
		}

		public virtual void Update(GameTime gameTime)
		{
		}

		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if(texture != null)
			{
				spriteBatch.Draw(
					texture,
					Position,
					null,
					Colour,
					Rotation,
					Origin,
					Scale,
					SpriteEffects.None,
					Layer
				);
			}
		}

		public bool Intersects(Sprite other, Vector2 offset)
		{
			if(this.TextureData == null)
				return false;

			if(other.TextureData == null)
				return false;

			if(this.CollisionArea.Intersects(other.CollisionArea))
				return true;

			return false;
		}

		public object Clone()
		{
			var sprite = this.MemberwiseClone() as Sprite;
			return sprite ?? throw new Exception("Unable to return Clone. ");
		}
	}
}
