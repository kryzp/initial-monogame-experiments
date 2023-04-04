using Microsoft.Xna.Framework;
using Spellpath.Actors;

namespace Spellpath
{
    public class FollowDriver : CameraDriver
    {
        Actor Follow = null;

        public FollowDriver(Actor act)
            : base()
        {
            Follow = act;
        }

        public override void Init()
        {
        }

        public override void Destroy()
        {
        }

        public override void Update(GameTime time)
        {
            Camera.Position = Vector2.Lerp(
                Camera.Position,
                Follow.Transform.Position,
                0.075f
            );
        }
    }
}
