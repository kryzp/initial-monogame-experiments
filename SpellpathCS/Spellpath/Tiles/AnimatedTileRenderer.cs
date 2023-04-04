using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace Spellpath.Tiles
{
    public class AnimatedTileRenderer : TileRenderer
    {
        private TiledMapTilesetAnimatedTile m_sourceCasted => (TiledMapTilesetAnimatedTile)Source;

        private int m_frame = 0;
        private float m_timer = 0f;

        public AnimatedTileRenderer(TiledMapTile tile, TiledMapTilesetTile source, LayerRenderer layer, TiledMapTileset tileset)
            : base(tile, source, layer, tileset)
        {
        }

        public override void Update(GameTime time)
        {
            m_timer += (float)time.ElapsedGameTime.TotalMilliseconds;

            if (m_timer >= m_sourceCasted.AnimationFrames[m_frame].Duration.TotalMilliseconds)
            {
                m_timer = 0f;
                m_frame++;

                if (m_frame >= m_sourceCasted.AnimationFrames.Count)
                    m_frame = 0;
            }
        }

        public override void Draw(SpriteBatch b, float layerDepth, int xoffset, int yoffset)
        {
            var dest = new Rectangle(
                (Tile.X * Layer.Map.Source.TileWidth) + xoffset,
                (Tile.Y * Layer.Map.Source.TileHeight) + yoffset,
                Layer.Map.Source.TileWidth,
                Layer.Map.Source.TileHeight
            );

            var source = new Rectangle(
                (FirstGID + m_sourceCasted.AnimationFrames[m_frame].LocalTileIdentifier - 1) * Layer.Map.Source.TileWidth % Tileset.Texture.Width,
                (FirstGID + m_sourceCasted.AnimationFrames[m_frame].LocalTileIdentifier) * Layer.Map.Source.TileWidth / Tileset.Texture.Width * Layer.Map.Source.TileHeight,
                Layer.Map.Source.TileWidth,
                Layer.Map.Source.TileHeight
            );

            b.Draw(
                Tileset.Texture,
                dest,
                source,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                layerDepth
            );
        }
    }
}
