using System;

using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

public class BystanderController : CustomWalkController
{
	public float FallDamageVelocity = 550f;
	public float FallDamageScale = 0.25f;
	public float MaxSprintSpeed = 300f;

	private float _fallVelocity;

	public override void Simulate()
	{
		if (Pawn is Player)
		{
			SprintSpeed = WalkSpeed + (((MaxSprintSpeed - WalkSpeed) / 100f)) + 40f;
		}

		base.Simulate();
	}

	public override void OnPreTickMove()
	{
		_fallVelocity = Velocity.z;
	}

	public override void OnPostCategorizePosition(bool stayOnGround, TraceResult trace)
	{
		if (Host.IsServer && trace.Hit && _fallVelocity < -FallDamageVelocity)
		{
			var damage = (MathF.Abs(_fallVelocity) - FallDamageVelocity) * FallDamageScale;

			using (Prediction.Off())
			{
				var damageInfo = new DamageInfo()
					.WithAttacker(Pawn)
					.WithFlag(DamageFlags.Fall);

				damageInfo.Damage = damage;

				Pawn.TakeDamage(damageInfo);
			}
		}
	}
}
