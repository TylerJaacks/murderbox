using Sandbox;

namespace MurderboxGamemode
{
    [Library("murderbox", Title = "Murderbox")]
    partial class Game : Sandbox.Game
    {
        public MurdererTeam MurdererTeam { get; set; }
        public BystandersTeam BystandersTeam { get; set; }
        public Hud Hud { get; set; }

        public static Game Instance
        {
            get => Current as Game;
        }

        [Net] public BaseRound Round { get; set; }

        private BaseRound _lastRound;
        private List<BaseTeam> _teams;

        [ServerVar("mdrbox_min_players", Help = "The minimum players required to start.")]
		public static int MinPlayers { get; set; } = 4;

        public Game()
        {
            _teams = new List<BaseTeam>();

            if (IsServer)
            {
                Hud = new Hud();
            }

            MurdererTeam = new MurdererTeam();
            BystandersTeam = new BystandersTeam();

            AddTeam(MurdererTeam);
            AddTeam(BystandersTeam);

            _ = StartTickTimer();
        }

        public void AddTeam(BaseTeam team)
        {
            _teams.Add(team);
            team.Index = _teams.Count;
        }

        public List<Player> GetTeamPlayers<T>(bool isAlive = false) where T : BaseTeam
        {
            List<Player> output = new List<Player>();

            foreach (var client in Client.All)
            {
                if (client.Pawn is Player && player.Team is T)
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

        public override void DoPlayerNoclip(Client client)
        {

        }

        public override void DoPlayerSuicide(Client client)
        {
            if (client.Pawn.LifeState == LifeState.Alive && Round?.CanPlayerSuicide == true)
            {
                client.Pawn.LifeState = LifeState.Dead;
                client.Pawn.OnKilled();
                OnKilled(client.Pawn);
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

			for (var i = 0; i < _teams.Count; i++)
			{
				_teams[i].OnTick();
			}

			if (IsClient)
			{
				// We have to hack around this for now until we can detect changes in net variables.
				if (_lastRound != Round)
				{
					_lastRound?.Finish();
					_lastRound = Round;
					_lastRound.Start();
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
				if (Round is LobbyRound || Round == null)
				{
					ChangeRound(new MurderRound());
				}
			}
			else if (Round is not LobbyRound)
			{
				ChangeRound(new LobbyRound());
			}
		}
    }    
}