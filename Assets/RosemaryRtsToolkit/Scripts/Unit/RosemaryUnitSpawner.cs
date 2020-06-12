using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Fires projectiles at the target enemy.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="08/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	///		<log date="04/06/2020"
	///			version="1.001"
	///			author="Elijah Shadbolt">
	///			Added projectile spawners.
	///		</log>
	/// </changelog>
	public sealed class RosemaryUnitSpawner : RosemarySpawner
	{
		[SerializeField]
		private RosemaryFaction m_faction;
		public RosemaryFaction faction {
			get => m_faction;
			private set => m_faction = value;
		}

		public RosemarySpawn Spawn(Vector3 position, Quaternion rotation, RosemaryFaction faction)
		{
			var oldFaction = this.faction;
			this.faction = faction;
			try
			{
				return Spawn(position, rotation);
			}
			finally
			{
				this.faction = oldFaction;
			}
		}
	}
}
