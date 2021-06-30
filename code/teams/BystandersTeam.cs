using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sandbox;

namespace MurderboxGamemode
{
    class BystandersTeam : BaseTeam
    {
        public override string HudClassName => "team_bystanders";
		public override string Name => "Bystanders";

		private Battery _batteryHud;
		private Radar _radarHud;

		public override void SupplyLoadout(Player player)
		{
			player.ClearAmmo();
			player.Inventory.DeleteContents();

            if (player._hasPistol) player.Inventory.Add(new Pistol(), true);
		}

        // TODO: Give Random Name to the Player.
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

			player.Controller = new BystandersTeam();
			player.Camera = new FirstPersonCamera();
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

		public override void OnPlayerKilled(Player player)
		{
			player.GlowActive = false;
		}

		public override void OnLeave(Player player)
		{
			var client = player.GetClientOwner();

			base.OnLeave(player);
		}
    }
}