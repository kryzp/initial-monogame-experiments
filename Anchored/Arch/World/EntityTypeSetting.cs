using System;

namespace Arch.World
{
	public class EntityTypeSetting : Attribute
	{
		private string name;
		public string Name => name;

		public EntityTypeSetting(string name)
		{
			this.name = name;
		}
	}
}
