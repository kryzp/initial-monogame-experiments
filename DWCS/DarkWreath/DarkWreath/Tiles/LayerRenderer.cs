using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TiledCS;

namespace DarkWreath.Tiles
{
    public class LayerRenderer
    {
        public TiledMapRenderer Map { get; private set; }
        public TiledLayer Source { get; private set; }
        public List<TileRenderer> Tiles { get; private set; }

        public LayerRenderer(TiledLayer source, TiledMapRenderer map)
        {
            this.Map = map;
            this.Source = source;

            this.Tiles = new List<TileRenderer>();

            for (int y = 0; y < source.height; y++)
            {
                for (int x = 0; x < source.width; x++)
                {
                    var index = x + (y * source.width);
                    var gid = source.data[index];
                    var tileX = x * map.Source.TileWidth;
                    var tileY = y * map.Source.TileHeight;

                    if (gid <= 0)
                        continue;

                    var mapTileset = map.Source.GetTiledMapTileset(gid);
                    var tileset = Map.Tilesets[mapTileset.firstgid];

                    var tile = map.Source.GetTiledTile(mapTileset, tileset, gid);
                    
                    Tiles.Add(new TileRenderer(tileX, tileY, this, mapTileset, tileset, tile, gid));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var tile in Tiles)
                tile.Update(gameTime);
        }
        
        public void Draw(SpriteBatch b, int x, int y)
        {
            foreach (var tile in Tiles)
                tile.Draw(b, (tile.Y + 1.15f * Map.Source.TileHeight) / 10000f, x, y);
        }
    }
}
