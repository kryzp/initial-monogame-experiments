using System;
using Breakout.States;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout.Entities.Sprites.Bricks
{
    public class BallBrick : Brick
    {
        private const int BALL_AMOUNT = 2;

        public Ball BallPrefab { get; set; }
        
        public BallBrick(Texture2D tex)
            : base(tex)
        {
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StatePlaying.SpawnBalls = true;
        }
    }
}