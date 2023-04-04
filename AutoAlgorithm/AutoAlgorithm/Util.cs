using System;

namespace AutoAlgorithm
{
	public static class Util
	{
		public static bool IsDigitsOnly(string str)
		{
			foreach(char c in str)
			{
				if (c < '0' || c > '9')
					return false;
			}

			return true;
		}
		
		public static void DrawSeperator(int length, char type, bool endline)
		{
			for(int ii = 0; ii <= length; ii++) Console.Write(type);
			if(endline) Console.WriteLine();
		}
	}
}