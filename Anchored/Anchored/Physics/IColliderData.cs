﻿using Arch.Math;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Anchored.Util.Physics
{
	public interface IColliderData
	{
		void Project(Vector2 axis, ref float min, ref float max);
		void UpdateWorldBounds(Matrix mat, ref RectangleF worldBounds, ref List<Vector2> axis, ref List<Vector2> points);
	}
}
