using System.Collections.Generic;
using System.Linq;

using Sandbox;
using Sandbox.UI;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

[UseTemplate]
public class Scoreboard : Panel
{
	private readonly Dictionary<Client, ScoreboardEntry> Entries = new();
		
	public Panel PlayerSection { get; set; }

	public override void Tick()
	{
		base.Tick();

		SetClass("open", Input.Down(InputButton.Score));

		if (!IsVisible)
			return;

		foreach (var cl in Client.All.Except(Entries.Keys))
		{
			ScoreboardEntry entry = new();
			Entries.Add(cl, entry);
			entry.UpdateFrom(cl);

		}

		foreach (var cl in Entries.Keys.Except(Client.All))
		{
			if (Entries.TryGetValue(cl, out var entry))
			{
				entry.Delete();
				Entries.Remove(cl);
			}
		}

		var incorrectlyLocated = Entries.Where(kvp => kvp.Value.Parent != GetCorrectSection(kvp.Key)).ToList();

		foreach (var kvp in incorrectlyLocated)
			kvp.Value.Parent = GetCorrectSection(kvp.Key);
	}

	private Panel GetCorrectSection(Client client)
	{
		return PlayerSection;
	}
}
