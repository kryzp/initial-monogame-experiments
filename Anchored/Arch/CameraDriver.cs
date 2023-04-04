using Anchored.Graphics;

namespace Arch
{
	public abstract class CameraDriver
	{
		public Camera Camera;

		public abstract void Init();
		public abstract void Destroy();
		public abstract void Update();
	}
}
