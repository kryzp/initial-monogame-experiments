namespace ComponentArchitecture
{
	public class Component
	{
		public Entity Owner;

		public virtual void Init() { }
		public virtual void Update() { }
		public virtual void Draw() { }
	}
}
