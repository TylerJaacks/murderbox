using Sandbox.UI.Construct;
using Sandbox.UI;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

// TODO: This is going to need to be looked at.
public class Nameplate : Panel
{
	public Label NameLabel;

	public Nameplate(Player player)
	{
		NameLabel = Add.Label(player.Client.Name);
	}
}
