using System;
using System.Collections.Generic;
using System.Linq;
using Asteroids.Managers;
using Asteroids.Models;
using Asteroids.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids.States
{
	public class StateDebug : StateBase
	{
		private AsteroidManager asteroidManager;

		private SpriteFont scoreFont;
		private SpriteFont healthFont;
		
		private List<Sprite> sprites;

		public StateDebug(Game1 game, ContentManager content)
			: base(game, content)
		{
		}

		public override void LoadContent()
		{
			var playerTex = content.Load<Texture2D>("ship");
			var bulletTex = content.Load<Texture2D>("shipBullet");

			scoreFont = content.Load<SpriteFont>("fonts/arial");
			healthFont = content.Load<SpriteFont>("fonts/arial");
			
			asteroidManager = new AsteroidManager(content);
			
			var bulletPrefab = new Bullet(bulletTex)
			{
				Layer = 0.1f
			};

			sprites = new List<Sprite>()
			{
				new Player(playerTex)
				{
					Colour = Color.White,
					Position = new Vector2(160, 90),
					Layer = 0.5f,
					Health = 5,
					Bullet = bulletPrefab,
					Input = new Input()
					{
						Thrust = Keys.C,
						Shoot = Keys.X, 
						TurnLeft = Keys.Left,
						TurnRight = Keys.Right
					},
					Score = new Score()
					{
						PlayerName = "Player 1",
						Value = 0
					}
				}
			};
		}

		public override void Update(GameTime gameTime)
		{
			foreach(var sprite in sprites)
				sprite.Update(gameTime);
			
			asteroidManager.Update(gameTime);

			if(sprites.Count(c => c is Asteroid) == 0)
				for(int ii = 0; ii <= 3; ii++)
				{
					var asteroid = asteroidManager.GetAsteroid();
					
					asteroid.Position = new Vector2(
						Game1.Random.Next(-Game1.WorldWidth, Game1.WorldWidth),
						Game1.Random.Next(-Game1.WorldHeight, Game1.WorldHeight)
					);
					
					sprites.Add(asteroid);
				}
		}

		public override void PostUpdate(GameTime gameTime)
		{
			var collidableSprites = sprites.Where(c => c is ICollidable);

			foreach(var a in collidableSprites)
			foreach(var b in collidableSprites)
			{
				if(a == b)
					continue;

				if(!a.CollisionArea.Intersects(b.CollisionArea))
					continue;

				if(a.Intersects(b))
					((ICollidable)a).OnCollide(b);
			}
			
			int spriteCount = sprites.Count;
			for(int i = 0; i < spriteCount; i++)
			{
				var sprite = sprites[i];
				foreach (var child in sprite.Children)
					sprites.Add(child);

				sprite.Children = new List<Sprite>();
			}

			for(int i = 0; i < sprites.Count; i++)
			{
				if(sprites[i].IsRemoved)
				{
					sprites.RemoveAt(i);
					i--;
				}
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

			foreach(var sprite in sprites)
				sprite.Draw(gameTime, spriteBatch);

			spriteBatch.End();
		}

		public override void DrawGUI(GameTime gameTime, SpriteBatch spriteBatch)
		{
			int dist = 25;
			var player = (Player)sprites[0];
			spriteBatch.DrawString(healthFont, "Health: " + player.Health, new Vector2(dist, dist), Color.White);
			spriteBatch.DrawString(scoreFont, "Score: " + player.Score.Value, new Vector2(dist, Game1.ScreenHeight - (dist * 4)), Color.White);
		}
	}
}