using System;
using Microsoft.Xna.Framework;

namespace ARPG.Util.Collisions
{
    public class CollisionManager
    {
        /*
         * I didn't make this code, I only have 2 braincells (One for breathing, one for eating), it was all made
         * by this amazing person, link to their github profile and youtube account:
         * https://github.com/OneLoneCoder/olcPixelGameEngine/blob/master/Videos/OneLoneCoder_PGE_PolygonCollisions1.cpp
         * https://www.youtube.com/channel/UC-yuWVUplUJZvieEligKBkA
         *
         * And the code:
         * https://github.com/OneLoneCoder/olcPixelGameEngine/blob/master/Videos/OneLoneCoder_PGE_PolygonCollisions1.cpp
         * 
         * While the original code was made in C++ it wasn't hard to translate into C# and it works wonderfully.
         */
        
        public static bool ShapeOverlap_SAT(PolygonCollider r1, PolygonCollider r2)
        {
            PolygonCollider poly1 = r1;
            PolygonCollider poly2 = r2;

            for(int shape = 0; shape < 2; shape++)
            {
                if(shape == 1)
                {
                    poly1 = r2;
                    poly2 = r1;
                }

                for(int a = 0; a < poly1.Points.Count; a++)
                {
                    int b = (a + 1) % poly1.Points.Count;
                    Vector2 axisProj = new Vector2(-(poly1.Points[b].Y - poly1.Points[a].Y), poly1.Points[b].X - poly1.Points[a].X);

                    float min_r1 = Single.PositiveInfinity;
                    float max_r1 = Single.NegativeInfinity;

                    for(int p = 0; p < poly1.Points.Count; p++)
                    {
                        float q = (poly1.Points[p].X * axisProj.X + poly1.Points[p].Y * axisProj.Y);
                        min_r1 = Math.Min(min_r1, q);
                        max_r1 = Math.Max(max_r1, q);
                    }
                    
                    float min_r2 = Single.PositiveInfinity;
                    float max_r2 = Single.NegativeInfinity;

                    for(int p = 0; p < poly2.Points.Count; p++)
                    {
                        float q = (poly2.Points[p].X * axisProj.X + poly2.Points[p].Y * axisProj.Y);
                        min_r2 = Math.Min(min_r2, q);
                        max_r2 = Math.Max(max_r2, q);
                    }

                    if(!(max_r2 >= min_r1 && max_r1 >= min_r2))
                        return false;
                }
            }

            return true;
        }
        
        public static bool ShapeOverlap_SAT_STATIC(PolygonCollider r1, PolygonCollider r2)
        {
            PolygonCollider poly1 = r1;
            PolygonCollider poly2 = r2;

            float overlap = Single.PositiveInfinity;

            for(int shape = 0; shape < 2; shape++)
            {
                if(shape == 1)
                {
                    poly1 = r2;
                    poly2 = r1;
                }

                for(int a = 0; a < poly1.Points.Count; a++)
                {
                    int b = (a + 1) % poly1.Points.Count;
                    Vector2 axisProj = new Vector2(-(poly1.Points[b].Y - poly1.Points[a].Y), poly1.Points[b].X - poly1.Points[a].X);

                    float min_r1 = Single.PositiveInfinity;
                    float max_r1 = Single.NegativeInfinity;

                    for(int p = 0; p < poly1.Points.Count; p++)
                    {
                        float q = (poly1.Points[p].X * axisProj.X + poly1.Points[p].Y * axisProj.Y);
                        min_r1 = Math.Min(min_r1, q);
                        max_r1 = Math.Max(max_r1, q);
                    }
                    
                    float min_r2 = Single.PositiveInfinity;
                    float max_r2 = Single.NegativeInfinity;

                    for(int p = 0; p < poly2.Points.Count; p++)
                    {
                        float q = (poly2.Points[p].X * axisProj.X + poly2.Points[p].Y * axisProj.Y);
                        min_r2 = Math.Min(min_r2, q);
                        max_r2 = Math.Max(max_r2, q);
                    }

                    overlap = Math.Min(Math.Min(max_r1, max_r2) - Math.Max(min_r1, min_r2), overlap);
                    
                    if(!(max_r2 >= min_r1 && max_r1 >= min_r2))
                        return false;
                }
            }
            
            Vector2 d = new Vector2(r2.Parent.Position.X - r1.Position.X, r2.Position.Y - r1.Position.Y);
            float s = (float)Math.Sqrt(d.X * d.X + d.Y * d.Y);
            

            var pos = r1.Parent.Position;
            pos.X -= overlap * d.X / s;
            pos.Y -= (overlap * d.Y / s);
            r1.Parent.Position = pos;

            return false;
        }
    }
}