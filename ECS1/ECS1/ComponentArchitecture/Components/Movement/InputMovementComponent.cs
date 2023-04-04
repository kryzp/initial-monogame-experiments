using ECS1.ComponentArchitecture.Components.Physics;
using ECS1.ComponentArchitecture.Components.Transform;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace ECS1.ComponentArchitecture.Components.Movement
{
	public class InputMovementComponent : EntityComponent
	{
		private readonly float MOVE_SPEED = 100f;
		private readonly float DIAG_SPEED = (float)(100f * (Math.Sqrt(2f) / 2f));
		private float currentSpeed;

		private VelocityComponent velocity;

		public InputMovementComponent(VelocityComponent velocity)
		{
			this.velocity = velocity;
		}

		public override void Update(float deltaTime)
		{
			KeyboardState kb = Keyboard.GetState();
			bool isKeyDown(Keys key) => kb.IsKeyDown(key);

			int mx = 0;
			int my = 0;

			if(isKeyDown(Keys.W)) my--;
			if(isKeyDown(Keys.S)) my++;
			if(isKeyDown(Keys.D)) mx++;
			if(isKeyDown(Keys.A)) mx--;

			if(mx != 0 && my != 0)
				currentSpeed = DIAG_SPEED;
			else
				currentSpeed = MOVE_SPEED;

			velocity.X = mx * currentSpeed * deltaTime;
			velocity.Y = my * currentSpeed * deltaTime;
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}
	}
}
