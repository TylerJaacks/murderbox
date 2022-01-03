﻿using Sandbox.UI.Construct;
using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace HiddenGamemode
{
	public class Radar : Panel
	{
		private readonly Dictionary<Player, RadarDot> _radarDots = new();

		public Panel Anchor;

		public Radar()
		{
			StyleSheet.Load( "/ui/Radar.scss" );
			SetTemplate( "/ui/Radar.html" );
		}

		public override void Tick()
		{
			base.Tick();

			if ( Local.Pawn is not Player localPlayer )
				return;

			SetClass( "hidden", localPlayer.LifeState != LifeState.Alive );

			var deleteList = new List<Player>();
			var count = 0;

			deleteList.AddRange( _radarDots.Keys );

			var players = Entity.All.OfType<Player>().OrderBy( x => Vector3.DistanceBetween( x.EyePos, CurrentView.Position ) );

			foreach ( var v in players )
			{
				if ( UpdateRadar( v ) )
				{
					deleteList.Remove( v );
					count++;
				}
			}

			foreach ( var player in deleteList )
			{
				_radarDots[player].Delete();
				_radarDots.Remove( player );
			}
		}

		public RadarDot CreateRadarDot( Player player )
		{
			var tag = new RadarDot( player )
			{
				Parent = this
			};

			return tag;
		}

		public bool UpdateRadar( Player player )
		{
			if ( player.IsLocalPawn || !player.HasTeam || player.Team.HideNameplate )
				return false;

			if ( player.LifeState != LifeState.Alive )
				return false;

			if ( Local.Pawn is not Player localPlayer )
				return false;

			var radarRange = 2048f;

			if ( player.Position.Distance( localPlayer.Position ) > radarRange )
				return false;

			if ( !_radarDots.TryGetValue( player, out var tag ) )
			{
				tag = CreateRadarDot( player );
				_radarDots[player] = tag;
			}

			// This is probably fucking awful maths but it works.

			var difference = player.Position - localPlayer.Position;
			var radarSize = 256f;

			var x = (radarSize / radarRange) * difference.x * 0.5f;
			var y = (radarSize / radarRange) * difference.y * 0.5f;

			var angle = (MathF.PI / 180) * (CurrentView.Rotation.Yaw() - 90f);
			var x2 = x * MathF.Cos( angle ) + y * MathF.Sin( angle );
			var y2 = y * MathF.Cos( angle ) - x *MathF.Sin( angle );

			tag.Style.Left = (radarSize / 2f) + x2;
			tag.Style.Top = (radarSize / 2f) - y2;
			tag.Style.Dirty();

			return true;
		}
	}
}
