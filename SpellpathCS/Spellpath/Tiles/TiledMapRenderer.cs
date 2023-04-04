using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System.Collections.Generic;

namespace Spellpath.Tiles
{
    public class TiledMapRenderer
    {
        public TiledMap Source { get; private set; }
        public Dictionary<string, LayerRenderer> Layers { get; private set; }

        public TiledMapRenderer()
        {
        }

        public TiledMapRenderer(TiledMap source)
        {
            Load(source);
        }

        public void Update(GameTime time)
        {
            foreach (var layer in Layers.Values)
                layer.Update(time);
        }

        public void Draw(SpriteBatch b, int x = 0, int y = 0)
        {
            foreach (var layer in Layers.Values)
                layer.Draw(b, x, y);
        }

        public void DrawLayer(string name, GameTime time, SpriteBatch b, int x = 0, int y = 0)
        {
            Layers[name].Draw(b, x, y);
        }

        public void Load(TiledMap source)
        {
            Source = source;
            Reload();
        }

        private void Reload()
        {
            if (Layers == null)
                Layers = new Dictionary<string, LayerRenderer>();
            
            Layers.Clear();

            foreach (var layer in Source.TileLayers)
                Layers.Add(layer.Name, new LayerRenderer(layer, this));
        }
    }
}
