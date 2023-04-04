using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace vectorfieldmg
{
    public struct Particle
    {
        private List<Vector2> trail;
        public float x;
        public float y;

        public Particle(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.trail = new List<Vector2>();
        }

        public void Update()
        {
            /*
            trail.Add(new Vector2(x / Game1.GRID_PX_SIZE, y / Game1.GRID_PX_SIZE));

            if (trail.Count > 50)
                trail.RemoveAt(0);
            */

            var vel = Game1.Function1(x, y);
            x += vel.X;
            y += vel.Y;
        }

        public void Draw(SpriteBatch b)
        {
            b.Draw(Game1.WHITE_PIXEL, new Rectangle((int)x, (int)y, 1, 1), Color.Aqua);

            foreach (var p in trail)
                b.Draw(Game1.WHITE_PIXEL, new Rectangle((int)p.X, (int)p.Y, 1, 1), Color.Aqua * 0.01f);
        }
    };

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public static Texture2D WHITE_PIXEL;
        public const int GRID_PX_SIZE = 64;
        public const int WINDOW_WIDTH = 1280;
        public const int WINDOW_HEIGHT = 720;
        public const float VEC_SCALE = 0.05f;
        public float POSX = 0f;
        public float POSY = 0f;
        public float ZOOMX = 1f;
        public float ZOOMY = 1f;
        public List<Particle> Particles = new List<Particle>();
        public static Random RNG = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            WHITE_PIXEL = Content.Load<Texture2D>("pixel");

            for (int i = 0; i < 5000; i++)
            {
                Particles.Add(new Particle(
                    Util.RandomRange(-WINDOW_WIDTH / 2, WINDOW_WIDTH / 2),
                    Util.RandomRange(-WINDOW_HEIGHT / 2, WINDOW_HEIGHT / 2)
                ));
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                ZOOMX = MathF.Max(0.2f, ZOOMX - 0.01f);
                ZOOMY = MathF.Max(0.2f, ZOOMY - 0.01f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                ZOOMX += 0.01f;
                ZOOMY += 0.01f;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A)) POSX += 5f;
            if (Keyboard.GetState().IsKeyDown(Keys.D)) POSX -= 5f;
            if (Keyboard.GetState().IsKeyDown(Keys.W)) POSY += 5f;
            if (Keyboard.GetState().IsKeyDown(Keys.S)) POSY -= 5f;

            foreach (Particle p in Particles)
                p.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(13, 13, 13));

            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: GetViewMatrix());
            {
                DrawGraph();
                DrawVectors();
                DrawParticles();
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // random name but this is the function the vector field and particles use
        public static Vector2 Function1(float x, float y)
        {
            return new Vector2(
                x-y,
                x
            );
        }

        private void DrawGraph()
        {
            spriteBatch.Draw(WHITE_PIXEL, new Rectangle(-5000/2, 0, 5000, 1), Color.White * 0.75f);
            spriteBatch.Draw(WHITE_PIXEL, new Rectangle(0, -5000/2, 1, 5000), Color.White * 0.75f);

            for (int x = 0; x < 5000; x += GRID_PX_SIZE)
                spriteBatch.Draw(WHITE_PIXEL, new Rectangle(x, -5000/2, 1, 5000), Color.White * 0.15f);
            for (int x = 0; x > -5000; x -= GRID_PX_SIZE)
                spriteBatch.Draw(WHITE_PIXEL, new Rectangle(x, -5000/2, 1, 5000), Color.White * 0.15f);

            for (int y = 0; y < 5000; y += GRID_PX_SIZE)
                spriteBatch.Draw(WHITE_PIXEL, new Rectangle(-5000/2, y, 5000, 1), Color.White * 0.15f);
            for (int y = 0; y > -5000; y -= GRID_PX_SIZE)
                spriteBatch.Draw(WHITE_PIXEL, new Rectangle(-5000/2, y, 5000, 1), Color.White * 0.15f);
        }

        private void DrawVectors()
        {
            for (int y = -32; y < 32; y++)
            {
                for (int x = -32; x < 32; x++)
                {
                    Vector2 vector = Function1(x, y);
                    DrawVector(x, y, vector);
                }
            }
        }

        private void DrawVector(float x, float y, Vector2 vector)
        {
            y *= -1;
            vector.Y *= -1;
            vector *= GRID_PX_SIZE;
            vector *= VEC_SCALE;

            var position = new Vector2(x, y) * GRID_PX_SIZE;
            var tgtvec = position + vector;

            spriteBatch.DrawLine(position, tgtvec, Color.Aqua, 0f, 1f);

            float deg = MathHelper.ToRadians(20);
            float len = MathF.Min(15f, MathF.Abs(vector.Length()) * 0.75f);

            var arrowheadleft = new Vector2(
                vector.X*MathF.Cos(deg) - vector.Y*MathF.Sin(deg),
                vector.X*MathF.Sin(deg) + vector.Y*MathF.Cos(deg)
            ) * -1;

            var arrowheadright = new Vector2(
                vector.X*MathF.Cos(-deg) - vector.Y*MathF.Sin(-deg),
                vector.X*MathF.Sin(-deg) + vector.Y*MathF.Cos(-deg)
            ) * -1;

            arrowheadleft.Normalize();
            arrowheadright.Normalize();

            spriteBatch.DrawLine(tgtvec, tgtvec + arrowheadleft*len, Color.Aqua, 0f, 1f);
            spriteBatch.DrawLine(tgtvec, tgtvec + arrowheadright*len, Color.Aqua, 0f, 1f);
        }

        private void DrawParticles()
        {
            foreach (Particle p in Particles)
                p.Draw(spriteBatch);
        }

        private Matrix GetViewMatrix()
        {
            return (
                Matrix.CreateTranslation(POSX, POSY, 0f) * // position
                Matrix.CreateScale(ZOOMX, ZOOMY, 1f) * // zoom
                Matrix.CreateTranslation(1280/2, 720/2, 0f) // origin
            );
        }
    }
}
