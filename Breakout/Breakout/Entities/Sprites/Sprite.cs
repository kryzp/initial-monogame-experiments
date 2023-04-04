using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities.Sprites
{
    public class Sprite : Entity, ICloneable
    {
        protected Texture2D texture;
        
        protected float layer { get; set; }
        protected Vector2 origin { get; set; }
        
        protected Vector2 position { get; set; }
        protected float rotation { get; set; }
        protected float scale { get; set; }

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

        public Rectangle Rectangle
        {
            get
            {
                if(texture != null)
                {
                    int xOffset = 0;
                    int yOffset = 0;

                    if(texture.Width % 2 == 1) xOffset = 1;
                    if(texture.Height % 2 == 1) yOffset = 1;
                    
                    return new Rectangle(
                        ((int)Position.X - (int)Origin.X * (int)Scale) + xOffset,
                        ((int)Position.Y - (int)Origin.Y - (int)Scale) + yOffset,
                        texture.Width * (int)Scale,
                        texture.Height * (int)Scale
                    );
                }
                
                throw new Exception("Unknown Texture.");
            }
        }

        public Rectangle CollisionArea => this.Rectangle;

        public Color Colour { get; set; }
        public Color[] TextureData;

        public Sprite(Texture2D tex)
        {
            texture = tex;
            Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);

            Scale = 1f;

            Colour = Color.White;
            TextureData = new Color[texture.Width * texture.Height];
            texture.GetData(TextureData);
        }
        
        public override void Update(float deltaTime)
        {
        }

        public override void Draw(float deltaTime, SpriteBatch spriteBatch)
        {
            if(texture != null)
                spriteBatch.Draw(texture, Position, null, Colour, Rotation, Origin, Scale, SpriteEffects.None, Layer);
        }

        public bool Intersects(Sprite other, Vector2 origin)
        {
            if(this.TextureData == null) return false;
            if(other.TextureData == null) return false;

            if(this.CollisionArea.Intersects(other.CollisionArea))
                return true;

            return false;
        }

        public object Clone()
        {
            var sprite = this.MemberwiseClone() as Sprite;
            return sprite ?? throw new Exception("Failed to return Clone.");
        }
    }
}