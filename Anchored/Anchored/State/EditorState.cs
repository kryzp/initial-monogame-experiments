using Arch.State;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using ImGuiNET;
using Anchored.Assets.Maps;
using Anchored.Debug;
using Anchored.UI.Menus.Editors;
using Arch;
using Microsoft.Xna.Framework.Input;
using Anchored.World.Types;
using Arch.Graphics;
using Microsoft.Xna.Framework;
using Arch.Streams;
using Arch.Math.Tween;
using Anchored.World;
using Arch.World;

namespace Anchored.State
{
	public class EditorState : GameState
	{
		private Vector2 mousePanStartPosition;
		private Vector2 mousePanPosition;
		private Vector2 mousePanAmount => mousePanPosition - mousePanStartPosition;
		private Vector2 mousePanStartCameraPosition;

		private List<string> maps = new List<string>();
		private float cameraTargetZoom = 1f;

		public EditorWindow Editor;
		public AnchoredMap Map;
		public Camera Camera;
		public EntityWorld World;

		public EditorState()
		{
		}

		public override void Load(SpriteBatch sb)
		{
			{
				var mapDir = FileHandle.FromRoot("maps\\");

				if (mapDir.Exists())
					LoadMapStrings(mapDir);
			}

			Camera.Main = new Camera(Game1.WindowWidth, Game1.WindowHeight);
			Camera.Main.Origin = new Vector2(Game1.WindowWidth / 2, Game1.WindowHeight / 2);

			World = new EntityWorld();
			Map = null;
			Camera = Camera.Main;

			Editor = new EditorWindow(
				new Editor()
				{
					Map = Map,
					World = World,
					Camera = Camera
				},
				maps
			);
		}

		public override void Unload()
		{
		}

		public override void Update()
		{
			if (Input.IsPressed(Keys.F5))
			{
				Editor.Save();
				Game1.ChangeState(new PlayingState());
			}

			DebugConsole.Update();

			if (Input.Ctrl())
			{
				if (Input.MouseScrollChange() != 0)
				{
					cameraTargetZoom += Input.MouseScrollChange() / 4f;
					cameraTargetZoom = MathHelper.Max(cameraTargetZoom, 0);
				}

				if (Input.IsPressed(Keys.Z))
					Editor.Commands.Undo();

				if (Input.IsPressed(Keys.Y))
					Editor.Commands.Redo();
			}

			if (Input.IsPressed(MouseButton.Middle))
			{
				mousePanStartPosition = Input.MouseScreenPosition().ToVector2();
				mousePanStartCameraPosition = Camera.Position;
			}

			if (Input.IsDown(MouseButton.Middle))
			{
				mousePanPosition = Input.MouseScreenPosition().ToVector2();
				Camera.Position = mousePanStartCameraPosition - ((mousePanAmount / Camera.Scale.X) / Camera.Zoom);
			}

			if (Map != null)
			{
				// Camera
				{
					int mx = 0;
					int my = 0;

					float mspd = 555f * Time.RawDelta;

					if (!Input.Ctrl())
					{
						if (Input.IsDown(Keys.W, true))
							my -= 1;

						if (Input.IsDown(Keys.S, true))
							my += 1;

						if (Input.IsDown(Keys.A, true))
							mx -= 1;

						if (Input.IsDown(Keys.D, true))
							mx += 1;
					}

					Camera.Position.X += mspd * mx;
					Camera.Position.Y += mspd * my;

					Camera.Zoom = MathHelper.Lerp(Camera.Zoom, cameraTargetZoom, 35 * Time.RawDelta);
				}
			}

		}

		public override void Draw(SpriteBatch sb)
		{
			Editor.DrawInGame(sb);
		}

		public override void DrawUI(SpriteBatch sb)
		{
		}

		public override void DrawDebug()
		{
			Editor.Draw();

			if (DebugConsole.Open)
				DebugConsole.Draw();
		}

		private void LoadMapStrings(FileHandle handle)
		{
			foreach (var h in handle.ListFileHandles())
				LoadMap(h);

			foreach (var h in handle.ListDirectoryHandles())
				LoadMap(h);
		}

		private void LoadMap(FileHandle handle)
		{
			if (handle.IsDirectory())
				LoadMapStrings(handle);

			maps.Add(handle.NameWithoutExtension);
		}
	}
}
