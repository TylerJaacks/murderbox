using Sandbox;
using Sandbox.UI;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

[Library]
public partial class Hud : HudEntity<RootPanel>
{
	public Hud()
	{
		if (!IsClient)
			return;

		RootPanel.StyleSheet.Load("/ui/Hud.scss");

		RootPanel.AddChild<RoundInfo>();
		RootPanel.AddChild<Ammo>();
		RootPanel.AddChild<VoiceList>();
		RootPanel.AddChild<Nameplates>();
		RootPanel.AddChild<DamageIndicator>();
		RootPanel.AddChild<HitIndicator>();
		RootPanel.AddChild<InventoryBar>();
		RootPanel.AddChild<ChatBox>();
		RootPanel.AddChild<Scoreboard>();
		RootPanel.AddChild<LoadingScreen>();
		RootPanel.AddChild<Vitals>();
	}
}
