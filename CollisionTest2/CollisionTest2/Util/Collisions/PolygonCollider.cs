using System.Collections.Generic;
using CollisionTest2.Entities.Sprites;
using Microsoft.Xna.Framework;

namespace ARPG.Util.Collisions
{
    public class PolygonCollider
    {
        public List<Vector2> Points;
        public List<Vector2> Original;

        public Sprite Parent;
        
        public Vector2 Position;
        public float Rotation; // TODO: Implement Rotation

        public bool Overlap;
    }
}