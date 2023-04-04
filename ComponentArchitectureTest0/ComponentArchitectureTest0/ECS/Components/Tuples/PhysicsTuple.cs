using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComponentArchitectureTest0.ECS.Components.Transform;
using ComponentArchitectureTest0.ECS.Components.Physics;

namespace ComponentArchitectureTest0.ECS.Components.Tuples
{
	public class PhysicsTuple : Tuple
	{
		public PositionComponent Position { get; set; }
		public VelocityComponent Velocity { get; set; }
	}
}
