using ECS1.ComponentArchitecture.Components.Transform;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS1.ComponentArchitecture.Components.Physics
{
	public class VelocityComponent : EntityComponent
	{
		private PositionComponent position;

		public float X { get; set; }
		public float Y { get; set; }

		public float XA { get; set; }
		public float YA { get; set; }

		public VelocityComponent(PositionComponent p)
		{
			position = p;
		}

		public override void Update(float deltaTime)
		{
			// Movement
			position.X += X;
			position.Y += Y;

			// Acceleration
			X += XA;
			Y += YA;
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}
	}
}
