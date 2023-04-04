using System.Collections.Generic;
using System.Linq;
using CollisionTest.Meta;
using CollisionTest.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollisionTest
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		private Camera camera;
		
		public static List<Sprite> Sprites;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = @"Content\bin";
		}

		protected override void Initialize()
		{
			Sprites = new List<Sprite>();

			GraphicsDevice.Viewport = new Viewport(0, 0, 320, 180);
			camera = new Camera(GraphicsDevice.Viewport);

			graphics.PreferredBackBufferWidth = 1280;
			graphics.PreferredBackBufferHeight = 720;
			graphics.ApplyChanges();
			
			IsMouseVisible = true;
			base.Initialize();
		}

		protected override void LoadContent()
		{
			var pointerTex = Content.Load<Texture2D>("pointer");
			var solidTex = Content.Load<Texture2D>("solid");
			
			Sprites.Add(new SolidSprite(solidTex)
			{
				Position = new Vector2(320 / 2, 180 / 2)
			});
			
			Sprites.Add(new MouseFollower(pointerTex)
			{
				Position = new Vector2(100, 100)
			});
			
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void UnloadContent()
		{
		}

		protected override void Update(GameTime gameTime)
		{
			foreach(var sprite in Sprites)
			{
				sprite.Update(gameTime);
			}
			
			var collidableSprites = Sprites.Where(c => c is ICollidable);

			foreach(var a in collidableSprites)
			foreach(var b in collidableSprites)
			{
				if(a == b)
					continue;

				if(!a.CollisionArea.Intersects(b.CollisionArea))
					continue;

				if(a.Intersects(b, Vector2.Zero))
				{
					((ICollidable)a).OnCollide(b);
				}
			}
			
			int spriteCount = Sprites.Count;
			for(int i = 0; i < spriteCount; i++)
			{
				var sprite = Sprites[i];
				foreach (var child in sprite.Children)
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

			camera.Update(gameTime);
			base.Update(gameTime);
		}
        
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: camera.Transform);

			foreach(var sprite in Sprites)
			{
				var rectangle = sprite.CollisionArea;
				Texture2D rect = new Texture2D(graphics.GraphicsDevice, rectangle.Width, rectangle.Height);
				Color[] data = new Color[rectangle.Width * rectangle.Height];
				for(int ii = 0; ii < data.Length; ++ii) data[ii] = Color.Chocolate;
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