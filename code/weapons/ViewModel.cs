using System;

using Sandbox;

// ReSharper disable once CheckNamespace
namespace MurderboxGamemode;

partial class ViewModel : BaseViewModel
{
	float WalkBob = 0;

	public override void PostCameraSetup(ref CameraSetup camSetup)
	{
		base.PostCameraSetup(ref camSetup);

		AddCameraEffects(ref camSetup);
	}

	private void AddCameraEffects(ref CameraSetup camSetup)
	{
		Rotation = Local.Pawn.EyeRot;

		var speed = Owner.Velocity.Length.LerpInverse(0, 320);
		var left = camSetup.Rotation.Left;
		var up = camSetup.Rotation.Up;

		if (Owner.GroundEntity != null)
		{
			WalkBob += Time.Delta * 25.0f * speed;
		}

		Position += up * MathF.Sin(WalkBob) * speed * -1;
		Position += left * MathF.Sin(WalkBob * 0.6f) * speed * -0.5f;
	}
}
