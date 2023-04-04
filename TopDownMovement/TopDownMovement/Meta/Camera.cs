using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TopDownMovement.Meta
{
	public class Camera
	{
		private Vector2 targetPosition;
		
		private Viewport view;
		private Vector2 position;
		
		public Matrix Transform;

		public Camera(Viewport newView)
		{
			view = newView;
		}

		public void Update(GameTime gameTime)
		{
			targetPosition = Game1.MainPlayer.Position;
			position.X = MathHelper.Lerp(position.X, targetPosition.X - Game1.ScreenWidth / 2f, 0.2f);
			position.Y = MathHelper.Lerp(position.Y, targetPosition.Y - Game1.ScreenHeight / 2f, 0.2f);
			
			Transform = Matrix.CreateScale(new Vector3(Game1.WorldScale, Game1.WorldScale, 0)) *
			            Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0));
		}
	}
}