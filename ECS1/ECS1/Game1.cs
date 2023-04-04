using ECS1.ComponentArchitecture;
using ECS1.ComponentArchitecture.Components.Physics;
using ECS1.ComponentArchitecture.Components.Transform;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ECS1.ComponentArchitecture.Components.Movement;
using ECS1.Meta.Cam;
using ECS1.ComponentArchitecture.Components.Visual;

namespace ECS1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Camera MainCamera;
        public static World World;

        public static int WorldWidth = 320;
        public static int WorldHeight = 180;

        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            MainCamera = new Camera(GraphicsDevice.Viewport);

            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();

            IsMouseVisible = true;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            World = new World();

            Texture2D boxTex = Content.Load<Texture2D>("box");

			//==========[Player Entity]=======//
			{
				Entity playerEntity = new Entity();

                TransformComponent transform = new TransformComponent();
                playerEntity.AddComponent(transform);

                VelocityComponent velocity = new VelocityComponent(transform.Position);
				playerEntity.AddComponent(velocity);

                InputMovementComponent input = new InputMovementComponent(velocity);
				playerEntity.AddComponent(input);

                SpriteComponent sprite = new SpriteComponent(boxTex, transform);
                playerEntity.AddComponent(sprite);

				TriggerColliderComponent trigger = new TriggerColliderComponent(sprite);
                trigger.CollisionArea = sprite.Rectangle;
                playerEntity.AddComponent(trigger);

                World.AddEntity(playerEntity);
			}
			//================================//

			//==========[Box Entity]==========//
			{
				Entity boxEntity = new Entity();

                TransformComponent transform = new TransformComponent();
                transform.Position.X = 100f;
                transform.Position.Y = 90f;
                boxEntity.AddComponent(transform);

				SpriteComponent sprite = new SpriteComponent(boxTex, transform);
                boxEntity.AddComponent(sprite);

				TriggerColliderComponent trigger = new TriggerColliderComponent(sprite);
                trigger.CollisionArea = sprite.Rectangle;
                boxEntity.AddComponent(trigger);

				World.AddEntity(boxEntity);
			}
			//================================//

		}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            World.Update(deltaTime);
            MainCamera.Update(deltaTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: MainCamera.Transform);
            World.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
