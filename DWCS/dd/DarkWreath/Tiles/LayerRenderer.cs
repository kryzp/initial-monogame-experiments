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

            foreach (var tile in source.Tiles)
            {
                if (tile.GlobalIdentifier <= 0)
                    continue;

                var tileset = map.Source.GetTilesetFirstGlobalIdentifier(tile.GlobalIdentifier);

                if (tileset.Tiles.Count > 0)
                {
                    foreach (var tile2 in tileset.Tiles)
                    {
                        if (tile2.LocalTileIdentifier == tile.GlobalIdentifier - map.Source.GetTilesetFirstGlobalIdentifier(tileset))
                        {
                            if (tile2 is TiledMapTilesetAnimatedTile)
                            {
                                Tiles.Add(new TileRenderer(tile, tile2, this, tileset));
                                continue;
                            }
                        }
                    }
                }

                Tiles.Add(new StaticTileRenderer(tile, null, this, tileset));
            }
        }

        public void Update(GameTime time)
        {
            foreach (var tile in Tiles)
                tile.Update(time);
        }
        
        public void Draw(SpriteBatch b, int x, int y)
        {
            foreach (var tile in Tiles)
                tile.Draw(b, ((tile.Y + 1.15f) * Map.Source.TileHeight) / 10000f, x, y);
        }
    }
}
