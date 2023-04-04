using Microsoft.Xna.Framework;
using DarkWreath.Actors;

namespace DarkWreath
{
	public class FollowDriver : CameraDriver
	{
		Actor Follow = null;

		public FollowDriver(Actor act)
			: base()
		{
			Follow = act;
		}

		public override void Init(Camera camera)
		{
		}

		public override void Destroy(Camera camera)
		{
		}

		public override void Drive(Camera camera)
		{
			camera.Transform.Position = Vector2.Lerp(
				camera.Transform.Position,
				Follow.Transform.Position,
				0.075f
			);
		}
	}
}
