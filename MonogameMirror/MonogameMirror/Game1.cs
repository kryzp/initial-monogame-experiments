using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using TiledSharp;

namespace MonogameMirror
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Texture2D mirrorSprite;
        public static Texture2D cubeSprite;

        public static Mirror mirror;
        public static Cube cube;

        public static TmxMap map;
        public static TmxMapRenderer mapRenderer;

        public static readonly RasterizerState ScissorEnabled = new RasterizerState()
        {
            ScissorTestEnable = true
        };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mirrorSprite = Content.Load<Texture2D>("mirror");
            cubeSprite = Content.Load<Texture2D>("cube");

            map = Content.Load<TmxMap>("map/map");
            //map = new TmxMap("map/map.tmx");

            mapRenderer = new TmxMapRenderer(map);
            mapRenderer.load(Content);

            mirror = new Mirror();
            cube = new Cube();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //mapRenderer.update();
            cube.update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            mapRenderer.draw(spriteBatch);
            spriteBatch.End();

            mirror.draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            cube.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
