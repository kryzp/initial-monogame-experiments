using System;

namespace AutoAlgorithm
{
	public class AlgorithmManager
	{
		private string startValue;
		private string length;
		private string increment;
		private string incrementIncrement;

		private double current;

		private int seperatorLength = 79;

		public void Run()
		{
			Util.DrawSeperator(seperatorLength, '=', true);
			Console.WriteLine("Welcome to AutoAlgorithm :: Krystof Przeczek");
			Util.DrawSeperator(seperatorLength, '=', true);
			
			Console.Write("Starting Value >> ");
			startValue = Console.ReadLine();
			
			Console.Write("Length >> ");
			length = Console.ReadLine();
			
			Console.Write("Increment >> ");
			increment = Console.ReadLine();
			
			Console.Write("Increment Increment >> ");
			incrementIncrement = Console.ReadLine();

			current = Double.Parse(startValue ?? throw new Exception("Start Value is Null!"));
			int len = Int32.Parse(length ?? throw new Exception("Length is Null!"));
			double inc = Double.Parse(increment ?? throw new Exception("Increment is Null!"));
			double incInc = Double.Parse(incrementIncrement ?? throw new Exception("Increment Increment is Null!"));
			
			Console.Clear();
			
			Util.DrawSeperator(seperatorLength, '=', true);
			Console.WriteLine("Output");
			Util.DrawSeperator(seperatorLength, '-', true);

			for(int ii = 0; ii < len; ii++)
			{
				Console.Write(current + ", ");
				current += inc;
				inc += incInc;
			}
			
			Console.WriteLine();
			Util.DrawSeperator(seperatorLength, '=', true);
			
			inc = Double.Parse(increment ?? throw new Exception("Increment is Null!"));

			Console.WriteLine("Increment");
			Util.DrawSeperator(seperatorLength, '-', true);
			
			for(int ii = 0; ii < len; ii++)
			{
				Console.Write(inc + ", ");
				inc += incInc;
			}
			
			Console.WriteLine();
			Util.DrawSeperator(seperatorLength, '=', true);

			Console.ReadLine();
		}
	}
}