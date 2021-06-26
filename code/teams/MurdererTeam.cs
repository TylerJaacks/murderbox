using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
    class MurdererTeam : BaseTeam
	{
		public override bool HideNameplate => false;
		public override string HudClassName => "team_murderer";
		public override string Name => "Murderer";
		public Player CurrentPlayer { get; set; }

		public override void SupplyLoadout(Player player)
		{
			player.ClearAmmo();
			player.Inventory.DeleteContents();
			player.Inventory.Add(new Knife(), true);
		}

		public override void OnStart(Player player)
		{
			player.ClearAmmo();
			player.Inventory.DeleteContents();

			if (Host.IsServer)
			{
				player.RemoveClothing();
			}

			player.SetModel("models/citizen/citizen.vmdl");

			player.EnableAllCollisions = true;
			player.EnableDrawing = true;
			player.EnableHideInFirstPerson = true;
			player.EnableShadowInFirstPerson = true;

			player.Controller = new MurdererController();
			player.Camera = new FirstPersonCamera();
		}

		public override void OnTakeDamageFromPlayer(Player player, Player attacker, DamageInfo info)
		{
			info.Damage *= 1.5f;
		}

		public override void OnDealDamageToPlayer(Player player, Player target, DamageInfo info)
		{
            info.Damage *= 1.25f;
		}

		public override void OnTick()
		{
			if (Host.IsClient)
			{
				/*
				if (Local.Pawn is not Player localPlayer)
					return;

				if (localPlayer.Team == this)
					return;

				var hidden = Game.Instance.GetTeamPlayers<HiddenTeam>(true).FirstOrDefault();

				if (hidden != null && hidden.IsValid())
				{
					var distance = localPlayer.Pos.Distance(hidden.Pos);
					hidden.RenderAlpha = 0.2f - ((0.2f / 1500f) * distance);
				}
				*/
			}
			else
			{
				var player = CurrentPlayer;

				if (player != null && player.IsValid())
				{
					var overlaps = Physics.GetEntitiesInSphere(player.Position, 2048f);
				}
			}
		}

		public override void OnTick(Player player)
		{
            // TODO: Do Footstep shit here.
		}

		public override bool PlayPainSounds(Player player)
		{
			player.PlaySound("grunt" + Rand.Int(1, 4));

			return true;
		}

		public override void OnJoin(Player player)
		{
			var client = player.GetClientOwner();

			player.EnableShadowCasting = false;
			player.EnableShadowReceive = false;
			player.RenderAlpha = 0.15f;

			CurrentPlayer = player;

			base.OnJoin(player);
		}

		public override void OnLeave(Player player)
		{
			var client = player.GetClientOwner();

			player.EnableShadowReceive = true;
			player.EnableShadowCasting = true;
			player.RenderAlpha = 1f;

			CurrentPlayer = null;

			base.OnLeave(player);
		}
	}
}