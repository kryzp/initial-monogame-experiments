using Anchored.Assets;
using Anchored.State;
using Anchored.World;
using Anchored.World.Components;
using Anchored.Save;
using Anchored.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Arch.Math.Tween;
using Arch;
using Arch.Assets;
using Arch.UI;
using Arch.Assets.Maps.Serialization;
using Newtonsoft.Json;
using Anchored.Assets.Maps;
using Anchored.Assets.Textures;
using Anchored.UI.Elements;
using Arch.Util;
using Anchored.UI.Constraints;
using Arch.Graphics;
using Arch.World;

namespace Anchored
{
	public class Game1 : Engine
	{
		public static Entity Player;

		public Game1()
			: base("Anchored", 1280, 720)
		{
		}

		protected override void LoadContent()
		{
			base.LoadContent();

			Options.Load(SaveManager.GetOptionsFilePath());

			Camera.Main = new Camera(WindowWidth, WindowHeight);
			Camera.Main.Origin = Vector2.Zero;

			SaveManager.Init();
			ChangeState(new AssetLoadState());
		}

		protected override void UnloadContent()
		{
			base.UnloadContent();

			SubTextureManager.Destroy();
			MapManager.Destroy();
		}

		protected override void Update(GameTime gt)
		{
			base.Update(gt);
		}

		protected override void Draw(GameTime gt)
		{
			base.Draw(gt);
		}
	}
}
