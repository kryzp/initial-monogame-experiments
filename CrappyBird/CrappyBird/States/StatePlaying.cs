using CrappyBird.Models;
using CrappyBird.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using CrappyBird.Managers;

namespace CrappyBird.States
{
	public class StatePlaying : StateBase
	{
		private List<Sprite> sprites;
		private List<PipePair> pipePairs;

		private PipePairManager pipePairManager;

		public StatePlaying(Game1 game, ContentManager content) : base(game, content)
		{
		}

		public override void LoadContent()
		{
			sprites = new List<Sprite>();
			pipePairs = new List<PipePair>();

			var playerTex = Content.Load<Texture2D>("players/yellowBirb");

			var bottomPipeTex = Content.Load<Texture2D>("pipes/bottomPipe");
			var topPipeTex = Content.Load<Texture2D>("pipes/topPipe");

			pipePairManager = new PipePairManager(sprites, topPipeTex, bottomPipeTex);

			sprites.Add(new Player(playerTex)
			{
				Position = new Vector2(200f, 200f),
				Input = new PlayerInput()
				{
					Flap = Keys.Space
				},
				Score = new PlayerScore()
				{
					Value = 0
				}
			});
		}

		public override void Update(GameTime gameTime)
		{
			pipePairManager.Update(gameTime);

			foreach(var sprite in sprites)
				sprite.Update(gameTime);

			foreach(var pipePair in pipePairs)
				pipePair.Update(gameTime);

			if(pipePairManager.CanSpawn)
				pipePairs.Add(pipePairManager.getPipePair());
		}

		public override void PostUpdate(GameTime gameTime)
		{
			var collidableSprites = sprites.Where(c => c is ICollidable);

			foreach(var a in collidableSprites)
			{
				foreach(var b in collidableSprites)
				{
					if(a == b)
						continue;

					if(a.Intersects(b, Vector2.Zero))
						((ICollidable)a).OnCollide(b);
				}
			}

			int spriteCount = sprites.Count;
			for(int i = 0; i < spriteCount; i++)
			{
				var sprite = sprites[i];
				foreach(var child in sprite.Children)
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
			foreach(var sprite in sprites)
				sprite.Draw(gameTime, spriteBatch);

			foreach(var pipePair in pipePairs)
				pipePair.Draw(gameTime, spriteBatch);
		}

		public override void DrawGUI(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// TODO: Draw GUI
		}
	}
}
