using System.Collections.Generic;
using System.Threading.Tasks;

using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

[Library("murderbox", Title = "Murderbox")]
internal partial class Game : Sandbox.Game
{
	public MurdererTeam MurdererTeam { get; set; }
	public BystandersTeam BystandersTeam { get; set; }
	public DetectiveTeam DetectiveTeam { get; set; }
	public Hud Hud { get; set; }

	public static Game Instance
	{
		get => Current as Game;
	}

	[Net] public BaseRound Round { get; private set; }

	private BaseRound _lastRound;
	private readonly List<BaseTeam> _teams;

	[ServerVar("mb_min_players", Help = "The minimum players required to start.")]
	public static int MinPlayers { get; set; } = 3;


	[ServerVar("mb_voice_radius", Help = "How far away players can hear each other talk.")]
	public static int VoiceRadius { get; set; } = 2048;

	public Game()
	{
		_teams = new List<BaseTeam>();

		if (IsServer)
		{
			Hud = new Hud();
		}

		MurdererTeam = new MurdererTeam();
		BystandersTeam = new BystandersTeam();
		DetectiveTeam = new DetectiveTeam();

		AddTeam(MurdererTeam);
		AddTeam(BystandersTeam);
		AddTeam(DetectiveTeam);

		_ = StartTickTimer();
	}

	public void AddTeam(BaseTeam team)
	{
		_teams.Add(team);
		team.Index = _teams.Count;
	}

	public BaseTeam GetTeamByIndex(int index)
	{
		return _teams[index - 1];
	}

	public List<Player> GetTeamPlayers<T>(bool isAlive = false) where T : BaseTeam
	{
		var output = new List<Player>();

		foreach (var client in Client.All)
		{
			if (client.Pawn is Player {Team: T} player)
			{
				if (!isAlive || player.LifeState == LifeState.Alive)
				{
					output.Add(player);
				}
			}
		}

		return output;
	}

	public void ChangeRound(BaseRound round)
	{
		Assert.NotNull(round);

		Round?.Finish();
		Round = round;
		Round?.Start();
	}

	public async Task StartSecondTimer()
	{
		while (true)
		{
			await Task.DelaySeconds(1);
			OnSecond();
		}
	}

	public async Task StartTickTimer()
	{
		while (true)
		{
			await Task.NextPhysicsFrame();
			OnTick();
		}
	}

	public override void PostLevelLoaded()
	{
		_ = StartSecondTimer();

		base.PostLevelLoaded();
	}

	public override void OnKilled(Entity entity)
	{
		if (entity is Player player)
			Round?.OnPlayerKilled(player);

		base.OnKilled(entity);
	}

	public override void ClientDisconnect(Client client, NetworkDisconnectionReason reason)
	{
		Log.Info(client.Name + " left, checking minimum player count...");

		Round?.OnPlayerLeave(client.Pawn as Player);

		base.ClientDisconnect(client, reason);
	}

	public override void ClientJoined(Client client)
	{
		var pawn = new Player();
		client.Pawn = pawn;
		pawn.Respawn();

		base.ClientJoined(client);
	}

	private void OnSecond()
	{
		CheckMinimumPlayers();
		Round?.OnSecond();
	}

	private void OnTick()
	{
		Round?.OnTick();

		foreach (var team in _teams)
		{
			team.OnTick();
		}

		if (IsClient)
		{
			// We have to hack around this for now until we can detect changes in net variables.
			if (_lastRound != Round)
			{
				_lastRound?.Finish();
				_lastRound = Round;
				_lastRound?.Start();
			}

			foreach (var client in Client.All)
			{
				if (client.Pawn is not Player player) return;

				if (player.TeamIndex != player.LastTeamIndex)
				{
					player.Team = GetTeamByIndex(player.TeamIndex);
					player.LastTeamIndex = player.TeamIndex;
				}
			};
		}
	}

	private void CheckMinimumPlayers()
	{
		if (Client.All.Count >= MinPlayers)
		{
			if (Round is LobbyRound or null)
			{
				ChangeRound(new HideRound());
			}
		}
		else if (Round is not LobbyRound)
		{
			ChangeRound(new LobbyRound());
		}
	}
}
