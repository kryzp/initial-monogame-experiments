using System;

namespace ComponentArchitecture
{
	public static class ComponentTools
	{
		private static int prevID = 0;
		private static int typeID;

		public static int GetComponentTypeID()
		{
			return prevID++;
		}

		public static int GetComponentTypeID<T>()
		{
			typeID = GetComponentTypeID();
			Console.WriteLine(typeID);
			return typeID;
		}
	}
}
