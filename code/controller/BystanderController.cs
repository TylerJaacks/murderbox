using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MurderboxGamemode
{
	public class BystanderController : CustomWalkController
	{
		public float FallDamageVelocity = 550f;
		public float FallDamageScale = 0.25f;
		public float MaxDefaultSpeed = 190f;
		public float MaxWalkSpeed = 150f;

		private float _fallVelocity;

		public override void Simulate()
		{
			if ( Pawn is Player player )
			{
				var staminaLossPerSecond = StaminaLossPerSecond;

				if ( player.Deployment == DeploymentType.IRIS_BRAWLER )
				{
					MaxDefaultSpeed = 170f;
					MaxWalkSpeed = 120f;
				}

				SprintSpeed = WalkSpeed;
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
