using System.Collections.Generic;
using System.Linq;

using Sandbox.UI;
using Sandbox.UI.Construct;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

public class InventoryColumn : Panel
{
	public int Column;
	public Label Header;

	internal List<InventoryIcon> Icons = new();

	public InventoryColumn(int i, Panel parent)
	{
		Parent = parent;
		Column = i;
		Header = Add.Label($"{i + 1}", "slot-number");
	}

	internal void UpdateWeapon(Weapon weapon)
	{
		var icon = ChildrenOfType<InventoryIcon>().FirstOrDefault(x => x.Weapon == weapon);

		if (icon == null)
		{
			icon = new InventoryIcon(weapon)
			{
				Parent = this
			};

			Icons.Add(icon);
		}
	}

	internal void TickSelection(Weapon selectedWeapon)
	{
		SetClass("active", selectedWeapon?.Bucket == Column);

		foreach (var icon in Icons)
		{
			icon.TickSelection(selectedWeapon);
		}
	}
}
