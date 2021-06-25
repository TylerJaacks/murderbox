using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
	public partial class StatsRound : BaseRound
	{
		public override string RoundName => "STATS";
		public override int RoundDuration => 10;

		[Net] public string MurdererName { get; set; }
		[Net] public int MurdererKills { get; set; }
		[Net] public string FirstDeath { get; set; }
		[Net] public string Winner { get; set; }

		private Stats _statsPanel;

		protected override void OnStart()
		{
			Log.Info("Started Stats Round");

			if (Host.IsClient)
			{
				_statsPanel = Local.Hud.AddChild<Stats>();

				_statsPanel.Winner.Text = Winner;

				_statsPanel.AddStat(new StatInfo
				{
					Title = "Murderer Kills",
					PlayerName = MurdererName,
					ImageClass = "kills",
					TeamClass = "team_murderer",
					Text = MurdererKills.ToString()
				});

				_statsPanel.AddStat(new StatInfo
				{
					Title = "First Death",
					PlayerName = !string.IsNullOrEmpty(FirstDeath) ? FirstDeath : "N/A",
					ImageClass = "first_death",
					TeamClass = "team_bystanders",
					Text = ""
				});
			}
		}

		protected override void OnFinish()
		{
			Log.Info("Finished Stats Round");

			if (_statsPanel != null)
			{
				_statsPanel.Delete();
			}
		}

		protected override void OnTimeUp()
		{
			Log.Info("Stats Time Up!");

			Game.Instance.ChangeRound(new HideRound());

			base.OnTimeUp();
		}

		public override void OnPlayerSpawn(Player player)
		{
			if (Players.Contains(player)) return;

			player.MakeSpectator();

			AddPlayer(player);

			base.OnPlayerSpawn(player);
		}
	}
}
