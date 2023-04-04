using Anchored.Assets.Maps;
using Arch.Assets.Maps;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anchored.UI.Menus.Editors.Command
{
	public class SetTileCommand : ICommand
	{
		public int X;
		public int Y;
		public ushort ID;
		public ushort PriorID;
		public Layer Layer;

		public void Do(AnchoredMap map)
		{
			Layer.Data[Y, X] = ID;
		}

		public void Undo(AnchoredMap map)
		{
			Layer.Data[Y, X] = PriorID;
		}
	}
}
