using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace DarkWreath.MousePattern
{
	public abstract class Pattern
	{
		protected int pCurrentSegmentIndex;
		protected Vector2 pCurrentSegmentStartingPosition;
		protected Vector2 pFirstSegmentStartingPosition;
		
		public List<PatternSegment> Segments { get; protected set; }

		public int CurrentSegmentIndex => pCurrentSegmentIndex;
		public Vector2 FirstReloadPosition => pFirstSegmentStartingPosition;
		public Vector2 SegmentReloadPosition => pCurrentSegmentStartingPosition;

		public PatternSegment CurrentSegment => Segments[pCurrentSegmentIndex];

		public Pattern()
		{
			pCurrentSegmentIndex = 0;
			pCurrentSegmentStartingPosition = Vector2.Zero;
			pFirstSegmentStartingPosition = Vector2.Zero;
			
			SetSegments();
		}

		public abstract void SetSegments();
	}
}
