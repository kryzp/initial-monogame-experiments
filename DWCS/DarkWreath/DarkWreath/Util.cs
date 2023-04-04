using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DarkWreath.Math;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DarkWreath
{
	
    public static class Util
    {
        public static Random Random = new Random();
        
        public static float NextFloat(this Random rng, float min, float max)
        {
            return (float)(rng.NextDouble() * (max - min) + min);
        }

        public static Vector2 UnitVector(this Random rng)
        {
            float theta = rng.NextFloat(0f, MathF.PI * 2f);
            
            return new Vector2(
                MathF.Cos(theta),
                MathF.Sin(theta)
            );
        }

        public static Vector2 ReflectVector(Vector2 incident, Vector2 normal)
        {
            return incident - (2f * Vector2.Dot(normal, incident) * normal);
        }
        
        public static float Snap(float number, float interval)
        {
            if (interval <= 1f)
                return MathF.Floor(number) + (MathF.Round((number - MathF.Floor(number)) * (1f / interval)) * interval);
            else
                return MathF.Round(number / interval) * interval;
        }

        public static string TextAfter(this string value, string search)
        {
            return value.Substring(value.IndexOf(search) + search.Length);
        }

        public static string TextBefore(this string value, string search)
        {
            return value.Substring(0, value.IndexOf(search));
        }

        public static void DrawRect(this SpriteBatch b, Rectangle rect, Color color, float layerDepth = 0f)
        {
            b.Draw(Game1.WhitePixel, rect, null, color, 0f, Vector2.Zero, SpriteEffects.None, layerDepth);
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
            b.Draw(Game1.WhitePixel, point, null, color, angle, origin, scale, SpriteEffects.None, layerDepth);
        }

        public static void DrawLineGradient(this SpriteBatch b, Vector2 point1, Vector2 point2, Color color, float layerDepth = 0f, float thickness = 1f)
        {
            var distance = Vector2.Distance(point1, point2);
            var angle = MathF.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            DrawLine(b, point1, distance, angle, color, layerDepth, thickness);
        }

        public static void DrawLineGradient(this SpriteBatch b, Vector2 point, float length, float angle, Color color, float layerDepth = 0f, float thickness = 1f)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(length, thickness);
            b.Draw(Game1.WhiteGradient, point, null, color, angle, origin, scale / new Vector2(Game1.WhiteGradient.Width, 1f), SpriteEffects.None, layerDepth);
        }

        public static void Project(this Polygon polygon, Vector2 axis, ref float min, ref float max)
        {
            float dot = Vector2.Dot(polygon.Vertices[0], axis);
            min = dot;
            max = dot;

            for (int i = 1; i < polygon.Vertices.Length; i++)
            {
                dot = Vector2.Dot(polygon.Vertices[i], axis);
                min = MathF.Min(dot, min);
                max = MathF.Max(dot, max);
            }
        }

        public static Texture2D CreateTexture(GraphicsDevice graphics, int width, int height, Func<int, Color> paint)
        {
            var texture = new Texture2D(graphics, width, height);
            Color[] data = new Color[width * height];
            for (int pixel = 0; pixel < data.Length; pixel++)
                data[pixel] = paint(pixel);
            texture.SetData(data);
            return texture;
        }

        public static float Angle(this Vector2 v)
        {
            return MathF.Atan2(v.Y, v.X);
        }

        public static Vector2 Normalized(this Vector2 v)
        {
            Vector2 vv = v;
            vv.Normalize();
            return vv;
        }
        
        public static T DeepClone<T>(this T obj)
        {
			using MemoryStream ms = new MemoryStream();
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(ms, obj);
			ms.Position = 0;
			return (T)formatter.Deserialize(ms);
		}

        public static float Sigmoid_01(float x)
        {
            return 1f - (1f / (1f + MathF.Exp(x)));
        }

        public static float Sigmoid(float x)
        {
            return 1f - (2f / (1f + MathF.Exp(x)));
        }
        
        public static float Spring(float from, float to, float bounciness, float tension, ref float intermediate)
        {
            intermediate = MathHelper.Lerp(intermediate, (to - from) * tension, 1f / bounciness);
            return from + intermediate;
        }

        public static Vector2 Spring(Vector2 from, Vector2 to, float bounciness, float tension, ref Vector2 intermediate)
        {
            return new Vector2(
                Spring(from.X, to.Y, bounciness, tension, ref intermediate.X),
                Spring(from.Y, to.Y, bounciness, tension, ref intermediate.Y)
            );
        }

        public static Rectangle GetSourceRectForTilesheet(Texture2D tilesheet, int id, int tilesize = 16)
        {
            return new Rectangle(
                id * tilesize % tilesheet.Width,
                id * tilesize / tilesheet.Width * tilesize,
                tilesize,
                tilesize
            );
        }

        public static RectangleF ToRectangleF(this Rectangle r)
        {
            return new RectangleF(r.X, r.Y, r.Width, r.Height);
        }
    }
}
