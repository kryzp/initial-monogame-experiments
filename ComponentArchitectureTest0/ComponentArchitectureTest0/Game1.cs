using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ComponentArchitectureTest0.ECS;
using ComponentArchitectureTest0.ECS.Entities;
using ComponentArchitectureTest0.ECS.Components.Transform;
using ComponentArchitectureTest0.ECS.Systems.Physics;
using ComponentArchitectureTest0.ECS.Components;
using ComponentArchitectureTest0.ECS.Components;
using System.Collections.Generic;
using ComponentArchitectureTest0.ECS.Components.Physics;
using ComponentArchitectureTest0.ECS.Components.Tuples;

namespace ComponentArchitectureTest0
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static World World;
        
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
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
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

            Entity boxEntity = World.CreateEntity();
            World.AddComponent(boxEntity, new PositionComponent());

            PhysicsSystem physics = new PhysicsSystem();

            World.AddSystem(physics);
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
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            World.Update(dt);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            World.Draw((float)gameTime.ElapsedGameTime.TotalSeconds, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
