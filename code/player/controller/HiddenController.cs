using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

public class HiddenController : CustomWalkController
{
	public override float SprintSpeed { get; set; } = 380f;
	public bool IsFrozen { get; set; }
	public float LeapVelocity { get; set; } = 300f;

	public override void AddJumpVelocity()
	{
		if (Pawn is Player)
		{
			var minLeapVelocity = (LeapVelocity * 0.2f);
			var extraLeapVelocity = (LeapVelocity * 0.8f);
			var actualLeapVelocity = minLeapVelocity + (extraLeapVelocity / 100f);

			Velocity += (Pawn.EyeRot.Forward * actualLeapVelocity);
		}

		base.AddJumpVelocity();
	}

	public override float GetWishSpeed()
	{
		var speed = base.GetWishSpeed();

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
