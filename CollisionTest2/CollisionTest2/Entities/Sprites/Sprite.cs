using System;
using System.Collections.Generic;
using System.Linq;
using ARPG.Util.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollisionTest2.Entities.Sprites
{
    public class Sprite : Entity, ICloneable
    {
        protected Texture2D texture;

        protected float layer { get; set; }
        protected Vector2 origin { get; set; }
        
        protected Vector2 position { get; set; }
        protected float rotation { get; set; }
        protected float scale { get; set; }

        /// <summary>
        /// Normally Sprites sort via their Origin.Y Point, however this offsets that
        /// meaning sprites can sort at different areas without actually affecting the
        /// actual drawing position of the sprite
        /// </summary>
        public float YSortOffset { get; set; }

        public float Layer
        {
            get => layer;
            set
            {
                layer = value;
            }
        }
        
        public Vector2 Origin
        {
            get => origin;
            set
            {
                origin = value;
            }
        }

        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
            }
        }

        public float Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
            }
        }

        public float Scale
        {
            get => scale;
            set
            {
                scale = value;
            }
        }
        
        public Color Colour { get; set; }
        public readonly Color[] TextureData;

        /// <summary>
        /// Just a rectangular area around the texture
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                if(texture != null)
                {
                    return new Rectangle(
                        ((int)Position.X - (int)Origin.X) * (int)Scale, // TODO: this might be a bit buggy, maybe only origin should be multiplied by scale(?)
                        ((int)Position.Y - (int)Origin.Y) * (int)Scale,
                        (int)texture.Width * (int)Scale,
                        (int)texture.Height * (int)scale
                    );
                }
                
                throw new Exception("Failed to find texture.");
            }
        }

        /// <summary>
        /// Collides with ISolid Entities
        /// </summary>
        public PolygonCollider CollisionArea
        {
            get
            {
                return new PolygonCollider()
                {
                    Position = this.Position,
                    Rotation = this.Rotation,
                    Parent = this,
                    Original = new List<Vector2>()
                    {
                        new Vector2(Rectangle.X, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height),
                        new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height)
                    },
                    Points = new List<Vector2>()
                    {
                        new Vector2(Rectangle.X, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height),
                        new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height)
                    }
                };
            }
        }

        /// <summary>
        /// Used as a 'trigger collider' for detection with things
        /// like projectiles without affecting the actual CollisionArea
        /// </summary>
        public PolygonCollider Hitbox
        {
            get
            {
                return new PolygonCollider()
                {
                    Position = this.Position,
                    Rotation = this.Rotation,
                    Parent = this,
                    Original = new List<Vector2>()
                    {
                        new Vector2(Rectangle.X, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height),
                        new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height)
                    },
                    Points = new List<Vector2>()
                    {
                        new Vector2(Rectangle.X, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y),
                        new Vector2(Rectangle.X + Rectangle.Width, Rectangle.Y + Rectangle.Height),
                        new Vector2(Rectangle.X, Rectangle.Y + Rectangle.Height)
                    }
                };
            }
        }

        public Sprite(Texture2D tex)
        {
            texture = tex;
            Origin = new Vector2(texture.Width / 2, texture.Height);

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
            {
                spriteBatch.Draw(texture, Position, null, Colour, Rotation, Origin, Scale, SpriteEffects.None, Layer);
            }
        }
        
        public bool Intersects(Sprite other)
        {
            if(this.TextureData == null) return false;
            if(other.TextureData == null) return false;

            if(other is ISolid)
                return (CollisionManager.ShapeOverlap_SAT_STATIC(CollisionArea, other.CollisionArea));

            return (CollisionManager.ShapeOverlap_SAT(CollisionArea, other.CollisionArea));
        }

        public object Clone()
        {
            var sprite = this.MemberwiseClone() as Sprite;

            return sprite ?? throw new Exception("Failed to return Clone.");
        }
    }
}