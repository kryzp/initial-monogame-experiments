using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollisionTest.Meta
{
	public class Camera
	{
		private Viewport view;
		private Vector2 origin;
		
		public Matrix Transform;

		public Camera(Viewport newView)
		{
			view = newView;
		}

		public void Update(GameTime gameTime)
		{
			origin = new Vector2(0, 0);
			Transform = Matrix.CreateScale(new Vector3(4, 4, 0)) *
			            Matrix.CreateTranslation(new Vector3(-origin.X, -origin.Y, 0));
		}
	}
}