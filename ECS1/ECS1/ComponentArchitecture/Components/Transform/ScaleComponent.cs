using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS1.ComponentArchitecture.Components.Transform
{
	public class ScaleComponent : EntityComponent
	{
		public float X { get; set; } = 1f;
		public float Y { get; set; } = 1f;

		public override void Update(float deltaTime)
		{
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}
	}
}
