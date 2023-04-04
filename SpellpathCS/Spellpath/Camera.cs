using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spellpath
{
	public class Camera
	{
		private int m_width;
		private int m_height;

		public int Width => m_width;
		public int Height => m_height;
		public Vector2 Size => new Vector2(m_width, m_height);

		public Vector2 Position = Vector2.Zero;
		public float Zoom = 1f;
		public Vector2 Scale = Vector2.One * 4;
		public Vector2 Origin = Vector2.Zero;
		
		private float m_rotation = 0f;

		public float RotationDeg
		{
			get => MathHelper.ToDegrees(m_rotation);
			set => m_rotation = MathHelper.ToRadians(value);
		}

		public float RotationRad
		{
			get => m_rotation;
			set => m_rotation = value;
		}

		private CameraDriver m_driver;

		public CameraDriver Driver
		{
			get => m_driver;

			set
			{
				m_driver?.Destroy();
				m_driver = value;
				m_driver.Camera = this;
				m_driver?.Init();
			}
		}

		public Camera(int width, int height)
		{
			this.m_width = width;
			this.m_height = height;

			Scale.X = Game1.WINDOW_WIDTH / width;
			Scale.Y = Game1.WINDOW_HEIGHT / height;
		}

		public void Update(GameTime time)
		{
			m_driver?.Update(time);
		}

		public Viewport GetViewport()
		{
			return new Viewport(
				(int)(Position.X - Origin.X),
				(int)(Position.Y - Origin.Y),
				(int)(Game1.WINDOW_WIDTH / Scale.X),
				(int)(Game1.WINDOW_HEIGHT / Scale.Y)
			);
		}

		public Matrix GetViewMatrix()
		{
			return (
				Matrix.CreateTranslation((Width / 2f) - Position.X, (Height / 2f) - Position.Y, 0f) *
				Matrix.CreateRotationZ(m_rotation) *
				Matrix.CreateScale(Zoom, Zoom, 1f) *
				Matrix.CreateTranslation(Origin.X, Origin.Y, 0f) *
				Matrix.CreateScale(Scale.X, Scale.Y, 1f)
			);
		}

		public Matrix GetPerfectPositionViewMatrix()
		{
			Matrix mat = GetViewMatrix();

			mat.M41 = (int)mat.M41;
			mat.M42 = (int)mat.M42;
			mat.M43 = (int)mat.M43;

			return mat;
		}

		/*
		private void ClampPositionToMap()
		{
			int worldwidth = Game1.CurrentArea.Map.WidthInPixels*4;
			int worldheight = Game1.CurrentArea.Map.HeightInPixels*4;

			Position.X = MathHelper.Clamp(Position.X, 0, worldwidth - this.Width);
			Position.Y = MathHelper.Clamp(Position.Y, 0, worldheight - this.Height);
		}
		*/

		// TODO: bool Sees(Actor a) functions!
	}
}
