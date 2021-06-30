using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
    public class MurderRound : BaseRound
    {
        public override string RoundName => "MURDER";
        public override int RoundDuration => 300;
        public override bool CanPlayerSuicide => true;

        public List<Player> Spectators = new();

        private string _murderer;
        private string _firstDeath;
        private bool _isGameOver;
        private int _murdererKills;

        public override void OnPlayerKilled(Player player)
        {
            Players.Remove(player);
            Spectators.Add(player);

            player.MakeSpectator(player.EyePos);

            if (player.Team is MurdererTeam)
			{
				_ = LoadStatsRound("The Bystanders Won!");

				return;
			}
			else
			{
				if (string.IsNullOrEmpty(_firstDeath))
				{
					var client = player.GetClientOwner();

					_firstDeath = client.Name;
				}

				_murdererKills++;
			}

			if (Players.Count <= 1)
			{
				_ = LoadStatsRound("The Murderer Won!");
			}
        }

        public override void OnPlayerLeave(Player player)
        {
            base.OnPlayerLeave(player);
            
            Spectators.Remove(player);

            if (player.Team is MurdererTeam)
            {
                _ = LoadStatsRound("The Murderer Disconnected");
            }
        }

        // TODO: Make sure the Player spawns at one of the spawn points.
        public override void OnPlayerSpawn(Player player)
		{
			// player.MakeSpectator();

			// Spectators.Add(player);
			// Players.Remove(player);

			// base.OnPlayerSpawn(player);

			if (Players.Contains(player)) return;

			AddPlayer(player);

			if (_roundStarted)
			{
				player.Team = Game.Instance.IrisTeam;
				player.Team.OnStart(player);

				if (player.Team.HasDeployments)
					OpenDeploymentCmd(To.Single(player), player.TeamIndex);
			}

			base.OnPlayerSpawn(player);
		}

        protected override void OnStart()
		{
			Log.Info("Started Murder Round");

			if (Host.IsServer)
			{
				var murderer = Players[Rand.Int(Players.Count - 1)];
                var bystanderWithGun = Players[Rand.Int(Players.Count - 1)];

                while (bystanderWithGun == murderer) {
                    bystanderWithGun = Players[Rand.Int(Players.Count - 1)];
                }

                bystanderWithGun.hasGun = true;

                Assert.NotNull(murderer);

				murderer.Team = Game.Instance.MurdererTeam;
				murderer.Team.OnStart(murderer);

				Players.ForEach((player) =>
				{
					if (player != murderer)
					{
						player.Team = Game.Instance.BystanderTeam;
						player.Team.OnStart(player);
					}

					if (player.Team.HasDeployments)
						OpenDeploymentCmd(To.Single(player), player.TeamIndex);
				});

				foreach (var client in Client.All)
				{
					if (client.Pawn is Player player)
						SupplyLoadouts(player);
				}

                _roundStarted = true;
			}
		}

		protected override void OnTick()
		{
			// Check every tick of the round to see if any Bystanders have acquired all there clues.
			Players.ForEach((player) =>
			{
				if (player != murderer || player.hasGun != true && player.clues.size() == 4)
				{ 
					player.ClearAmmo();
					player.Inventory.DeleteContents();
					player.Inventory.Add(new Knife(), true);
				}
			});
		}

        protected override void OnFinish()
		{
			Log.Info("Finished Murder Round");

			if (Host.IsServer)
			{
				Spectators.Clear();
			}
		}

        protected override void OnTimeUp()
		{
			if (_isGameOver) return;

			Log.Info("Murder Time Up!");

			_ = LoadStatsRound("The Murderer Survived the Bystanders.");

			base.OnTimeUp();
		}

        private void SupplyLoadouts(Player player)
		{
			// Give everyone who is alive their starting loadouts.
			if (player.Team != null && player.LifeState == LifeState.Alive)
			{
				player.Team.SupplyLoadout(player);
				AddPlayer(player);
			}
		}

        private async Task LoadStatsRound(string winner, int delay = 3)
		{
			_isGameOver = true;

			await Task.Delay(delay * 1000);

			if (Game.Instance.Round != this)
				return;

			var murderer = Game.Instance.GetTeamPlayers<MurdererTeam>().FirstOrDefault();
			var murderName = hidden != null ? murderer.GetClientOwner().Name : "";

			Game.Instance.ChangeRound(new StatsRound
			{
				MurdererName = murderName,
				MurdererKills = _murdererKills,
				FirstDeath = _firstDeath,
				Winner = winner
			});
		}
    }   
}