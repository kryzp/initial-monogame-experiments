using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace vectorfieldmg
{
    public static class Util
    {
        public static float RandomRange(float mn, float mx)
        {
            return (float)(Game1.RNG.NextDouble() * (mx - mn) + mn);
        }

        public static void DrawLine(this SpriteBatch b, Vector2 point1, Vector2 point2, Color color, float layerDepth = 0f, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = MathF.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(b, point1, distance, angle, color, layerDepth, thickness);
        }

        public static void DrawLine(this SpriteBatch b, Vector2 point, float length, float angle, Color color, float layerDepth = 0f, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            b.Draw(Game1.WHITE_PIXEL, point, null, color, angle, origin, scale, SpriteEffects.None, layerDepth);
        }
    }
}
