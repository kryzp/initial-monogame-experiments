using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.Tiles
{
    public class StaticTileRenderer : TileRenderer
    {
        public StaticTileRenderer(TiledMapTile tile, TiledMapTilesetTile source, LayerRenderer layer, TiledMapTileset tileset)
            : base(tile, source, layer, tileset)
        {
        }

        public override void Update(GameTime time)
        {
        }

        public override void Draw(SpriteBatch b, float layerDepth, int xoffset, int yoffset)
        {
            var dest = new Rectangle(
                (Tile.X * Layer.Map.Source.TileWidth) + xoffset,
                (Tile.Y * Layer.Map.Source.TileHeight) + yoffset,
                Layer.Map.Source.TileWidth,
                Layer.Map.Source.TileHeight
            );

            b.Draw(
                MapTileset.Texture,
                dest,
                Util.GetSourceRectForTilesheet(MapTileset.Texture, ID),
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                layerDepth
            );
        }
    }
}
