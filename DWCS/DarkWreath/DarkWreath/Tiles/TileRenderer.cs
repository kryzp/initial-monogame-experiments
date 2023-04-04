using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace DarkWreath.Tiles
{
    public class TileRenderer
    {
        private int frame = 0;
        private float timer = 0f;

        public LayerRenderer Layer { get; private set; }
        public TiledMapTileset MapTileset { get; private set; }
        public TiledTileset Tileset { get; private set; }
        public TiledTile Properties { get; private set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int GID { get; set; }

        public TileRenderer(int x, int y, LayerRenderer layer, TiledMapTileset mapTileset, TiledTileset tileset, TiledTile properties, int gid)
        {
            this.Properties = properties;
            this.Layer = layer;
            this.MapTileset = mapTileset;
            this.Tileset = tileset;
            this.X = x;
            this.Y = y;
            this.GID = gid;
        }

        public void Update(GameTime gameTime)
        {
            if (Properties == null)
                return;

            if (Properties.animation.Length <= 0)
                return;
            
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timer >= Properties.animation[frame].duration)
            {
                timer = 0f;
                frame++;

                if (frame >= Properties.animation.Length)
                    frame = 0;
            }
        }

        public void Draw(SpriteBatch b, float layerDepth, int xoffset, int yoffset)
        {
            var dest = new Rectangle(
                X + xoffset,
                Y + yoffset,
                Layer.Map.Source.TileWidth,
                Layer.Map.Source.TileHeight
            );

            int gid = GID;

            if (Properties != null)
            {
                if (Properties.animation.Length > 0)
                    gid = MapTileset.firstgid + Properties.animation[frame].tileid;
            }
            
            var tileSource = Layer.Map.Source.GetSourceRect(
                MapTileset,
                Tileset,
                gid
            );

            var tex = TiledMapRenderer.GetTilesetTexture("maps/" + Path.GetFileNameWithoutExtension(Tileset.Image.source));

            b.Draw(
                tex,
                dest,
                new Rectangle(tileSource.x, tileSource.y, tileSource.width, tileSource.height),
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                layerDepth
            );
        }
    }
}
