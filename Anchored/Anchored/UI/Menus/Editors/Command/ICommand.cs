using Anchored.Assets.Maps;
using System;
using System.Collections.Generic;
using System.Text;

namespace Anchored.UI.Menus.Editors.Command
{
	public interface ICommand
	{
		void Do(AnchoredMap map);
		void Undo(AnchoredMap map);
	}
}
