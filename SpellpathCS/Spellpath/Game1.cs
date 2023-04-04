using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Spellpath.Actors;
using Spellpath.Areas;
using Spellpath.Debug;
using Spellpath.Input;
using Spellpath.Menus;
using Spellpath.Tiles;

namespace Spellpath
{
    public class Game1 : Game
    {
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;
        public const int FRAMES_PER_SECOND = 144;
        public const int PIXEL_SCALE = 4;

        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

        private static RenderTarget2D mainRenderTarget;

        public static new GraphicsDevice GraphicsDevice;

        public static new ContentManager Content;
        public static ContentManager TiledContent;

        public static GameState CurrentState;
        public static GameArea CurrentArea;
        public static TiledMapRenderer MapRenderer;

        public static Player Player;
        public static Camera Camera;

        public static Texture2D WhitePixel;
        public static SpriteFont DebugFont;

        public static Texture2D MenuMisc;

        public static ClickableMenu ActiveClickableMenu;

        // .:: DEBUG VARIABLES ::. //
        public static DebugConsole DBG_Console;
        public static bool DBG_DrawDebugView = false;
        public static bool DBG_ForceQuit = false;
        // .:::::::::::::::::::::. //

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            System.IServiceProvider provider = base.Content.ServiceProvider;
            {
                Content = new ContentManager(provider);
                Content.RootDirectory = "Content";

                TiledContent = new ContentManager(provider);
                TiledContent.RootDirectory = "Content";
            }

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.TextInput += HandleTextInput;

            IsMouseVisible = true;
			IsFixedTimeStep = true;
            TargetElapsedTime = System.TimeSpan.FromMilliseconds(1000f / (float)FRAMES_PER_SECOND);

            Game1.GraphicsDevice = base.GraphicsDevice;

            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();

            mainRenderTarget = new RenderTarget2D(GraphicsDevice, WINDOW_WIDTH / PIXEL_SCALE, WINDOW_HEIGHT / PIXEL_SCALE);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            WhitePixel = Content.Load<Texture2D>("pixel");
            DebugFont = Content.Load<SpriteFont>("debug_font");
            MenuMisc = Content.Load<Texture2D>("menus\\misc");

            DBG_Console = new DebugConsole();

            MapRenderer = new TiledMapRenderer();

            Player = new Player();
            
            Camera = new Camera(WINDOW_WIDTH / PIXEL_SCALE, WINDOW_HEIGHT / PIXEL_SCALE);
            Camera.Driver = new FollowDriver(Player);

            ActiveClickableMenu = new InventoryMenu();

            SetCurrentArea(new AlchemyShopRegion());
        }

        protected override void Update(GameTime gameTime)
        {
            if (DBG_ForceQuit)
                Exit();
            
            InputManager.Update();

            DBG_Console.Update();
            if (DBG_Console.Open)
                return;
            
            if (CurrentState == GameState.Playing)
            {
                MapRenderer.Update(gameTime);
                CurrentArea?.Update(gameTime);
                Player.Update(gameTime);
            }

            ActiveClickableMenu?.Update(gameTime);

            //Camera.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(13, 13, 13));
            GraphicsDevice.SetRenderTarget(mainRenderTarget);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            {
                MapRenderer.DrawLayer("Back", gameTime, spriteBatch);
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetPerfectPositionViewMatrix());
            {
                if (CurrentState == GameState.Playing)
                {
                    MapRenderer.DrawLayer("Depth", gameTime, spriteBatch);
                    CurrentArea?.Draw(gameTime, spriteBatch);
                    Player.Draw(gameTime, spriteBatch);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            {
                MapRenderer.DrawLayer("Front", gameTime, spriteBatch);

                DBG_DrawDBGView(gameTime);
            }
            spriteBatch.End();
            
            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(mainRenderTarget, new Rectangle(0, 0, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
            {
                CurrentArea?.DrawUI(gameTime, spriteBatch);
                Player.DrawUI(gameTime, spriteBatch);

                ActiveClickableMenu?.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            DBG_Draw(gameTime);

            base.Draw(gameTime);
        }

        public static void WarpPlayer(string name, int px, int py)
        {
            SetCurrentArea(Util.GetGameAreaFromName(name));
            Player.Transform.Position = new Vector2(px, py);
            Camera.Position = Player.Transform.Position;
        }

        public static void SetCurrentArea(GameArea area)
        {
            CurrentArea = area;
            Player.CurrentArea = CurrentArea;
        }

        public static void PlaySound(string name)
        {
            // todo
        }

        private void HandleTextInput(object sender, TextInputEventArgs e)
        {
            DBG_Console.HandleTextInput(e.Key, e.Character);
        }

        public enum GameState
        {
            Playing
        };
        
        private void DBG_Draw(GameTime time)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate);
            DBG_Console.Draw(time, spriteBatch);
            spriteBatch.End();
        }

        private void DBG_DrawDBGView(GameTime time)
        {
            if (DBG_DrawDebugView)
            {
                spriteBatch.Draw(WhitePixel, (Rectangle)Player.Collider.GetWorldBounds(), Color.Aqua * 0.5f);

                foreach (var act in CurrentArea.Actors)
                    spriteBatch.Draw(WhitePixel, (Rectangle)act.Collider.GetWorldBounds(), Color.White * 0.5f);

                MapRenderer.DrawLayer("Meta", time, spriteBatch);
                MapRenderer.DrawLayer("Collision", time, spriteBatch);
            }
        }
    }
}
