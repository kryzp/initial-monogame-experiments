using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Managers;
using Asteroids.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Sprites
{
    public class Sprite : IComponent, ICloneable
    {
        protected Dictionary<string, Animation> animations;
        protected AnimationManager animationManager;
        
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
                if(animationManager != null)
                    animationManager.Layer = Layer;
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
                if(animationManager != null)
                    animationManager.Origin = Origin;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                if(animationManager != null)
                    animationManager.Position = Position;
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set
            {
                rotation = value;
                if(animationManager != null)
                    animationManager.Rotation = Rotation;
            }
        }

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                if(animationManager != null)
                    animationManager.Scale = Scale;
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
                if(texture != null)
                {
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, texture.Width, texture.Height);
                }

                if(animationManager != null)
                {
                    var anim = animations.FirstOrDefault().Value;
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, anim.FrameWidth, anim.FrameHeight);
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
            Scale = 1f;

            Colour = Color.White;
            TextureData = new Color[texture.Width * texture.Height];
            texture.GetData(TextureData);
        }

        public Sprite(Dictionary<string, Animation> anims)
        {
            Children = new List<Sprite>();
            Scale = 1f;
            
            animations = anims;
            Animation animation = animations.FirstOrDefault().Value;
            animationManager = new AnimationManager(animation);

            texture = null;
            Colour = Color.White;
            TextureData = new Color[animation.Texture.Width * animation.Texture.Height];
            animation.Texture.GetData(TextureData);
            
            Origin = new Vector2((float)animation.FrameWidth / 2, (float)animation.FrameHeight / 2);
        }
        
        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, Position, null, Colour, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            else
                animationManager?.Draw(spriteBatch);
        }

        // While this function seems very basic it means I can later on implement my own collision detection if I want to extremely easily within this area.
        public bool Intersects(Sprite other)
        {
            if(this.TextureData == null) return false;
            if(other.TextureData == null) return false;

            if(CollisionArea.Intersects(other.CollisionArea))
                return true;

            return false;
        }

        public object Clone()
        {
            var sprite = MemberwiseClone() as Sprite;

            if(animations != null && sprite != null)
            {
                    sprite.animations = animations.ToDictionary(c => c.Key, v => v.Value.Clone() as Animation);
                    sprite.animationManager = sprite.animationManager.Clone() as AnimationManager;
            }

            return sprite ?? throw new Exception("ERROR :: Unable to clone class 'Sprite', attempted to return NULL.");
        }
    }
}