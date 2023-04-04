using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownMovement.Sprites
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
			set { layer = value; }
		}
		
		public List<Sprite> Children { get; set; }
		public Sprite Parent { get; set; }
		
		public Color Colour { get; set; }
		public readonly Color[] TextureData;
		
		public bool IsRemoved { get; set; }

		public Vector2 Origin
		{
			get { return origin; }
			set { origin = value; }
		}
		
		public Vector2 Position
		{
			get { return position; }
			set { position = value; }
		}
		
		public float Rotation
		{
			get { return rotation; }
			set { rotation = value; }
		}
		
		public float Scale
		{
			get { return scale; }
			set { scale = value; }
		}

		public Matrix Matrix =>
			Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
			Matrix.CreateRotationZ(Rotation) *
			Matrix.CreateTranslation(new Vector3(Position, 0));

		public Rectangle Rectangle
		{
			get
			{
				if(texture != null)
				{
					int xOffset = 0;
					int yOffset = 0;
					
					if(texture.Width % 2 == 1) // If there is an odd number of pixels in the width
						xOffset = 1;
					if(texture.Height % 2 == 1) // If there is an odd number of pixels in the height
						yOffset = 1;
					
					return new Rectangle(
						((int)Position.X - (int)Origin.X * (int)Scale) + xOffset,
						((int)Position.Y - (int)Origin.Y * (int)Scale) + yOffset,
						texture.Width * (int)Scale,
						texture.Height * (int)Scale
					);
				}
				
				throw new Exception("Unknown Texture");
			}
		}

		public Rectangle CollisionArea => this.Rectangle;

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

		public bool Intersects(Sprite other, Vector2 offset)
		{
			if(this.TextureData == null) return false;
			if(other.TextureData == null) return false;
			
			var collisionArea = new Rectangle(
				(int)CollisionArea.X + (int)offset.X,
				(int)CollisionArea.Y + (int)offset.Y,
				CollisionArea.Width,
				CollisionArea.Height
			);

			if(collisionArea.Intersects(other.CollisionArea))
				return true;

			return false;
		}

		public object Clone()
		{
			var spriteClone = this.MemberwiseClone() as Sprite;
			return spriteClone ?? throw new Exception("Unable to clone class 'Sprite'");
		}
	}
}