using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SweptAABB
{
	public static class CollisionManager
	{
		public static void HandleCollisions(Box box1, Box box2)
		{
			float normalx;
			float normaly;
			float collisiontime = Instersects_AABB_STATIC(box1, box2, out normalx, out normaly);
			box1.Position.X += box1.Velocity.X * collisiontime;
			box1.Position.Y += box1.Velocity.Y * collisiontime;

			float remainingtime = 1.0f - collisiontime;

			float dotprod = (box1.Velocity.X * normaly + box1.Velocity.Y * normalx) * remainingtime;
			box1.Velocity.X = dotprod * normalx;
			box1.Velocity.Y = dotprod * normaly;
		}

		public static bool Intersects_AABB(Box box1, Box box2)
		{
			var b1 = box1.Rectangle;
			var b2 = box2.Rectangle;

			return !(
				b1.X + b1.Width < b2.X ||
				b1.X > b2.X + b2.Width ||
				b1.Y + b1.Height < b2.Y ||
				b1.Y > b2.Y + b2.Height
			);
		}

		public static float Instersects_AABB_STATIC(Box box1, Box box2, out float normalx, out float normaly)
		{
			float xInvEntry, yInvEntry;
			float xInvExit, yInvExit;

			var b1 = box1.Rectangle;
			var b2 = box2.Rectangle;

			if(box1.Velocity.X > 0.0f)
			{
				xInvEntry = b2.X - (b1.X + b1.Width);
				xInvExit = (b2.X + b2.Width) - b1.X;
			}
			else
			{
				xInvEntry = (b2.X + b2.Width) - b1.X;
				xInvExit = b2.X - (b1.X + b1.Width);
			}

			if(box1.Velocity.Y > 0.0f)
			{
				yInvEntry = b2.Y - (b1.Y + b1.Height);
				yInvExit = (b2.Y + b2.Height) - b1.Y;
			}
			else
			{
				yInvEntry = (b2.Y + b2.Height) - b1.Y;
				yInvExit = b2.Y - (b1.Y + b1.Height);
			}

			float xEntry, yEntry;
			float xExit, yExit;

			if(box1.Velocity.X == 0.0f)
			{
				xEntry = Single.NegativeInfinity;
				xExit = Single.PositiveInfinity;
			}
			else
			{
				xEntry = xInvEntry / box1.Velocity.X;
				xExit = xInvExit / box1.Velocity.X;
			}

			if(box1.Velocity.Y == 0.0f)
			{
				yEntry = Single.NegativeInfinity;
				yExit = Single.PositiveInfinity;
			}
			else
			{
				yEntry = yInvEntry / box1.Velocity.Y;
				yExit = yInvExit / box1.Velocity.Y;
			}

			float entryTime = Math.Max(xEntry, yEntry);
			float exitTime = Math.Min(xExit, yExit);

			if(entryTime > exitTime || xEntry < 0.0f && yEntry < 0.0f || xEntry > 1.0f || yEntry > 1.0f)
			{
				normalx = 0.0f;
				normaly = 0.0f;

				return 1.0f;
			}
			else
			{
				if(xEntry > yEntry)
				{
					if(xInvEntry < 0.0f)
					{
						normalx = 1.0f;
						normaly = 0.0f;
					}
					else
					{
						normalx = -1.0f;
						normaly = 0.0f;
					}
				}
				else
				{
					if(yInvEntry < 0.0f)
					{
						normalx = 0.0f;
						normaly = 1.0f;
					}
					else
					{
						normalx = 0.0f;
						normaly = -1.0f;
					}
				}

				return entryTime;
			}
		}
	}
}
