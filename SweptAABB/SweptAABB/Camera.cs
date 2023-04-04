using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweptAABB
{
	public class Camera
	{
		private Vector2 targetPosition { get; set; }

		private Viewport viewport;

		private bool targetOverride = false;

		private Vector2 origin { get; set; }
		private Vector2 position { get; set; }
		private float scale { get; set; }
		private float rotation { get; set; }

		public Vector2 Origin
		{
			get => origin;
			set => origin = value;
		}

		public Vector2 Position
		{
			get => position;
			set
			{
				position = value;
				targetPosition = value;
			}
		}

		public float Scale
		{
			get => scale;
			set => scale = value;
		}

		public float Rotation
		{
			get => rotation;
			set => rotation = value;
		}

		public Matrix Transform { get; set; }

		public Vector2 TargetOverride
		{
			get => targetPosition;
			set
			{
				targetOverride = true;
				targetPosition = value;
			}
		}

		public Camera(Viewport view)
		{
			viewport = view;

			targetPosition = Position;

			Origin = Vector2.Zero;
			Scale = 4f;
		}

		public void Update(float deltaTime)
		{
			if(targetOverride)
			{
				var pos = Position;
				pos.X = MathHelper.Lerp(pos.X, targetPosition.X, 0.15f);
				pos.Y = MathHelper.Lerp(pos.Y, targetPosition.Y, 0.15f);
				position = pos;
			}

			if(new Vector2((int)targetPosition.X, (int)targetPosition.Y) == new Vector2((int)Position.X, (int)Position.Y))
			{
				targetOverride = false;
			}

			Transform = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
						Matrix.CreateRotationZ(Rotation) *
						Matrix.CreateScale(new Vector3(Scale, Scale, 0)) *
						Matrix.CreateTranslation(Origin.X, Origin.Y, 0);
		}

		public void Reset()
		{
			targetOverride = false;
			Origin = Vector2.Zero;
			Scale = 4f;
			Rotation = 0f;
		}
	}
}
