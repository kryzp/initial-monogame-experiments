using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TiledSharp;

namespace MonogameMirror
{
    public class TmxLayerRenderer
    {
        private TmxTileRenderer[] tileRenderers;

        public TmxMapRenderer map { get; private set; }
        public TmxLayer source { get; private set; }

        public TmxLayerRenderer(TmxMapRenderer map, TmxLayer source)
        {
            this.map = map;
            this.source = source;
        }

        public void load(ContentManager content)
        {
            tileRenderers = new TmxTileRenderer[source.Tiles.Count];

            foreach (var it in source.Tiles)
            {
                TmxTileRenderer tile = new TmxTileRenderer(this, it.Gid);
                tile.load(content);
                tileRenderers[it.Y*map.source.Width + it.X] = tile;
            }
        }

        public void unload()
        {
            foreach (var tile in tileRenderers)
                tile?.unload();
        }

        public void draw(SpriteBatch b)
        {
            for (int x = 0; x < map.source.Width; ++x)
            {
                for (int y = 0; y < map.source.Height; ++y)
                {
                    TmxTileRenderer tile = tileRenderers[y*map.source.Width + x];
                    tile?.draw(b, x, y);
                }
            }
        }
    }
}
