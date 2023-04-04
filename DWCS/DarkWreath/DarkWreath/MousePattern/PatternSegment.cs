using System;
using DarkWreath.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DarkWreath.MousePattern
{
	public abstract class PatternSegment
	{
		public Vector2 MousePosition => InputManager.MouseScreenPosition();
		
		public Vector2 StartPosition { get; set; }
		
		public PatternSegment()
		{
		}

		public abstract Vector2 GetEndPosition();
		
		public abstract bool Update();

		public virtual void DebugDraw(SpriteBatch b)
		{
		}
		
		public virtual void Reset()
		{
		}

		public virtual float Progress()
		{
			throw new NotImplementedException();
		}
	}
}
