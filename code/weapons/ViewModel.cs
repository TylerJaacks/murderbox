﻿using Sandbox;
using System;

namespace MurderboxGamemode
{
	partial class ViewModel : BaseViewModel
	{
		float walkBob = 0;

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
				walkBob += Time.Delta * 25.0f * speed;
			}

			Position += up * MathF.Sin(walkBob) * speed * -1;
			Position += left * MathF.Sin(walkBob * 0.6f) * speed * -0.5f;
		}
	}
}
