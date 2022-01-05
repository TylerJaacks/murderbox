using System.Threading.Tasks;

using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

public class LobbyRound : BaseRound
{
	public override string RoundName => "LOBBY";

	protected override void OnStart()
	{
		Log.Info("Started Lobby Round");

		if (Host.IsServer)
		{
			foreach (var client in Client.All)
			{
				if (client.Pawn is Player player)
					player.Respawn();
			}
		}
	}

	protected override void OnFinish()
	{
		Log.Info("Finished Lobby Round");
	}

	public override void OnPlayerKilled(Player player)
	{
		_ = StartRespawnTimer(player);

		base.OnPlayerKilled(player);
	}

	private async Task StartRespawnTimer(Player player)
	{
		await Task.Delay(1000);

		player.Respawn();
	}

	// TODO: Maybe Spawn Players With Hands.
	public override void OnPlayerSpawn(Player player)
	{
		if (Players.Contains(player))
		{
			player.Team.SupplyLoadout(player);
			return;
		}

		AddPlayer(player);

		player.Team = Game.Instance.BystandersTeam;
		player.Team.OnStart(player);
		player.Team.SupplyLoadout(player);

		base.OnPlayerSpawn(player);
	}
}
