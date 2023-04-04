using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ScaleTestHalfPixels.Sprites
{
	public class Sprite : IComponent, ICloneable
	{
		protected float layer { get; set; }
		protected Vector2 origin { get; set; }

		protected Vector2 position { get; set; }
		protected float rotation { get; set; }
		protected float scale { get; set; }

		protected Texture2D texture;

		public float Layer
		{
			get { return layer; }
			set
			{
				layer = value;
			}
		}

		public List<Sprite> Children { get; set; }
		public Sprite Parent { get; set; }

		public Color Colour { get; set; }
		public readonly Color[] TextureData;

		public bool IsRemoved { get; set; }

		public Vector2 Origin
		{
			get { return origin; }
			set
			{
				origin = value;
			}
		}

		public Vector2 Position
		{
			get { return position; }
			set
			{
				position = value;
			}
		}

		public float Rotation
		{
			get { return rotation; }
			set
			{
				rotation = value;
			}
		}

		public float Scale
		{
			get { return scale; }
			set
			{
				scale = value;
			}
		}

		public Matrix Transform =>
			Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
			Matrix.CreateRotationZ(Rotation) *
			Matrix.CreateTranslation(new Vector3(Position, 0));

		public Rectangle Rectangle
		{
			get
			{
				if (texture != null)
				{
					return new Rectangle((int)Position.X -1- (int)Origin.X * (int)Scale, (int)Position.Y -1- (int)Origin.Y * (int)Scale, texture.Width * (int)Scale, texture.Height * (int)Scale);
				}

				throw new Exception("Unknown Sprite");
			}
		}

		public Rectangle CollisionArea
		{
			get
			{
				return new Rectangle(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height);
			}
		}

		public Sprite(Texture2D tex)
		{
			texture = tex;
			Origin = new Vector2((float)texture.Width / 2, (float)texture.Height / 2);

			Children = new List<Sprite>();
			Scale = 4f;

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
				spriteBatch.Draw(texture, Position, null, Colour, Rotation, Origin, Scale, SpriteEffects.None, Layer);

		}

		// While this function seems very basic it means I can later on implement my own collision detection if I want to extremely easily within this area.
		public bool Intersects(Sprite other, Vector2 Offset)
		{
			if(this.TextureData == null) return false;
			if(other.TextureData == null) return false;

			var collisionArea = new Rectangle(
				(int)CollisionArea.X + (int)Offset.X,
				(int)CollisionArea.Y + (int)Offset.Y,
				CollisionArea.Width,
				CollisionArea.Height
			);

			if(collisionArea.Intersects(other.CollisionArea))
				return true;

			return false;
		}

		public object Clone()
		{
			var sprite = this.MemberwiseClone() as Sprite;
			return sprite ?? throw new Exception("ERROR :: Unable to clone class 'Sprite', attempted to return NULL.");
		}
	}
}