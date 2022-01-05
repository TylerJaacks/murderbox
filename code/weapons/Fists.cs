using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

[Library( "weapon_fists", Title = "Fists", Spawnable = false )]
partial class Fists : Weapon
{
	public override string ViewModelPath => "models/firstperson/temp_punch/temp_punch.vmdl";
	public override float PrimaryRate => 2.0f;
	public override float SecondaryRate => 2.0f;

	public override bool CanReload()
	{
		return false;
	}

	private void Attack( bool leftHand )
	{

	}

	public override void AttackPrimary()
	{
		Attack( true );
	}

	public override void AttackSecondary()
	{
		Attack( false );
	}

	public override void OnCarryDrop( Entity dropper )
	{
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetParam( "holdtype", 5 );
		anim.SetParam( "aimat_weight", 1.0f );
	}
}
