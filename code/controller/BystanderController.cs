using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Needs to be looked at.
namespace MurderboxGamemode
{
	public class BystanderController : CustomWalkController
	{
		public float FallDamageVelocity = 550f;
		public float FallDamageScale = 0.25f;
		public float MaxSprintSpeed = 300f;
		public float MaxDefaultSpeed = 190f;
		public float MaxWalkSpeed = 150f;
		public float StaminaLossPerSecond = 15f;
		public float StaminaGainPerSecond = 20f;

		private float _fallVelocity;

		public override void Simulate()
		{
			if ( Pawn is Player player )
			{
				var staminaLossPerSecond = StaminaLossPerSecond;

				if ( player.Deployment == DeploymentType.IRIS_BRAWLER )
				{
					staminaLossPerSecond *= 1.3f;

					MaxSprintSpeed = 250f;
					MaxDefaultSpeed = 170f;
					MaxWalkSpeed = 120f;
				}

				if ( Input.Down( InputButton.Run ) && Velocity.Length >= (SprintSpeed * 0.8f) )
				{
					player.Stamina = MathF.Max( player.Stamina - (staminaLossPerSecond * Time.Delta), 0f );
				}
				else
				{
					player.Stamina = MathF.Min( player.Stamina + (StaminaGainPerSecond * Time.Delta), 100f );
				}

				SprintSpeed = WalkSpeed + (((MaxSprintSpeed - WalkSpeed) / 100f) * player.Stamina) + 40f;
			}

			base.Simulate();
		}

		public override void OnPreTickMove()
		{
			_fallVelocity = Velocity.z;
		}

		public override void OnPostCategorizePosition( bool stayOnGround, TraceResult trace )
		{
			if ( Host.IsServer && trace.Hit && _fallVelocity < -FallDamageVelocity )
			{
				var damage = (MathF.Abs( _fallVelocity ) - FallDamageVelocity) * FallDamageScale;

				using ( Prediction.Off() )
				{
					var damageInfo = new DamageInfo()
						.WithAttacker( Pawn )
						.WithFlag( DamageFlags.Fall );

					damageInfo.Damage = damage;

					Pawn.TakeDamage( damageInfo );
				}
			}
		}
	}
}
