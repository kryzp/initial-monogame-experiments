using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TopDownMovement.Meta;
using TopDownMovement.Models;
using TopDownMovement.Sprites;

namespace TopDownMovement
{
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public static Camera NativeCamera;
		public static Random Random;

		public static int WorldWidth = 320;
		public static int WorldHeight = 180;
		public static int WorldScale = 1;
		
		public static int ScreenWidth = 1280;
		public static int ScreenHeight = 720;

		public static Player MainPlayer;
		public static List<Sprite> Sprites;
		
		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = @"Content\bin";
		}

		protected override void Initialize()
		{
			Random = new Random();
			
			GraphicsDevice.Viewport = new Viewport(0, 0, WorldWidth, WorldHeight);
			NativeCamera = new Camera(GraphicsDevice.Viewport);

			graphics.PreferredBackBufferWidth = ScreenWidth;
			graphics.PreferredBackBufferHeight = ScreenHeight;
			graphics.ApplyChanges();
			
			IsMouseVisible = true;
			base.Initialize();
		}

		protected override void LoadContent()
		{
			var playerTex = Content.Load<Texture2D>("player");
			var wallTex = Content.Load<Texture2D>("environment/wall");
			
			Sprites = new List<Sprite>();
			
			MainPlayer = new Player(playerTex)
			{
				Position = new Vector2(400, 400),
				Input = new PlayerInput()
				{
					Up = Keys.W,
					Down = Keys.S,
					Left = Keys.A,
					Right = Keys.D
				}
			};

			Sprites.Add(MainPlayer);
			
			Sprites.Add(new Wall(wallTex) { Position = new Vector2(800, 400) });
			Sprites.Add(new Wall(wallTex) { Position = new Vector2(800 - 16 * 4, 400) });
			Sprites.Add(new Wall(wallTex) { Position = new Vector2(800, 400 - 16 * 4) });
			
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
			
			#region Collision Detection
			var collidableSprites = Sprites.Where(c => c is ICollidable);

			foreach(var a in collidableSprites)
			foreach(var b in collidableSprites)
			{
				if(a == b)
					continue;

				if(!a.CollisionArea.Intersects(b.CollisionArea))
					continue;

				if(a.Intersects(b, Vector2.Zero))
					((ICollidable)a).OnCollide(b);
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
			
			NativeCamera.Update(gameTime);
			base.Update(gameTime);
		}
        
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp, transformMatrix: NativeCamera.Transform);

			foreach(var sprite in Sprites)
			{
				sprite.Draw(gameTime, spriteBatch);
				
				/*
				var rectangle = sprite.CollisionArea;
				Texture2D rect = new Texture2D(graphics.GraphicsDevice, rectangle.Width, rectangle.Height);
				Color[] data = new Color[rectangle.Width * rectangle.Height];
				for(int i = 0; i < data.Length; ++i)
					data[i] = Color.Chocolate;
				rect.SetData(data);
				Vector2 coor = new Vector2(rectangle.X, rectangle.Y);
				spriteBatch.Draw(rect, coor, Color.White);
				*/
			}
			
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}