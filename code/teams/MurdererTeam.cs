using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

class MurdererTeam : BaseTeam
{
	public override bool HideNameplate => true;
	public override string HudClassName => "team_murderer";
	public override string Name => "Murderer";
	public Player CurrentPlayer { get; set; }

	// TODO: Give the Murderer fists.
	public override void SupplyLoadout(Player player)
	{
		player.ClearAmmo();

		player.Inventory.DeleteContents();

		player.Inventory.Add( new Knife(), false );
		player.Inventory.Add( new Fists(), true );
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

		player.Controller = new HiddenController();
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

			var hidden = Game.Instance.GetTeamPlayers<MurdererTeam>(true).FirstOrDefault();

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

	public override bool PlayPainSounds(Player player)
	{
		// TODO: Change the sound.
		player.PlaySound("hidden_grunt" + Rand.Int(1, 2));

		return true;
	}

	public override void OnJoin(Player player )
	{
		Log.Info($"{ player.Client.Name } joined the Murderer team.");

		player.EnableShadowCasting = false;
		player.EnableShadowReceive = false;
		player.RenderColor = player.RenderColor.WithAlpha(0.15f);

		CurrentPlayer = player;

		base.OnJoin(player);
	}

	public override void OnLeave(Player player)
	{
		player.EnableShadowReceive = true;
		player.EnableShadowCasting = true;
		player.RenderColor = player.RenderColor.WithAlpha(1f);

		Log.Info($"{ player.Client.Name } left the Murderer team.");

		CurrentPlayer = null;

		base.OnLeave(player);
	}
}
