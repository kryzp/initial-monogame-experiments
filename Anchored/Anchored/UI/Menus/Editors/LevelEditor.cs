using Anchored.State;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Num = System.Numerics;

namespace Anchored.UI.Menus.Editors
{
	public static class LevelEditor
	{
		private static Num.Vector2 position = new System.Numerics.Vector2(10, 10);
		private static Num.Vector2 size = new System.Numerics.Vector2(200, 400);

		public static Editor Editor;

		public static void DrawInGame(SpriteBatch sb)
		{
		}

		public static void Draw()
		{
			ImGui.SetNextWindowPos(new Num.Vector2(Game1.WindowWidth - position.X - size.X, position.Y), ImGuiCond.Once);
			ImGui.SetNextWindowSize(size, ImGuiCond.Once);
			ImGui.Begin("Levels", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize);
			{
			}
			ImGui.End();
		}
	}
}
