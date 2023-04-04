using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS1.ComponentArchitecture.Components.Transform
{
	public class TransformComponent : EntityComponent
	{
		public PositionComponent Position = new PositionComponent();
		public RotationComponent Rotation = new RotationComponent();
		public ScaleComponent Scale = new ScaleComponent();

		public override void Update(float deltaTime)
		{
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}
	}
}
