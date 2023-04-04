using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using System.IO;

namespace Spellpath.Tiles
{
    public abstract class TileRenderer
    {
        public LayerRenderer Layer { get; private set; }
        public TiledMapTileset Tileset { get; private set; }
        public TiledMapTilesetTile Source { get; private set; }
        public TiledMapTile Tile { get; private set; }

        public int FirstGID => Layer.Map.Source.GetTilesetFirstGlobalIdentifier(Tileset);
        public int ID => GID - FirstGID;
        public int GID { get; set; }

        public TileRenderer(TiledMapTile tile, TiledMapTilesetTile source, LayerRenderer layer, TiledMapTileset tileset)
        {
            this.Source = source;
            this.Layer = layer;
            this.Tileset = tileset;
            this.Tile = tile;
            this.GID = tile.GlobalIdentifier;
        }

        public abstract void Update(GameTime time);

        public abstract void Draw(SpriteBatch b, float layerDepth, int xoffset, int yoffset);
    }
}
