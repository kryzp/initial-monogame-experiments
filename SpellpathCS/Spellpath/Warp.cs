namespace Spellpath
{
	public class Warp
	{
		public string TargetArea;

		public int FromTileX;
		public int FromTileY;
		
		public int TargetTileX;
		public int TargetTileY;

		public Warp(string area, int fromX, int fromY, int tgtX, int tgtY)
		{
			TargetArea = area;
			FromTileX = fromX;
			FromTileY = fromY;
			TargetTileX = tgtX;
			TargetTileY = tgtY;
		}
	}
}