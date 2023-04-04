using Anchored.Assets;
using Anchored.Assets.Maps;
using Anchored.CameraDrivers;
using Anchored.Debug;
using Anchored.World;
using Anchored.World.Components;
using Anchored.World.Types;
using Arch;
using Arch.Graphics;
using Arch.State;
using Arch.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Anchored.State
{
	public class PlayingState : GameState
	{
		private EntityWorld world;
		private Camera camera;

		public override void Load(SpriteBatch sb)
		{
			world = new EntityWorld();
			DebugConsole.World = world;

			SetupLevel(MapManager.Get("test"));

			Camera.Main = new Camera(Game1.WindowWidth / 4, Game1.WindowHeight / 4);
			Camera.Main.Origin = new Vector2(Game1.WindowWidth / 4 / 2, Game1.WindowHeight / 4 / 2);
			Camera.Main.Driver = new FollowDriver();
			Camera.Main.Follow = world.GetComponent<Player>().Entity;
			camera = Camera.Main;
		}

		public override void Unload()
		{
		}

		public override void Update()
		{
			if (Input.IsPressed(Keys.F5))
				Game1.ChangeState(new EditorState());

			world.Update();
			DebugConsole.Update();
		}

		public override void Draw(SpriteBatch sb)
		{
			world.Draw(sb, Camera.Main.GetPerfectViewMatrix());
		}

		public override void DrawUI(SpriteBatch sb)
		{
			sb.Begin(
				SpriteSortMode.FrontToBack,
				samplerState: SamplerState.PointClamp
			);

			sb.End();
		}

		public override void DrawDebug()
		{
			if (!DebugConsole.Open)
				return;

			DebugConsole.Draw();
			DebugManager.Draw(world);
		}

		private Entity SetupLevel(AnchoredMap map, bool loadEntities = true)
		{
			Entity tileMapEntity = world.AddEntity("TileMap");
			new LevelType(map, loadEntities).Create(tileMapEntity);
			return tileMapEntity;
		}
	}
}
