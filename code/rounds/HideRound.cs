using System.Collections.Generic;

using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

public partial class HideRound : BaseRound
{
	[ServerVar("mb_host_always_hidden", Help = "Make the host always the hidden.")]
	public static bool HostAlwaysHidden { get; set; } = false;

	public override string RoundName => "PREPARE";
	public override int RoundDuration => 20;

	private bool _roundStarted;

	public override void OnPlayerSpawn(Player player)
	{
		if (Players.Contains(player)) return;

		AddPlayer(player);

		if (_roundStarted)
		{
			player.Team = Game.Instance.BystandersTeam;
			player.Team.OnStart(player);
		}

		base.OnPlayerSpawn(player);
	}

	protected override void OnStart()
	{
		Log.Info("Started Hide Round");

		if (Host.IsServer)
		{
			foreach (var client in Client.All)
			{
				if (client.Pawn is Player player)
					player.Respawn();
			}

			if (Players.Count == 0) return;

			// Select a random Hidden player.
			var murderer = Players[Rand.Int(Players.Count - 1)];

			if (HostAlwaysHidden)
			{
				murderer = Players[0];
			}

			Log.Info( murderer.Client.Name  + " is the murderer." );

			IList<Player> playersRemaining = new List<Player>();

			Players.ForEach((player) =>
			{
				if ( player != murderer ) playersRemaining.Add(player);

			});

			var detective = playersRemaining[Rand.Int( playersRemaining.Count - 1 )];

			Log.Info( detective.Client.Name + " is the detective." );

			Assert.NotNull(murderer);

			murderer.Team = Game.Instance.MurdererTeam;
			murderer.Team.OnStart(murderer);

			detective.Team = Game.Instance.DetectiveTeam;
			detective.Team.OnStart(detective);

			
			Players.ForEach((player) =>
			{
				if (player != murderer && player != detective)
				{
					player.Team = Game.Instance.BystandersTeam;
					player.Team.OnStart(player);
				}
			});

			_roundStarted = true;
		}
	}

	protected override void OnFinish()
	{
		Log.Info("Finished Hide Round");
	}

	protected override void OnTimeUp()
	{
		Log.Info("Hide Time Up!");

		Game.Instance.ChangeRound(new HuntRound());

		base.OnTimeUp();
	}
}
