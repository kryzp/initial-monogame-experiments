﻿using Anchored.Graphics;
using Arch.Math;
using Arch.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Arch
{
	public static class Utility
	{
		public static Vector2 GetOrigin(OriginPosition position, TextureRegion texture)
		{
			Vector2 result = Vector2.Zero;

			switch (position)
			{
				case OriginPosition.TopLeft:
					result.X = 0;
					result.Y = 0;
					break;

				case OriginPosition.TopCenter:
					result.X = texture.Width / 2;
					result.Y = 0;
					break;

				case OriginPosition.TopRight:
					result.X = texture.Width;
					result.Y = 0;
					break;

				case OriginPosition.CenterLeft:
					result.X = 0;
					result.Y = texture.Height / 2;
					break;

				case OriginPosition.CenterCenter:
					result.X = texture.Width / 2;
					result.Y = texture.Height / 2;
					break;

				case OriginPosition.CenterRight:
					result.X = texture.Width;
					result.Y = texture.Height / 2;
					break;

				case OriginPosition.BottomLeft:
					result.X = 0;
					result.Y = texture.Height;
					break;

				case OriginPosition.BottomCenter:
					result.X = texture.Width / 2;
					result.Y = texture.Height;
					break;

				case OriginPosition.BottomRight:
					result.X = texture.Width;
					result.Y = texture.Height;
					break;
			}

			return result;
		}

		public static T FromByteArray<T>(byte[] data)
		{
			if (data == null)
				return default(T);
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream(data))
			{
				object obj = bf.Deserialize(ms);
				return (T)obj;
			}
		}

		public static object FromByteArray(Type type, byte[] data)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (var ms = new MemoryStream(data))
			{
				object obj = bf.Deserialize(ms);
				return Convert.ChangeType(obj, type);
			}
		}

		public static byte[] ToByteArray<T>(T obj)
		{
			if (obj == null)
				return null;
			BinaryFormatter bf = new BinaryFormatter();
			using (MemoryStream ms = new MemoryStream())
			{
				bf.Serialize(ms, obj);
				return ms.ToArray();
			}
		}

		public static string WordWrap(SpriteFont font, string text, float maxLineWidth)
		{
			string[] words = text.Split(' ');
			StringBuilder sb = new StringBuilder();

			float lineWidth = 0f;
			float spaceWidth = font.MeasureString(" ").X;

			foreach (string word in words)
			{
				Vector2 size = font.MeasureString(word);

				if (lineWidth + size.X < maxLineWidth)
				{
					sb.Append(word + " ");
					lineWidth += size.X + spaceWidth;
				}
				else
				{
					sb.Append("\n" + word + " ");
					lineWidth = size.X + spaceWidth;
				}
			}

			return sb.ToString();
		}

		public static Rectangle ConvertStringToRectangle(string raw)
		{
			string[] values = raw.Split(' ');

			Int32 x = Int32.Parse(values[0]);
			Int32 y = Int32.Parse(values[1]);
			Int32 w = Int32.Parse(values[2]);
			Int32 h = Int32.Parse(values[3]);

			return new Rectangle(x, y, w, h);
		}

		public static string ConvertRectangleToString(Rectangle raw)
		{
			return $"{raw.Width} {raw.Height} {raw.Width} {raw.Height}";
		}

		public static Vector2 ConvertStringToVector2(string raw)
		{
			string[] values = raw.Split(' ');

			Single x = Single.Parse(values[0]);
			Single y = Single.Parse(values[1]);

			return new Vector2(x, y);
		}

		public static string ConvertVector2ToString(Vector2 raw)
		{
			return $"{raw.X} {raw.Y}";
		}


		public static Color BlendColours(Color a, Color b, float factor)
		{
			return new Color(
				(byte)MathHelper.Clamp(((a.R * factor) + (b.R * (1f - factor))), 0, 255),
				(byte)MathHelper.Clamp(((a.G * factor) + (b.G * (1f - factor))), 0, 255),
				(byte)MathHelper.Clamp(((a.B * factor) + (b.B * (1f - factor))), 0, 255)
			);
		}

		public static Texture2D GetWhitePixel(SpriteBatch spriteBatch = null)
		{
			if (spriteBatch == null)
				spriteBatch = Engine.SpriteBatch;
			Texture2D texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			texture.SetData(new[] { Color.White });
			return texture;
		}

		public static void DrawRectangle(Rectangle rectangle, Color color, float layer = 0.95f, SpriteBatch sb = null)
		{
			if (sb == null)
				sb = Engine.SpriteBatch;
			Texture2D rect = new Texture2D(
				sb.GraphicsDevice,
				(int)rectangle.Width,
				(int)rectangle.Height
			);
			Color[] data = new Color[(int)rectangle.Width * (int)rectangle.Height];
			for (int ii = 0; ii < data.Length; ii++)
				data[ii] = color;
			rect.SetData(data);
			Vector2 coor = new Vector2(rectangle.X, rectangle.Y);
			sb.Draw(rect, coor, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, layer);
		}
		
		public static void DrawRectangleOutline(RectangleF rectangle, Color color, float t = 1f, float layer = 0.95f, SpriteBatch sb = null)
		{
			if (sb == null)
				sb = Engine.SpriteBatch;

			var tl = new Vector2(rectangle.Left, rectangle.Top - t);
			var tr = new Vector2(rectangle.Right, rectangle.Top);
			var bl = new Vector2(rectangle.Left, rectangle.Bottom);
			var br = new Vector2(rectangle.Right, rectangle.Bottom);

			var top    = new Line(new Vector2(rectangle.Left,  rectangle.Top),          new Vector2(rectangle.Right, rectangle.Top));
			var right  = new Line(new Vector2(rectangle.Right, rectangle.Top-(t/2)),    new Vector2(rectangle.Right, rectangle.Bottom+(t/2)));
			var bottom = new Line(new Vector2(rectangle.Right, rectangle.Bottom),       new Vector2(rectangle.Left,  rectangle.Bottom));
			var left   = new Line(new Vector2(rectangle.Left,  rectangle.Bottom+(t/2)), new Vector2(rectangle.Left,  rectangle.Top-(t/2)));

			DrawLine(top, color, t, layer, sb);
			DrawLine(left, color, t, layer, sb);
			DrawLine(right, color, t, layer, sb);
			DrawLine(bottom, color, t, layer, sb);
		}

		public static void DrawLine(Line line, Color color, float thickness = 1f, float layer = 0.95f, SpriteBatch sb = null)
		{
			if (sb == null)
				sb = Engine.SpriteBatch;
			DrawLine(line.A, line.B, color, thickness, layer, sb);
		}

		public static void DrawLine(Vector2 point1, Vector2 point2, Color color, float thickness = 1f, float Layer = 0.95f, SpriteBatch sb = null)
		{
			if (sb == null)
				sb = Engine.SpriteBatch;
			var distance = Vector2.Distance(point1, point2);
			var angle = MathF.Atan2(point2.Y - point1.Y, point2.X - point1.X);
			DrawLine(point1, distance, angle, color, thickness, Layer, sb);
		}

		public static void DrawLine(Vector2 point, float length, float angle, Color color, float thickness = 1f, float layer = 0.95f, SpriteBatch sb = null)
		{
			if (sb == null)
				sb = Engine.SpriteBatch;
			var origin = new Vector2(0f, 0.5f);
			var scale = new Vector2(length, thickness);
			sb.Draw(GetWhitePixel(sb), point, null, color, angle, origin, scale, SpriteEffects.None, layer);
		}

		public static void DrawCircleOutline(Circle circle, int steps, Color color, float thickness = 1f, float layer = 0.95f, SpriteBatch sb = null)
		{
			Vector2 lastInner = new Vector2(circle.Center.X + circle.Radius - thickness, circle.Center.Y);
			Vector2 lastOuter = new Vector2(circle.Center.X + circle.Radius, circle.Center.Y);

			for (int ii = 1; ii <= steps; ii++)
			{
				float radians = (ii / (float)steps) * MathHelper.Tau;
				Vector2 normal = new Vector2(MathF.Cos(radians), MathF.Sin(radians));

				Vector2 nextInner = new Vector2(
					circle.Center.X + (normal.X * (circle.Radius - thickness)),
					circle.Center.Y + (normal.Y * (circle.Radius - thickness))
				);

				Vector2 nextOuter = new Vector2(
					circle.Center.X + (normal.X * circle.Radius),
					circle.Center.Y + (normal.Y * circle.Radius)
				);

				Polygon polygon = new Polygon(
					new List<Vector2>()
					{
						lastInner,
						lastOuter,
						nextOuter,
						nextInner
					}
				);

				DrawPolygonOutline(polygon, color, thickness, layer, sb);

				lastInner = nextInner;
				lastOuter = nextOuter;
			}
		}

		public static void DrawPolygonOutline(Polygon polygon, Color color, float thickness = 1f, float layer = 0.95f, SpriteBatch sb = null)
		{
			polygon.ForeachPoint((ii, vertex, nextVertex) =>
			{
				Line line = new Line(vertex, nextVertex);
				DrawLine(line, color, thickness, layer, sb);
			});
		}

		public static Texture2D CreateTexture(int width, int height, Func<int, Color> paint, SpriteBatch sb = null)
		{
			if (sb == null)
				sb = Engine.SpriteBatch;
			var texture = new Texture2D(sb.GraphicsDevice, width, height);
			Color[] data = new Color[width * height];
			for (int pixel = 0; pixel < data.Length; pixel++)
				data[pixel] = paint(pixel);
			texture.SetData(data);
			return texture;
		}

		public static int GetXFromIndexAndWidth(int index, int width)
		{
			return index % width;
		}

		public static int GetYFromIndexAndWidth(int index, int width)
		{
			return (int)(MathF.Floor(index / width));
		}
	}
}
