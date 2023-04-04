using Microsoft.Xna.Framework.Graphics;
using System;

namespace ComponentArchitectureTest0.ECS.Components
{
	public abstract class Component : IDisposable
	{
		public abstract void Create(params GraphicsResource[] resource);
		public abstract void Dispose();
	}
}
