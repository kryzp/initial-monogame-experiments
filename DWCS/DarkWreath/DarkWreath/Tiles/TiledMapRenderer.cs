using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TiledCS;

namespace DarkWreath.Tiles
{
    public class TiledMapRenderer
    {
        private static Dictionary<string, Texture2D> tilesetTextures = new Dictionary<string, Texture2D>();
        public Dictionary<int, TiledTileset> Tilesets { get; set; } = new Dictionary<int, TiledTileset>();

        public TiledMap Source { get; private set; }
        public Dictionary<string, LayerRenderer> TileLayers { get; private set; } = new Dictionary<string, LayerRenderer>();

        public TiledMapRenderer()
        {
        }

        public TiledMapRenderer(TiledMap source)
        {
            Load(source);
        }

        public void Update(GameTime gameTime)
        {
            foreach (var layer in TileLayers.Values)
                layer.Update(gameTime);
        }

        public void Draw(SpriteBatch b, int x = 0, int y = 0)
        {
            foreach (var layer in TileLayers.Values)
                layer.Draw(b, x, y);
        }

        public void DrawLayer(string name, SpriteBatch b, int x = 0, int y = 0)
        {
            TileLayers[name].Draw(b, x, y);
        }

        public void Load(TiledMap source)
        {
            Source = source;
            Reload();
        }

        private void Reload()
        {
            Tilesets = Source.GetTiledTilesets(Game1.MapContent.RootDirectory + "/maps/");

            TileLayers.Clear();
            
            foreach (var x in Source.Layers.Where(x => x.type == TiledLayerType.TileLayer))
                TileLayers.Add(x.name, new LayerRenderer(x, this));
        }

        public static Texture2D GetTilesetTexture(string source)
        {
            if (tilesetTextures.ContainsKey(source))
                return tilesetTextures[source];

            var tex = Game1.MapContent.Load<Texture2D>(source);
            tilesetTextures.Add(source, tex);
            return tex;
        }
    }
}
