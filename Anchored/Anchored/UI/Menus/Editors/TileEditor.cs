using Anchored.State;
using Anchored.World.Components;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Arch;
using Arch.Math;
using Arch.Assets;
using System.Runtime.InteropServices;
using Num = System.Numerics;
using Arch.Assets.Maps;
using Microsoft.Xna.Framework.Input;
using Anchored.UI.Menus.Editors.Command;

// TODO: I USE THE FIRST LAYER OF THE MAP FOR EVERYTHING
//       IN THE FUTURE THE LAYERS WINDOW SHOULD BE USED TO INDICATE WHAT LAYER TO EDIT!

namespace Anchored.UI.Menus.Editors
{
	public static class TileEditor
	{
		private static Num.Vector2 tileSize = new Num.Vector2(32, 32);
		private static List<TileInfo> infos = new List<TileInfo>();
		private static IntPtr tilesetPtr = new IntPtr();

		private static float painterPositionX;
		private static float painterPositionY;
		private static float painterColorBlend = 0f;

		private static TileInfo currentSelectedTile;

		public static readonly Color PainterColorA = new Color(69, 247, 238);
		public static readonly Color PainterColorB = new Color(87, 144, 242);

		public const int TILE_LIST_TILE_WIDTH = 5;

		public static Editor Editor;
		public static EditorWindow Window;

		public static void DrawInGame(SpriteBatch sb)
		{
			if (Editor.Map != null)
			{
				sb.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, transformMatrix: Editor.Camera.GetPerfectViewMatrix());
				
				if (!Input.GuiBlocksMouse)
				{
					DrawPainterBox(sb);

					if (currentSelectedTile != null)
						DrawTilePreview(sb);
				}

				if (Input.IsDown(MouseButton.Left))
				{
					int x = (int)(painterPositionX / 64);
					int y = (int)(painterPositionY / 64);
					var layer = GetCurrentLayer();

					if (currentSelectedTile != null && layer.Data[y, x] != currentSelectedTile.Tile.ID)
					{
						if (x >= 0 && x < layer.Width &&
							y >= 0 && y < layer.Height)
						{
							Window.Commands.Do(new SetTileCommand()
							{
								X = x,
								Y = y,
								ID = currentSelectedTile.Tile.ID,
								PriorID = layer.Data[y, x],
								Layer = layer
							});
						}
					}
				}

				sb.End();
			}
		}

		public static void Draw()
		{
			foreach (var info in infos)
			{
				ImGui.PushID(info.Tile.ID);

				if (((info.Tile.ID-1)%TileEditor.TILE_LIST_TILE_WIDTH)!=0)
					ImGui.SameLine();

				Num.Vector4 tint = new Num.Vector4(255, 255, 255, 255);
				bool selected = false;

				if (info == currentSelectedTile)
				{
					selected = true;
					ImGui.PushStyleColor(ImGuiCol.Border, new Num.Vector4(255, 255, 255, 255));
					ImGui.PushStyleVar(ImGuiStyleVar.FrameBorderSize, 5.0f);
				}

				if (Input.IsPressed(Keys.Escape))
				{
					currentSelectedTile = null;
				}

				if (ImGui.ImageButton(info.Texture, tileSize, info.Uv0, info.Uv1, 0, new Num.Vector4(0, 0, 0, 255), tint))
				{
					currentSelectedTile = info;
				}

				if (selected)
				{
					ImGui.PopStyleColor();
					ImGui.PopStyleVar();
				}

				ImGui.PopID();
			}
		}

		private static void DrawPainterBox(SpriteBatch sb)
		{
			var mouse = Input.MouseWorldPosition(Editor.Camera.GetViewMatrix());
			int targetMouseX = (int)(MathF.Floor(mouse.X/(EditorWindow.GRID_SIZE))*(EditorWindow.GRID_SIZE));
			int targetMouseY = (int)(MathF.Floor(mouse.Y/(EditorWindow.GRID_SIZE))*(EditorWindow.GRID_SIZE));

			painterPositionX = targetMouseX;//MathHelper.Lerp(painterPositionX, targetMouseX, 40 * Time.RawDelta);
			painterPositionY = targetMouseY;//MathHelper.Lerp(painterPositionY, targetMouseY, 40 * Time.RawDelta);

			painterColorBlend = MathF.Sin(Time.TotalSeconds * 4f);
			RectangleF rect = new RectangleF(painterPositionX, painterPositionY, EditorWindow.GRID_SIZE, EditorWindow.GRID_SIZE);
			Color col = Utility.BlendColours(PainterColorA, PainterColorB, painterColorBlend);
			Utility.DrawRectangleOutline(rect, col, 2, 0.95f, sb);
		}

		private static void DrawTilePreview(SpriteBatch sb)
		{
			GetCurrentLayer().Tileset.GetTileTexture(currentSelectedTile.Tile).Draw(
				new Rectangle((int)painterPositionX, (int)painterPositionY, 64, 64),
				Vector2.Zero,
				Color.White * 0.5f,
				0f,
				0.95f,
				sb
			);
		}

		private static void DefineTiles()
		{
			int width = 3;

			foreach (var tile in GetCurrentLayer().Tileset.Tiles)
			{
				DefineTile(
					tile,
					Utility.GetXFromIndexAndWidth(tile.ID-1, width) * GetCurrentLayer().Tileset.TileSize,
					Utility.GetYFromIndexAndWidth(tile.ID-1, width) * GetCurrentLayer().Tileset.TileSize
				);
			}
		}

		private static void DefineTile(Tile tile, int x, int y)
		{
			// temp
			infos.Add(new TileInfo(tile, GetCurrentLayer().Tileset.Texture, tilesetPtr, x, y));
		}

		public static void Reload()
		{
			tilesetPtr = Game1.ImGuiRenderer.BindTexture(GetCurrentLayer().Tileset.Texture);
			infos.Clear();
			DefineTiles();
		}

		private static Layer GetCurrentLayer()
		{
			return Editor.Map.Layers[/*temp*/0/*temp*/];
		}
	}
}
