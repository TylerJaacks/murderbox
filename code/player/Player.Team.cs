using Sandbox;
using System;

namespace MurderboxGamemode
{
	partial class Player
	{
		[Net] public int TeamIndex { get; set; }
		public int LastTeamIndex { get; set; }
		private BaseTeam _team;

		public BaseTeam Team
		{
			get => _team;

			set
			{
				// A player must be on a valid team.
				if (value != null && value != _team)
				{
					_team?.Leave(his);
					_team = value;
					_team.Join(this);

					if (IsServer)
					{
						TeamIndex = _team.Index;

						var client = GetClientOwner();

						// You have to do this for now.
						client.SetScore("team", TeamIndex);
					}
				}
			}
		}
	}
}
