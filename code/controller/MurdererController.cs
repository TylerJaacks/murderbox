using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO: Needs to be looked at.
namespace MurderboxGamemode
{
	public class MurdererController : CustomWalkController
	{
		public override float SprintSpeed { get; set; } = 380f;
		public bool IsFrozen { get; set; }
		public float LeapVelocity { get; set; } = 300f;
		public float LeapStaminaLoss { get; set; } = 40f;

		public override void AddJumpVelocity()
		{
			if (Pawn is Player player)
			{
				var minLeapVelocity = (LeapVelocity * 0.2f);
				var extraLeapVelocity = (LeapVelocity * 0.8f);
				var actualLeapVelocity = minLeapVelocity + (extraLeapVelocity / 100f);

				Velocity += (Pawn.EyeRot.Forward * actualLeapVelocity);
			}

			base.AddJumpVelocity();
		}

        // TODO: Look at this code.
		public override float GetWishSpeed()
		{
			var speed = base.GetWishSpeed();

			if (Pawn is Player player)
			{
				if (player.Deployment == DeploymentType.HIDDEN_BEAST)
					speed *= 0.7f;
				else if (player.Deployment == DeploymentType.HIDDEN_ROGUE)
					speed *= 1.25f;
			}

			return speed;
		}

		public override void Simulate()
		{
			if (IsFrozen)
			{
				if (Input.Pressed(InputButton.Jump))
				{
					BaseVelocity = Vector3.Zero;
					WishVelocity = Vector3.Zero;
					Velocity = (Input.Rotation.Forward * LeapVelocity * 2f);
					IsFrozen = false;
				}

				return;
			}

			base.Simulate();
		}
	}
}
