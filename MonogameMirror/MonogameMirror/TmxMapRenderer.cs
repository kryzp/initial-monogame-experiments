using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using TiledSharp;

namespace MonogameMirror
{
    public class TmxMapRenderer
    {
        private Dictionary<int, Texture2D> tilesets;

        public TmxMap source { get; private set; }
        public TmxLayerRenderer[] layers { get; set; }

        public TmxMapRenderer(TmxMap source)
        {
            this.source = source;
        }

        public void load(ContentManager content)
        {
            tilesets = new Dictionary<int, Texture2D>();

            for (int index = 0; index < source.Tilesets.Count; index++)
            {
                TmxTileset mapTileset = source.Tilesets[index];

                string src = mapTileset.Image.Source;
                string assetName = Path.GetRelativePath("Content", src);
                assetName = Path.ChangeExtension(assetName, null);
                var image = content.Load<Texture2D>(assetName);

                tilesets.Add(mapTileset.FirstGid, image);
            }

            layers = new TmxLayerRenderer[source.Layers.Count];

            for (int index = 0; index < source.Layers.Count; index++)
            {
                TmxLayerRenderer layer = new TmxLayerRenderer(this, source.Layers[index]);
                layer.load(content);
                layers[index] = layer;
            }
        }

        public void unload()
        {
            for (int index = 0; index < layers.Length; ++index)
                layers[index].unload();

            layers = null;
            tilesets.Clear();
        }

        public void draw(SpriteBatch b)
        {
            for (int index = 0; index < layers.Length; ++index)
                layers[index].draw(b);
        }

        public TmxTileset getTilesetFromGid(int gid)
        {
            for (var i = 0; i < source.Tilesets.Count; i++)
            {
                if (i < tilesets.Count-1)
                {
                    int gid1 = source.Tilesets[i].FirstGid;
                    int gid2 = source.Tilesets[i+1].FirstGid;

                    if (gid >= gid1 && gid < gid2)
                        return source.Tilesets[i];
                }
                else
                    return source.Tilesets[i];
            }

            return default;
        }

        public TmxTileset getTilesetFromGid(TmxTileset tileset)
        {
            return getTilesetFromGid(tileset.FirstGid);
        }
    }
}
