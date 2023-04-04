using System;
using Microsoft.Xna.Framework;

namespace DarkWreath
{
	public abstract class CameraDriver
	{
		public abstract void Init(Camera camera);
		public abstract void Destroy(Camera camera);
		public abstract void Drive(Camera camera);
	}
	
	public class Camera
	{
		private Vector2 size = Vector2.Zero;
		private CameraDriver driver = null;
		
		public CameraDriver Driver
		{
			get => driver;

			set
			{
				driver?.Destroy(this);
				driver = value;
				driver?.Init(this);
			}
		}

		public Vector2 Size
		{
			get
			{
				return size;
			}
			
			set
			{
				size = value;
				Transform.Scale = new Vector2(Game1.WINDOW_WIDTH, Game1.WINDOW_HEIGHT) / size;
			}
		}
		
		public float Zoom { get; set; }
		public Transform Transform { get; set; }
		
		public Camera(float width, float height)
		{
			driver = null;
			Zoom = 1f;
			Transform = new Transform();
			Size = new Vector2(width, height);
		}

		public void Update()
		{
			Driver?.Drive(this);
		}

		public Rectangle GetViewport()
		{
			var pos = Transform.Position - Transform.Origin;

			return new Rectangle(
				(int)pos.X,
				(int)pos.Y,
				(int)Size.X,
				(int)Size.Y
			);
		}

		public Matrix GetViewMatrix()
		{
			return (
				Matrix.CreateTranslation(-Transform.Position.X, -Transform.Position.Y, 0f) *
				Matrix.CreateRotationZ(Transform.RotationRad) *
				Matrix.CreateScale(Zoom, Zoom, 1f) *
				Matrix.CreateTranslation(Transform.Origin.X, Transform.Origin.Y, 0f) *
				Matrix.CreateScale(Transform.Scale.X, Transform.Scale.Y, 1f)
			);
		}

		public Matrix GetPerfectViewMatrix(out Vector2 remainder)
		{
			var mat = GetViewMatrix();
			{
				remainder.X = mat.M31 - MathF.Floor(mat.M31);
				remainder.Y = mat.M32 - MathF.Floor(mat.M32);

				mat.M31 = MathF.Floor(mat.M31);
				mat.M32 = MathF.Floor(mat.M32);
			}
			return mat;
		}
	}
}
