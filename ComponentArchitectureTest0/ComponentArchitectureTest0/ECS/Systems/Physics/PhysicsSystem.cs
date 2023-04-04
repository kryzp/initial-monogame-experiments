using ComponentArchitectureTest0.ECS.Components;
using ComponentArchitectureTest0.ECS.Components.Tuples;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ComponentArchitectureTest0.ECS.Systems.Physics
{
	public class PhysicsSystem : EntitySystem
	{
		public PhysicsSystem()
		{
		}

		public override void Update(float deltaTime)
		{
			// Suuuuper simple physics system that just increments
			// the velocity and position of all physics entities
			foreach (PhysicsTuple t in GetPhysicsTuples())
			{
				t.Position.X += t.Velocity.X;
				t.Position.Y += t.Velocity.Y;

				t.Velocity.X += t.Velocity.XA;
				t.Velocity.Y += t.Velocity.YA;

				Console.WriteLine("T Position: " + new Vector2(t.Position.X, t.Position.Y));
			}
		}

		public override void Draw(float deltaTime, SpriteBatch spriteBatch)
		{
		}

		private List<PhysicsTuple> GetPhysicsTuples()
		{
			List<PhysicsTuple> tuples = new List<PhysicsTuple>();



			return tuples;
		}
	}
}
