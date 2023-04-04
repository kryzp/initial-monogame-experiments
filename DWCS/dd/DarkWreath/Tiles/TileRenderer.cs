using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledCS;

namespace DarkWreath.Tiles
{
    public class TileRenderer
    {
        private int m_frame = 0;
        private float m_timer = 0f;
        
        public LayerRenderer Layer { get; private set; }
        public TiledMapTileset MapTileset { get; private set; }
        public TiledTileset Tileset { get; private set; }
        public TiledTile Source { get; private set; }

        public int FirstGID => MapTileset.firstgid;
        public int ID => GID - FirstGID;
        public int GID { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public TileRenderer(TiledTile source, LayerRenderer layer, TiledMapTileset mapTileset, TiledTileset tileset, int x, int y, int gid)
        {
            this.Source = source;
            this.Layer = layer;
            this.MapTileset = mapTileset;
            this.Tileset = tileset;
            this.X = x;
            this.Y = y;
            this.GID = gid;
        }

        public void Update(GameTime time)
        {
            m_timer += (float)time.ElapsedGameTime.TotalMilliseconds;

            if (m_timer >= Source.animation[m_frame].duration)
            {
                m_timer = 0f;
                m_frame++;

                if (m_frame >= Source.animation.Length)
                    m_frame = 0;
            }
        }

        public void Draw(SpriteBatch b, float layerDepth, int xoffset, int yoffset)
        {
            var dest = new Rectangle(
                (X * Layer.Map.Source.TileWidth) + xoffset,
                (Y * Layer.Map.Source.TileHeight) + yoffset,
                Layer.Map.Source.TileWidth,
                Layer.Map.Source.TileHeight
            );

            var source = new Rectangle(
                (FirstGID + Source.animation[m_frame].tileid - 1) * Layer.Map.Source.TileWidth % (Tileset.Columns * Tileset.TileWidth),
                (FirstGID + Source.animation[m_frame].tileid) * Layer.Map.Source.TileWidth / (Tileset.Columns * Tileset.TileWidth) * Layer.Map.Source.TileHeight,
                Layer.Map.Source.TileWidth,
                Layer.Map.Source.TileHeight
            );

            b.Draw(
                Layer.Map.GetTilesetTexture(MapTileset.source),
                dest,
                source,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                layerDepth
            );
        }
    }
}
