using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;

namespace Spellpath.Tiles
{
    public class LayerRenderer
    {
        public TiledMapRenderer Map { get; private set; }
        public TiledMapLayer Source { get; private set; }
        public List<TileRenderer> Tiles { get; private set; }

        public LayerRenderer(TiledMapTileLayer source, TiledMapRenderer map)
        {
            this.Map = map;
            this.Source = source;

            this.Tiles = new List<TileRenderer>();

            foreach (var tile in source.Tiles)
            {
                if (tile.GlobalIdentifier <= 0)
                    continue;

                var tileset = map.Source.GetTilesetByTileGlobalIdentifier(tile.GlobalIdentifier);

                if (tileset.Tiles.Count > 0)
                {
                    foreach (var tile2 in tileset.Tiles)
                    {
                        if (tile2.LocalTileIdentifier == tile.GlobalIdentifier - map.Source.GetTilesetFirstGlobalIdentifier(tileset))
                        {
                            if (tile2 is TiledMapTilesetAnimatedTile)
                            {
                                Tiles.Add(new AnimatedTileRenderer(tile, tile2, this, tileset));
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
                tile.Draw(b, ((tile.Tile.Y + 1.15f) * Map.Source.TileHeight) / 10000f, x, y);
        }
    }
}
