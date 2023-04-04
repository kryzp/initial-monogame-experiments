using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScaleTestHalfPixels.Sprites;
using System.Collections.Generic;
using System.Linq;

namespace ScaleTestHalfPixels
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static List<Sprite> Sprites;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            Sprites = new List<Sprite>();

            IsMouseVisible = true;
            base.Initialize();
        }
		
        protected override void LoadContent()
        {
            var spriteTex = Content.Load<Texture2D>("testSprite");
            var mouseSpriteTex = Content.Load<Texture2D>("pointer");

            Sprites.Add(new TestSprite(spriteTex)
            {
                Position = new Vector2(1280 / 2, 720 / 2)
            });
            Sprites.Add(new MouseSprite(mouseSpriteTex));

            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // #TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            foreach(var sprite in Sprites)
                sprite.Update(gameTime);

			#region Collision Detection
			var collidableSprites = Sprites.Where(c => c is Collidable);

			foreach(var a in collidableSprites)
				foreach(var b in collidableSprites)
				{
					if(a == b)
						continue;

					if(!a.CollisionArea.Intersects(b.CollisionArea))
						continue;

					if(a.Intersects(b, Vector2.Zero))
						((Collidable)a).OnCollide(b);
				}

			int spriteCount = Sprites.Count;
			for(int i = 0; i < spriteCount; i++)
			{
				var sprite = Sprites[i];
				foreach(var child in sprite.Children)
                    Sprites.Add(child);

				sprite.Children = new List<Sprite>();
			}

			for(int i = 0; i < Sprites.Count; i++)
			{
				if(Sprites[i].IsRemoved)
				{
                    Sprites.RemoveAt(i);
					i--;
				}
			}
			#endregion

			base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            foreach(var sprite in Sprites)
            {
                var rectangle = sprite.CollisionArea;
                Texture2D rect = new Texture2D(graphics.GraphicsDevice, rectangle.Width, rectangle.Height);
                Color[] data = new Color[rectangle.Width * rectangle.Height];
				for(int i = 0; i < data.Length; ++i)
					data[i] = Color.Chocolate;
                rect.SetData(data);
                Vector2 coor = new Vector2(rectangle.X, rectangle.Y);
                spriteBatch.Draw(rect, coor, Color.White);

                sprite.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
	}
}
