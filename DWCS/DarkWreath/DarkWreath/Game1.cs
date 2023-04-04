using System;
using DarkWreath.Actors;
using DarkWreath.Areas;
using DarkWreath.Graphics.ShaderEffects;
using DarkWreath.Input;
using DarkWreath.Math;
using DarkWreath.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DarkWreath
{
	public class Game1 : Game
	{
		public const int WINDOW_WIDTH = 1280;
		public const int WINDOW_HEIGHT = 720;
		public const int FRAMES_PER_SECOND = 144;
		public const int PIXEL_SCALE = 2;
		
		private static GraphicsDeviceManager graphics;
		private static SpriteBatch spriteBatch;
		
		private static RenderTarget2D mainRenderTarget;
		public static new GraphicsDevice GraphicsDevice;

		public static new ContentManager Content;
		public static ContentManager MapContent;

		public static TiledMapRenderer MapRenderer;
		public static GameArea CurrentArea;
		public static Camera MainCamera;

		public static bool MarauderUIMode;

		public static Texture2D WhitePixel;
		public static Texture2D WhiteCircle;
		public static Texture2D WhiteGradient;

		public static Texture2D ProjectileSheet;
		
		public static SpriteFont DebugFont;

		public static IInputProvider PlayerInput;
		public static Player Player;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);

			System.IServiceProvider provider = base.Content.ServiceProvider;
			{
				Content = new ContentManager(provider);
				Content.RootDirectory = "Content";

				MapContent = new ContentManager(provider);
				MapContent.RootDirectory = "Content";
			}

			IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			Window.TextInput += HandleTextInput;

			IsMouseVisible = true;
			IsFixedTimeStep = true;
			TargetElapsedTime = TimeSpan.FromMilliseconds(1000f / FRAMES_PER_SECOND);

			Game1.GraphicsDevice = base.GraphicsDevice;

			graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
			graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
			graphics.ApplyChanges();

			mainRenderTarget = new RenderTarget2D(GraphicsDevice, WINDOW_WIDTH, WINDOW_HEIGHT);
			
			PlayerInput = new InputProvider();
			
			MarauderUIMode = false;

			base.Initialize();
		}
		
		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			WhitePixel = Content.Load<Texture2D>("white_px");
			WhiteCircle = Content.Load<Texture2D>("white_circle_128");
			WhiteGradient = Content.Load<Texture2D>("white_gradient");

			ProjectileSheet = Content.Load<Texture2D>("items/weapons/ranged/projectiles");

			DebugFont = Content.Load<SpriteFont>("debug_font");

			MapRenderer = new TiledMapRenderer();

			Player = new Player();

			MainCamera = new Camera(WINDOW_WIDTH / PIXEL_SCALE, WINDOW_HEIGHT / PIXEL_SCALE);
			MainCamera.Transform.Origin = new Vector2(WINDOW_WIDTH / (PIXEL_SCALE * 2f), WINDOW_HEIGHT / (PIXEL_SCALE * 2f));
			MainCamera.Driver = new FollowDriver(Player);
			
			ShaderEffects.Load(Content);
			
			SetCurrentArea(new DebugArea());
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			Time.Delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			Time.Total += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			
			InputManager.Update();
			
			DebugConsole.Update();
			if (DebugConsole.Open)
				return;

			// marauder mode temp
			{
				if (PlayerInput.GetState().MarauderModeToggle)
				{
					MarauderUIMode = !MarauderUIMode;
					
					if (MarauderUIMode)
						ShaderEffects.MarauderUIEffect.Init();
				}

				if (MarauderUIMode)
					ShaderEffects.MarauderUIEffect.Update();
			}
			
			MainCamera.Update();
			
			MapRenderer.Update(gameTime);
			CurrentArea?.Update(gameTime);
			Player.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.SetRenderTarget(mainRenderTarget);
			{
				GraphicsDevice.Clear(Color.Black);
				
				spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: MainCamera.GetPerfectViewMatrix(out _));
				{
					MapRenderer.DrawLayer("Back", spriteBatch);
				}
				spriteBatch.End();

				spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: MainCamera.GetPerfectViewMatrix(out _));
				{
					CurrentArea?.Draw(gameTime, spriteBatch);
					Player.Draw(gameTime, spriteBatch);
					MapRenderer.DrawLayer("Depth", spriteBatch);
				}
				spriteBatch.End();

				spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: MainCamera.GetPerfectViewMatrix(out _));
				{
					MapRenderer.DrawLayer("Front", spriteBatch);
					
					if (DebugConsole.DBG_DrawDebugView)
					{
						spriteBatch.Draw(WhitePixel, Player.Collider.WorldBounds.ToRectangle(), Color.Aqua * 0.5f);

						foreach (var act in CurrentArea.Actors)
						{
							if (act.Collider != null)
								spriteBatch.Draw(WhitePixel, act.Collider.WorldBounds.ToRectangle(), Color.White * 0.5f);
						}

						MapRenderer.DrawLayer("Meta", spriteBatch);
						MapRenderer.DrawLayer("Collision", spriteBatch);
					}
				}
				spriteBatch.End();
				
				spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
				{
					CurrentArea?.DrawUI(gameTime, spriteBatch);
					Player.DrawUI(gameTime, spriteBatch);
				}
				spriteBatch.End();
			}
			GraphicsDevice.SetRenderTarget(null);
			
			GraphicsDevice.Clear(Color.Black);
			{
				spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);

				if (MarauderUIMode)
					ShaderEffects.MarauderUIEffect.Apply();

				spriteBatch.Draw(mainRenderTarget, new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);
				spriteBatch.End();
				
				spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.LinearClamp);
				DebugConsole.Draw(gameTime, spriteBatch);
				spriteBatch.End();
			}

			base.Draw(gameTime);
		}

		public static void SetCurrentArea(GameArea area)
		{
			CurrentArea = area;
			Player.CurrentArea = CurrentArea;
		}

		private void HandleTextInput(object sender, TextInputEventArgs e)
		{
			DebugConsole.HandleTextInput(e.Key, e.Character);
		}
		
		public static void ScreenShake(float amount)
		{
			ScreenShake(Util.Random.UnitVector() * amount);
		}
		
		public static void ScreenShake(Vector2 amount)
		{
			Game1.MainCamera.Transform.Position += amount;
		}
	}
}
