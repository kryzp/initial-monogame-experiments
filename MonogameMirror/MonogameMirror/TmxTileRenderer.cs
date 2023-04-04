using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using TiledSharp;

namespace MonogameMirror
{
    public class TmxTileRenderer
    {
        private TmxLayerRenderer layer;
        private TmxTileset tileset;

        private Texture2D texture;
        private Rectangle sourceRectangle;

        private int firstgid;
        private int gid;
        private int tileid;

        public TmxTileRenderer(TmxLayerRenderer layer, int gid)
        {
            this.layer = layer;
            this.gid = gid;
        }

        public void load(ContentManager content)
        {
            tileset = layer.map.getTilesetFromGid(gid);

            firstgid = tileset.FirstGid;
            tileid = gid - firstgid;

            sourceRectangle = new Rectangle(
                (int)(tileid * tileset.TileWidth % tileset.Image.Width),
                (int)(tileid * tileset.TileWidth / tileset.Image.Width * tileset.TileHeight),
                tileset.TileWidth,
                tileset.TileHeight
            );

            string assetName = "sheets\\" + Path.GetFileNameWithoutExtension(tileset.Image.Source);
            texture = content.Load<Texture2D>(assetName);
        }

        public void unload()
        {
            texture.Dispose();
        }

        public void draw(SpriteBatch b, int x, int y)
        {
            b.Draw(texture, new Vector2(x * tileset.TileWidth * 4, y * tileset.TileHeight * 4), sourceRectangle, Color.White, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
        }
    }
}
