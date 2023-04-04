using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TiledSharp;

namespace MonogameMirror
{
    public class Cube
    {
        public Vector2 position;

        public bool jumping;
        public float jumpOffset;
        public float jumpVelocity;

        public const float JUMP_VELOCITY = 9f;
        public const float GRAVITY = 0.8f;
        public const int CUBE_SPEED = 3;

        public Cube()
        {
            this.position = Vector2.Zero;
        }

        public void update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.D)) position.X += CUBE_SPEED;
            if (Keyboard.GetState().IsKeyDown(Keys.A)) position.X -= CUBE_SPEED;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) position.Y += CUBE_SPEED;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) position.Y -= CUBE_SPEED;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !jumping)
            {
                jumping = true;
                jumpVelocity = JUMP_VELOCITY;
            }

            if (jumping)
            {
                jumpVelocity -= GRAVITY;
                jumpOffset += jumpVelocity;

                if (jumpOffset <= 0)
                {
                    jumping = false;
                    jumpOffset = 0f;
                    jumpVelocity = 0f;
                }
            }

            List<TmxLayerTile> overlappingTiles = new List<TmxLayerTile>();

            foreach (var layer in Game1.map.Layers)
            {
                foreach (var tile in layer.Tiles)
                {
                    TmxTileset ts = Game1.mapRenderer.getTilesetFromGid(tile.Gid);
                    TmxTilesetTile tsTile = Utility.getTilesetTileFromGid(ts, tile.Gid);

                    if (tsTile != null)
                    {
                        if (tsTile.Properties.ContainsKey("StairType"))
                        {
                            Rectangle mybounds = new Rectangle((int)position.X, (int)position.Y, 12*4, 13*4);
                            Rectangle itsbounds = new Rectangle(tile.X*64, tile.Y*64, 64, 64);

                            if (mybounds.Intersects(itsbounds))
                            {
                                var type = tsTile.Properties["StairType"];

                                if (type == "LeftToRight")
                                {
                                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                                        position.Y -= CUBE_SPEED;
                                    else if (Keyboard.GetState().IsKeyDown(Keys.A))
                                        position.Y += CUBE_SPEED;
                                }
                                else if (type == "RightToLeft")
                                {
                                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                                        position.Y += CUBE_SPEED;
                                    else if (Keyboard.GetState().IsKeyDown(Keys.A))
                                        position.Y -= CUBE_SPEED;
                                }

                                break;
                            }
                        }
                    }
                }
            }
        }

        public void draw(SpriteBatch b, Vector2? offset = null, float opacity = 1f)
        {
            var pos = position;
            pos.Y -= jumpOffset;
            if (offset != null) pos += (Vector2)offset;

            b.Draw(Game1.cubeSprite, pos, null, Color.White * opacity, 0f, Vector2.Zero, 4f, SpriteEffects.None, 0f);
        }
    }
}
