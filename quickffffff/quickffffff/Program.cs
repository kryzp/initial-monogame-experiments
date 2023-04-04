using System;

/*
 * Wrote it in python but it ran like crap :(
 * (hence the odd function naming since i literally just ripped it from my python code and shoved it into c#)
 */

namespace Solver
{
	public class Program
	{
		public static List<string> valves = new List<string>();
		public static List<(string, string)> tunnels = new List<(string, string)>();
		public static Dictionary<string, int> flow_rates = new Dictionary<string, int>();
		public static Dictionary<string, Dictionary<string, int>> connections = new Dictionary<string, Dictionary<string, int>>();
		
		public const int MAX_TIME = 30;

		public static void floyd_warsh_algorithm()
		{
			Dictionary<string, Dictionary<string, int>> dist = new Dictionary<string, Dictionary<string, int>>();
			foreach (var u in valves)
			{
				dist.Add(u, new Dictionary<string, int>());
				
				foreach (var v in valves)
				{
					dist[u].Add(v, valves.Count + 1);
				}
			}

			foreach (var uv in tunnels)
				dist[uv.Item1][uv.Item2] = 1;

			foreach (var u in valves)
				dist[u][u] = 0;
			
			foreach (var k in valves)
				foreach (var i in valves)
					foreach (var j in valves)
						if (dist[i][j] > dist[i][k] + dist[k][j])
							dist[i][j] = dist[i][k] + dist[k][j];

			connections = dist;
		}

		public static int calc_total_flow(List<string> valves)
		{
			int result = 0;
			foreach (var v in valves)
				result += flow_rates[v];
			return result;
		}

		public static int max_score(string curr, int t, int total, List<string> open_valves)
		{
			int maximum = total + (calc_total_flow(open_valves) - (MAX_TIME * t));

			foreach (var newValve in valves)
			{
				if (open_valves.Contains(newValve))
					continue;

				int dt = connections[curr][newValve] + 1;

				if (t + dt >= MAX_TIME)
					continue;

				int new_total = total + (dt * calc_total_flow(open_valves));
				open_valves.Add(newValve);
				int val = max_score(newValve, t + dt, new_total, open_valves);
				maximum = Math.Max(maximum, val);
				open_valves.Remove(newValve);
			}

			return maximum;
		}
		
		public static void Main(string[] args)
		{
			string[] ll = File.ReadAllLines("input.txt");

			foreach (var l in ll)
			{
				var split = l.Split(" ");
				for (int i = 0; i < split.Length; i++)
					split[i] = split[i].Trim();

				string fromV = split[1];
				int rate = Convert.ToInt32(split[4].Split("=")[1][..^1]);
				string[] toVs = String.Join("", split[9..]).Split(",");
				flow_rates.Add(fromV, rate);
				valves.Add(fromV);
				foreach (var toV in toVs)
					tunnels.Add((fromV, toV));
			}
			
			floyd_warsh_algorithm();
			Console.WriteLine(max_score("AA", 0, 0, new List<string>()));
		}
	}
}
