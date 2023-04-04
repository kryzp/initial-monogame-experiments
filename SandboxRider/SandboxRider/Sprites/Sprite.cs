using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandboxRider;
using SandboxRider.Models;

namespace SandboxRider.Sprites
{
    public class Sprite : Component, ICloneable
    {
        protected float layer { get; set; }
        protected Vector2 origin { get; set; }
        protected Vector2 position { get; set; }
        protected float rotation { get; set; }
        protected float scale { get; set; }
        protected Texture2D texture;

        public List<Sprite> Children { get; set; }
        public Color Colour { get; set; }
        public bool IsRemoved { get; set; }

        public float Layer
        {
          get { return layer; }
          set { layer = value; }
        }

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

        public float Scale
        {
          get { return scale; }
          set { scale = value; }
        }

        public Rectangle Rectangle
        {
          get
          {
            if(texture != null)
            {
              return new Rectangle((int) Position.X - (int) Origin.X, (int) Position.Y - (int) Origin.Y, texture.Width,
                texture.Height);
            }

            throw new Exception("Unknown Sprite");
          }
        }

        public float Rotation
        {
          get { return rotation; }
          set { rotation = value; }
        }

        public readonly Color[] TextureData;

        public Matrix Transform
        {
          get
          {
            return Matrix.CreateTranslation(new Vector3(-Origin, 0)) *
                   Matrix.CreateRotationZ(rotation) *
                   Matrix.CreateTranslation(new Vector3(Position, 0));
          }
        }

        public Sprite Parent;

        public Rectangle CollisionArea
        {
          get
          {
            return new Rectangle(Rectangle.X, Rectangle.Y, MathHelper.Max(Rectangle.Width, Rectangle.Height),
              MathHelper.Max(Rectangle.Width, Rectangle.Height));
          }
        }

        public Sprite(Texture2D texture)
        {
          this.texture = texture;
          Children = new List<Sprite>();
          Origin = new Vector2(texture.Width / 2, texture.Height / 2);
          Colour = Color.White;
          TextureData = new Color[texture.Width * texture.Height];
          texture.GetData(TextureData);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
          if(texture != null)
            spriteBatch.Draw(texture, Position, null, Colour, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }

        public bool Intersects(Sprite sprite)
        {
          if(this.TextureData == null)
            return false;

          if(sprite.TextureData == null)
            return false;

          var transformAToB = this.Transform * Matrix.Invert(sprite.Transform);

          var stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
          var stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

          var yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

          for(int yA = 0; yA < this.Rectangle.Height; yA++)
          {
            var posInB = yPosInB;

            for(int xA = 0; xA < this.Rectangle.Width; xA++)
            {
              var xB = (int) Math.Round(posInB.X);
              var yB = (int) Math.Round(posInB.Y);

              if(0 <= xB && xB < sprite.Rectangle.Width &&
                 0 <= yB && yB < sprite.Rectangle.Height)
              {
                var colourA = this.TextureData[xA + yA * this.Rectangle.Width];
                var colourB = sprite.TextureData[xB + yB * sprite.Rectangle.Width];

                if(colourA.A != 0 && colourB.A != 0) return true;
              }

              posInB += stepX;
            }

            yPosInB += stepY;
          }

          return false;
        }

        public object Clone()
        {
          var sprite = this.MemberwiseClone() as Sprite;
          return sprite;
        }
    }
}