using System;
using System.IO;
using System.Text;

namespace WiggleCreator
{
	class Program
	{
		static string NewLineString(string s)
		{
			return s + "\n";
		}

		static void Main(string[] args)
		{
			float toRadians = MathF.PI / 180f;

			int wiggleWidth = 20;
			int wiggleHeight = 30;
			int wiggleFreq = 12;
			string text = "@everyone";

			string wiggle = "";

			for (int yy = 0; yy < wiggleHeight; yy += 1)
			{
				string spaceAmount = "";
				int sin = (int)(((MathF.Cos(yy * toRadians * wiggleFreq) + MathF.Sin(yy * toRadians * wiggleFreq + 103f) + 1f) / 2f) * wiggleWidth);

				for (int xx = 0; xx < sin; xx += 1)
					spaceAmount += " ";

				wiggle += NewLineString(spaceAmount + text);
			}

			int charAmount = wiggle.Length;
			wiggle += NewLineString($"Char Amount: {charAmount}");

			using (FileStream fs = File.Create("D:\\Projects\\C#\\WiggleCreator\\WiggleCreator\\output.txt"))
			{
				byte[] info = new UTF8Encoding(true).GetBytes(wiggle);
				fs.Write(info, 0, info.Length);
			}
		}
	}
}
