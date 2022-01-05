﻿using Sandbox;
using System;

namespace MurderboxGamemode;

partial class Weapon : BaseWeapon
{
	public virtual AmmoType AmmoType => AmmoType.Pistol;
	public virtual int ClipSize => 16;
	public virtual float ReloadTime => 3.0f;
	public virtual bool IsMelee => false;
	public virtual int Bucket => 1;
	public virtual int BucketWeight => 100;
	public virtual bool UnlimitedAmmo => false;
	public virtual float ChargeAttackDuration => 2;
	public virtual int BaseDamage => 10;
	public override string ViewModelPath => "weapons/rust_pistol/v_rust_pistol.vmdl";

	[Net, Predicted]
	public int AmmoClip { get; set; }

	[Net, Predicted]
	public TimeSince TimeSinceReload { get; set; }

	[Net, Predicted]
	public bool IsReloading { get; set; }

	[Net, Predicted]
	public TimeSince TimeSinceDeployed { get; set; }

	[Net, Predicted]
	public TimeSince TimeSinceChargeAttack { get; set; }

	public float ChargeAttackEndTime;

	public int AvailableAmmo()
	{
		if (Owner is not Player owner) return 0;
		return owner.AmmoCount(AmmoType);
	}

	public override void ActiveStart(Entity owner)
	{
		base.ActiveStart(owner);

		TimeSinceDeployed = 0;
	}

	public override void Spawn()
	{
		base.Spawn();

		AmmoClip = ClipSize;

		SetModel("weapons/rust_pistol/rust_pistol.vmdl");
	}

	public override void Reload()
	{
		if (IsMelee || IsReloading)
			return;

		if (AmmoClip >= ClipSize)
			return;

		TimeSinceReload = 0;

		if (Owner is Player player)
		{
			if (!UnlimitedAmmo)
			{
				if (player.AmmoCount(AmmoType) <= 0)
					return;
			}
		}

		IsReloading = true;

		(Owner as AnimEntity).SetAnimBool("b_reload", true);

		DoClientReload();
	}

	public override void Simulate(Client owner)
	{
		if (owner.Pawn is Player player)
		{
			if (owner.Pawn.LifeState == LifeState.Alive)
			{
				if (ChargeAttackEndTime > 0f && Time.Now >= ChargeAttackEndTime)
				{
					OnChargeAttackFinish();
					ChargeAttackEndTime = 0f;
				}
			}
			else
			{
				ChargeAttackEndTime = 0f;
			}
		}

		if (!IsReloading)
		{
			base.Simulate(owner);
		}

		if (IsReloading && TimeSinceReload > ReloadTime)
		{
			OnReloadFinish();
		}
	}

	public override bool CanPrimaryAttack()
	{
		if (ChargeAttackEndTime > 0f && Time.Now < ChargeAttackEndTime)
			return false;

		return base.CanPrimaryAttack();
	}

	public override bool CanSecondaryAttack()
	{
		if (ChargeAttackEndTime > 0f && Time.Now < ChargeAttackEndTime)
			return false;

		return base.CanSecondaryAttack();
	}

	public virtual void StartChargeAttack()
	{
		ChargeAttackEndTime = Time.Now + ChargeAttackDuration;
	}

	public virtual void OnChargeAttackFinish() { }

	public virtual void OnReloadFinish()
	{
		IsReloading = false;

		if (Owner is Player player)
		{
			if (!UnlimitedAmmo)
			{
				var ammo = player.TakeAmmo(AmmoType, ClipSize - AmmoClip);

				if (ammo == 0)
					return;

				AmmoClip += ammo;
			}
			else
			{
				AmmoClip = ClipSize;
			}
		}
	}

	[ClientRpc]
	public virtual void DoClientReload()
	{
		ViewModelEntity?.SetAnimBool("reload", true);
	}

	public override void AttackPrimary()
	{
		TimeSincePrimaryAttack = 0;
		TimeSinceSecondaryAttack = 0;

		ShootEffects();
		ShootBullet(0.05f, 1.5f, BaseDamage, 3.0f);
	}

	[ClientRpc]
	protected virtual void ShootEffects()
	{
		Host.AssertClient();

		if (!IsMelee)
		{
			Particles.Create("particles/pistol_muzzleflash.vpcf", EffectEntity, "muzzle");
		}

		if (IsLocalPawn)
		{
			_ = new Sandbox.ScreenShake.Perlin();
		}

		ViewModelEntity?.SetAnimBool("fire", true);
		CrosshairPanel?.CreateEvent("fire");
	}

	public virtual void ShootBullet(float spread, float force, float damage, float bulletSize)
	{
		var forward = Owner.EyeRot.Forward;
		forward += (Vector3.Random + Vector3.Random + Vector3.Random + Vector3.Random) * spread * 0.25f;
		forward = forward.Normal;

		foreach (var tr in TraceBullet(Owner.EyePos, Owner.EyePos + forward * 5000, bulletSize))
		{
			tr.Surface.DoBulletImpact(tr);

			if (!IsServer) continue;
			if (!tr.Entity.IsValid()) continue;

			using (Prediction.Off())
			{
				var damageInfo = DamageInfo.FromBullet(tr.EndPos, forward * 100 * force, damage)
					.UsingTraceResult(tr)
					.WithAttacker(Owner)
					.WithWeapon(this);

				tr.Entity.TakeDamage(damageInfo);
			}
		}
	}

	public bool TakeAmmo(int amount)
	{
		if (AmmoClip < amount)
			return false;

		AmmoClip -= amount;
		return true;
	}

	public override void CreateViewModel()
	{
		Host.AssertClient();

		if (string.IsNullOrEmpty(ViewModelPath))
			return;

		ViewModelEntity = new ViewModel
		{
			Position = Position,
			Owner = Owner,
			EnableViewmodelRendering = true
		};

		ViewModelEntity.SetModel(ViewModelPath);
	}

	public override void CreateHudElements()
	{
		if (Local.Hud == null) return;
	}

	public bool IsUsable()
	{
		if (IsMelee || ClipSize == 0 || AmmoClip > 0)
		{
			return true;
		}

		return AvailableAmmo() > 0;
	}
}
