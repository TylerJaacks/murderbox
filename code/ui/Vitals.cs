using Sandbox;
using Sandbox.UI;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode
{
	public class Vitals : Panel
	{
		public Health Health;

		public Vitals()
		{
			Health = AddChild<Health>();
		}

		public override void Tick()
		{
			if ( Local.Pawn is not Player player ) return;

			SetClass( "murderer", player.LifeState != LifeState.Alive );

			base.Tick();
		}
	}
}
