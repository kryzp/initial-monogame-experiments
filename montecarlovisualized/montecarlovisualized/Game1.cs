using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace montecarlovisualized
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private KeyboardState prevKbState;

        private RenderTarget2D nativeRenderTarget;
        private Effect crtEffect;

        private Texture2D txPoint;
        private Belief belief;

        private float positionX = 0f;
        private float positionY = 0f;
        private float velocityX = 0f;
        private float velocityY = 0f;

        private float prevPositionX = 0f;
        private float prevPositionY = 0f;

        public bool Running = false;

        public const int GRID_SIZE = 64;
        public const int WINDOW_WIDTH = 1280;
        public const int WINDOW_HEIGHT = 720;

        public int Timer = 0;

        public enum DrawMode
        {
            ALL,
            STANDARD
        };

        public DrawMode CurrentDrawMode = DrawMode.ALL;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();

            nativeRenderTarget = new RenderTarget2D(GraphicsDevice, 1280 / 2, 720 / 2);

            belief = new Belief(10000, GetRoughPositionX(), GetRoughPositionY());
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            txPoint = Content.Load<Texture2D>("point");
            crtEffect = Content.Load<Effect>("crt");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var kbState = Keyboard.GetState();
            var msState = Mouse.GetState();

            if (kbState.IsKeyDown(Keys.Space) && !prevKbState.IsKeyDown(Keys.Space))
                Running = !Running;

            if (kbState.IsKeyDown(Keys.G) && !prevKbState.IsKeyDown(Keys.G))
                CurrentDrawMode = (CurrentDrawMode == DrawMode.ALL) ? DrawMode.STANDARD : DrawMode.ALL;

            if (Running)
            {
                prevPositionX = positionX;
                prevPositionY = positionY;

                positionX = msState.Position.X / 2f;
                positionY = msState.Position.Y / 2f;

                belief.Update(GetRoughPositionX(), GetRoughPositionY());
            }

            prevKbState = kbState;

            crtEffect.Parameters["uTime"].SetValue(Timer);
            Timer++;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(13, 13, 15));

            GraphicsDevice.SetRenderTarget(nativeRenderTarget);
            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
            {
                DrawGrid();
                DrawMover();

                if (CurrentDrawMode == DrawMode.ALL)
                    DrawParticles();
                else if (CurrentDrawMode == DrawMode.STANDARD)
                    DrawParticle(belief.GetStandardParticle(), new Color(240, 20, 130));
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, samplerState: SamplerState.PointClamp);
            crtEffect.CurrentTechnique.Passes[0].Apply();
            {
                spriteBatch.Draw(nativeRenderTarget, new Rectangle(0, 0, 1280, 720), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGrid()
        {
            int thickness = 2;

            for (int y = -thickness/2; y < WINDOW_HEIGHT; y += GRID_SIZE)
                spriteBatch.DrawLine(new Vector2(0, y), new Vector2(WINDOW_WIDTH, y), new Color(25, 25, 30), thickness);

            for (int x = -thickness/2; x < WINDOW_WIDTH; x += GRID_SIZE)
                spriteBatch.DrawLine(new Vector2(x, 0), new Vector2(x, WINDOW_HEIGHT), new Color(25, 25, 30), thickness);

            spriteBatch.Draw(txPoint, new Rectangle(GetRoughPositionX(), GetRoughPositionY(), GRID_SIZE, GRID_SIZE), new Color(25, 25, 30));
        }

        private void DrawMover()
        {
            Particle moverParticle = new Particle()
            {
                X = positionX,
                Y = positionY,
                VX = positionX - prevPositionX,
                VY = positionY - prevPositionY
            };

            DrawParticle(moverParticle, new Color(240, 115, 30));
        }

        private void DrawParticles()
        {
            foreach (var particle in belief.Particles)
                DrawParticle(particle, new Color(240, 20, 50) * 0.01f);
        }

        private void DrawParticle(Particle particle, Color colour)
        {
            spriteBatch.Draw(txPoint, new Rectangle((int)particle.X - 1, (int)particle.Y - 1, 2, 2), colour);

            spriteBatch.DrawLine(
                new Vector2(
                    particle.X,
                    particle.Y
                ),
                new Vector2(
                    particle.X + particle.VX*10f,
                    particle.Y + particle.VY*10f
                ),
                colour,
                1
            );
        }

        private int GetRoughPositionX()
        {
            return (int)(MathF.Floor(positionX / GRID_SIZE) * GRID_SIZE);
        }

        private int GetRoughPositionY()
        {
            return (int)(MathF.Floor(positionY / GRID_SIZE) * GRID_SIZE);
        }
    }
}
