using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace montecarlovisualized
{
    public static class Utility
    {
        public static Random RNG = new Random();

        private static Texture2D texture;

        public static float NextFloat(this Random rng, float min, float max)
        {
            return (float)(rng.NextDouble() * (max - min) + min);
        }

        private static Texture2D GetWhitePixel(SpriteBatch spriteBatch)
        {
            if (texture == null)
            {
                texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
                texture.SetData(new[] { Color.White });
            }

            return texture;
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = MathF.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            spriteBatch.Draw(GetWhitePixel(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
        }
    }
}
