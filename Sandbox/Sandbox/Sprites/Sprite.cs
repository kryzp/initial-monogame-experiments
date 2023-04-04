using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sandbox.Managers;
using Sandbox.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox.Sprites
{
	public class Sprite : Component, ICloneable
	{
		protected Dictionary<string, Animation> animations;
        protected AnimationManager animationManager;
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
            set
            {
                layer = value;

                if(animationManager != null)
                    animationManager.Layer = layer;
            }
        }
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;

                if(animationManager != null)
                    animationManager.Position = value;
            }
        }
        public Vector2 Origin
        {
            get { return origin; }
            set
            {
                origin = value;

                if(animationManager != null)
                    animationManager.Origin = value;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                if(texture != null) {
                    return new Rectangle((int)position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, texture.Width, texture.Height);
                }
			    if(animationManager != null) {
                    var anim = animations.FirstOrDefault().Value;

                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, anim.FrameWidth, anim.FrameHeight);
                }

                throw new Exception("ERROR :: UNKNOWN SPRITE");
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;

                if(animationManager != null)
                    animationManager.Rotation = value;
            }
        }

        public readonly Color[] TextureData;

        public Matrix Transform
        {
            get
            {
                return Matrix.CreateTranslation(new Vector3(-Origin, 0))
                    * Matrix.CreateRotationZ(rotation)
                    * Matrix.CreateTranslation(new Vector3(Position, 0));
            }
        }

        public Sprite Parent;

        public Rectangle CollisionArea
        {
            // Rough "area of effect" where a sprite COULD be colliding (more efficient), moves onto per-pixel collision detection
            get
            {
                return new Rectangle(Rectangle.X, Rectangle.Y, Math.Max(Rectangle.Width, Rectangle.Height), Math.Max(Rectangle.Width, Rectangle.Height));
            }
        }

		public Sprite(Texture2D texture)
		{
			this.texture = texture;
            Children = new List<Sprite>();
            Origin = new Vector2(this.texture.Width / 2, this.texture.Height / 2);
            Colour = Color.White;
            TextureData = new Color[this.texture.Width * this.texture.Height];
            this.texture.GetData(TextureData);
		}

        public Sprite(Dictionary<string, Animation> animations)
        {
            texture = null;
            TextureData = null;
            Children = new List<Sprite>();
            Colour = Color.White;
            this.animations = animations;
            var anim = this.animations.FirstOrDefault().Value;
            animationManager = new AnimationManager(anim);
            Origin = new Vector2(anim.FrameWidth / 2, anim.FrameHeight / 2);
        }

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            if(texture != null)
                spriteBatch.Draw(texture, Position, null, Colour, Rotation, Origin, 1f, SpriteEffects.None, Layer);
            else if(animationManager != null)
                animationManager.Draw(spriteBatch);
		}

        public bool Intersects(Sprite sprite)
        {
            if(this.TextureData == null) return false;
            if(sprite.TextureData == null) return false;

            var transA2B = this.Transform * Matrix.Invert(sprite.Transform);

            var stepX = Vector2.TransformNormal(Vector2.UnitX, transA2B);
            var stepY = Vector2.TransformNormal(Vector2.UnitY, transA2B);

            var yPosInB = Vector2.Transform(Vector2.Zero, transA2B);

            for(int yA = 0; yA < this.Rectangle.Height; yA++)
            {
				var posInB = yPosInB;

				for(int xA = 0; xA < this.Rectangle.Width; xA++)
				{
                    var xB = (int)Math.Round(posInB.X);
                    var yB = (int)Math.Round(posInB.Y);

                    if(0 <= xB && xB < sprite.Rectangle.Width &&
                       0 <= yB && yB < sprite.Rectangle.Height)
                    {
                        var colA = this.TextureData[xA + yA * this.Rectangle.Width];
                        var colB = this.TextureData[xB + yB * this.Rectangle.Width];

                        if(colA.A != 0 && colB.A != 0) return true;
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

			if(animations != null)
			{
				sprite.animations = animations.ToDictionary(c => c.Key, v => v.Value.Clone() as Animation);
				sprite.animationManager = sprite.animationManager.Clone() as AnimationManager;
			}

			return sprite;
		}
	}
}