﻿using Sandbox;
using System;

namespace HiddenGamemode
{
	[Library( "hdn_knife", Title = "Knife" )]
	partial class Knife : Weapon
	{
		public override string ViewModelPath => "weapons/rust_boneknife/v_rust_boneknife.vmdl";
		public override float PrimaryRate => 1.0f;
		public override float SecondaryRate => 0.3f;
		public override bool IsMelee => true;
		public override int HoldType => 0;
		public override int Bucket => 1;
		public override int BaseDamage => 35;
		public virtual int MeleeDistance => 80;

		public override void Spawn()
		{
			base.Spawn();

			// TODO: EnableDrawing = false does not work.
			RenderAlpha = 0f;

			SetModel( "weapons/rust_boneknife/rust_boneknife.vmdl" );
		}

		public virtual void MeleeStrike( float damage, float force )
		{
			var forward = Owner.EyeRot.Forward;
			forward = forward.Normal;

			foreach ( var tr in TraceBullet( Owner.EyePos, Owner.EyePos + forward * MeleeDistance, 10f ) )
			{
				if ( !tr.Entity.IsValid() ) continue;

				tr.Surface.DoBulletImpact( tr );

				if ( !IsServer ) continue;

				using ( Prediction.Off() )
				{
					var damageInfo = DamageInfo.FromBullet( tr.EndPos, forward * 100 * force, damage )
						.UsingTraceResult( tr )
						.WithAttacker( Owner )
						.WithWeapon( this );

					tr.Entity.TakeDamage( damageInfo );
				}
			}
		}

		public override void AttackSecondary()
		{
			StartChargeAttack();
		}

		public override void AttackPrimary()
		{
			ShootEffects();
			PlaySound( "rust_boneknife.attack" );
			MeleeStrike( BaseDamage, 1.5f );
		}

		public override void OnChargeAttackFinish()
		{
			ShootEffects();
			PlaySound( "rust_boneknife.attack" );
			MeleeStrike( BaseDamage * 3f, 1.5f );
		}
	}
}
