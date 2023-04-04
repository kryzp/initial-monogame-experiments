using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ECS1.ComponentArchitecture.Components.Transform
{
	public class RotationComponent : EntityComponent
	{
		private float rotation = 0f;

		public float Degrees
		{
			get => MathHelper.ToDegrees(rotation);
			set => rotation = MathHelper.ToRadians(value);
		}

		public float Radians
		{
			get => rotation;
			set => rotation = value;
		}

		public override void Update(float deltaTime)
		{
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}
	}
}
