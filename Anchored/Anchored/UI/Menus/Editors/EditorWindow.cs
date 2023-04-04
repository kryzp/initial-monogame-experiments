using Anchored.State;
using Arch;
using Arch.Assets;
using Arch.Math;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Num = System.Numerics;
using System.IO;
using Anchored.Assets.Maps;
using Anchored.UI.Menus.Editors.Command;

namespace Anchored.UI.Menus.Editors
{
	public class EditorWindow
	{
		// todo todo holy shit what the flying fuck oh my god why oh why why why do you do this oh my god
		// e n g i n e e r   g a m i n g
		public const string CONTENT_PATH = @"D:\Projects\Anchored\Anchored\Content\";

		private static Num.Vector2 position = new Num.Vector2(10, 10);
		private static Num.Vector2 size = new Num.Vector2(400, 500);

		public static readonly Color BackgroundBoxColorA = new Color(192, 192, 192);
		public static readonly Color BackgroundBoxColorB = new Color(128, 128, 128);
		public const int GRID_SIZE = 64;

		private bool showLoadMapDialog = false;
		private string currentMap;
		private List<string> maps = new List<string>();

		public Editor Editor;
		public CommandQueue Commands;

		public static bool ShowGrid = true;
		public static bool SnapToGrid = true;

		public enum Category
		{
			Tile,
			Entity
		}

		public Category CurrentCategory;

		public EditorWindow(Editor editor, List<string> maps)
		{
			this.maps = maps;

			this.Editor = editor;
			this.CurrentCategory = Category.Tile;

			Commands = new CommandQueue();
			Commands.Editor = editor;

			EntityEditor.Editor = editor;

			TileEditor.Editor = editor;
			TileEditor.Window = this;

			LevelEditor.Editor = editor;
		}

		public void DrawInGame(SpriteBatch sb)
		{
			if (Editor.Map != null)
			{
				sb.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, transformMatrix: Editor.Camera.GetPerfectViewMatrix());
				DrawTiles(sb);
				DrawEntityPreviews(sb);
				sb.End();
			}

			if (CurrentCategory == Category.Tile)
			{
				TileEditor.DrawInGame(sb);
			}
			else if (CurrentCategory == Category.Entity)
			{
				EntityEditor.DrawInGame(sb);
			}
			
			LevelEditor.DrawInGame(sb);
			
			if (ShowGrid && Editor.Map != null)
			{
				sb.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp, transformMatrix: Editor.Camera.GetPerfectViewMatrix());
				DrawGrid(sb);
				sb.End();
			}
		}

		public void Draw()
		{
			if (Input.Ctrl())
			{
				if (Input.IsPressed(Keys.S))
					Save();
			}

			ImGui.SetNextWindowPos(position, ImGuiCond.Once);
			ImGui.SetNextWindowSize(size, ImGuiCond.Once);
			ImGui.Begin("Map Editor", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
			{
				{
					int w = 150;
					int h = 20;

					if (ImGui.Button("Load", new Num.Vector2(w, h)))
						showLoadMapDialog = true;

					ImGui.SameLine();

					if (ImGui.Button("Save", new Num.Vector2(w, h)))
						Save();
				}

				ImGui.Separator();

				if (showLoadMapDialog)
				{
					if (ImGui.BeginCombo("##file", currentMap))
					{
						foreach (var map in maps)
						{
							bool isSelected = (currentMap == map);

							if (ImGui.Selectable(map, isSelected))
								currentMap = map;

							if (isSelected)
								ImGui.SetItemDefaultFocus();
						}
						ImGui.EndCombo();
					}

					if (ImGui.Button("LOAD"))
					{
						if (maps.Contains(currentMap))
						{
							Load(MapManager.Get(currentMap));
							showLoadMapDialog = false;
						}
					}
				}
				else
				{
					if (ImGui.BeginCombo("##category", CurrentCategory.ToString()))
					{
						foreach (EditorWindow.Category val in Enum.GetValues(typeof(EditorWindow.Category)))
						{
							bool isSelected = (val == CurrentCategory);

							if (ImGui.Selectable(val.ToString(), isSelected))
								CurrentCategory = val;

							if (isSelected)
								ImGui.SetItemDefaultFocus();
						}
						ImGui.EndCombo();
					}

					ImGui.Checkbox("Show Grid", ref ShowGrid);
					ImGui.Checkbox("Snap to Grid", ref SnapToGrid);

					ImGui.Separator();

					if (CurrentCategory == Category.Tile)
					{
						TileEditor.Draw();
					}
					else if (CurrentCategory == Category.Entity)
					{
						EntityEditor.Draw();
					}
				}
			}
			ImGui.End();

			LevelEditor.Draw();

			ImGui.SetNextWindowPos(new Num.Vector2(10, Game1.WindowHeight - 35 - 10), ImGuiCond.Once);
			ImGui.SetNextWindowSize(new Num.Vector2(150, 35), ImGuiCond.Once);
			ImGui.Begin("Mouse Position", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
			{
				var mousePosition = Input.MouseWorldPosition(Editor.Camera.GetViewMatrix()) / 4f;
				string mousePositionTxt = $"{mousePosition.X}, {mousePosition.Y}";
				ImGui.TextUnformatted(mousePositionTxt);
			}
			ImGui.End();
		}

		private void DrawGrid(SpriteBatch sb)
		{
			var col = Color.White * (1f/3f);

			for (int x = 0; x < Editor.Map.MapWidth + 1; x++)
			{
				Utility.DrawLine(
					new Line(
						x * GRID_SIZE,
						0,
						x * GRID_SIZE,
						Editor.Map.MapHeight * GRID_SIZE
					),
					col,
					1f,
					0.95f,
					sb
				);
			}

			for (int y = 0; y < Editor.Map.MapHeight + 1; y++)
			{
				Utility.DrawLine(
					new Line(
						0,
						y * GRID_SIZE,
						Editor.Map.MapWidth * GRID_SIZE,
						y * GRID_SIZE
					),
					col,
					1f,
					0.95f,
					sb
				);
			}
		}

		private void DrawTiles(SpriteBatch sb)
		{
			Editor.Map.Draw(sb, 4);
		}

		private void DrawEntityPreviews(SpriteBatch sb)
		{
			foreach (var entt in Editor.Map.Entities)
				entt.Type.DrawPreview(sb, entt.Position * 4f, 4f);
		}

		public void Save()
		{
			JsonSerializer serializer = new JsonSerializer();

			using (StreamWriter sw = new StreamWriter(CONTENT_PATH + @"maps\" +Editor.Map.Name + @".map"))
			using (JsonWriter writer = new JsonTextWriter(sw))
			{
				serializer.Serialize(writer, Editor.Map.JsonData);
			}
		}

		public void Load(AnchoredMap map)
		{
			Editor.Map = map;
			TileEditor.Reload();
			EntityEditor.Reload();
		}
	}
}
