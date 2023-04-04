﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrappyBird.States
{
	public abstract class StateBase
	{
		protected Game1 game;
		protected ContentManager Content;

		public StateBase(Game1 game, ContentManager content)
		{
			this.game = game;
			this.Content = content;
		}

		public abstract void LoadContent();
		public abstract void Update(GameTime gameTime);
		public abstract void PostUpdate(GameTime gameTime);
		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
		public abstract void DrawGUI(GameTime gameTime, SpriteBatch spriteBatch);
	}
}