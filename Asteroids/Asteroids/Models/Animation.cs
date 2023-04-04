using System;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids.Models
{
	public class Animation : ICloneable
	{
		public int CurrentFrame { get; set; }
		public int FrameCount { get; set; }
		
		public int FrameWidth { get; set; }
		public int FrameHeight { get; set; }
		
		public float FrameSpeed { get; set; }
		public bool IsLooping { get; set; }

		public Texture2D Texture { get; private set; }

		public Animation(Texture2D tex, int horizontalFrames, int verticalFrames)
		{
			int hFrames = Math.Max(1, horizontalFrames);
			int vFrames = Math.Max(1, verticalFrames);
			
			Texture = tex;
			CurrentFrame = 0;
			FrameCount = hFrames * vFrames;
			FrameSpeed = 0.2f;

			FrameWidth = Texture.Width / hFrames;
			FrameHeight = Texture.Height / vFrames;
		}

		public object Clone() => this.MemberwiseClone() as Animation ?? throw new Exception("Failed to return clone of class: 'Animation'");
	}
}