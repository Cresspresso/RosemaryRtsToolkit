using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Spawns projectiles.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="04/06/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryProjectileSpawner : RosemarySpawner
	{
		[SerializeField]
		private RosemaryFaction m_faction;
		public RosemaryFaction faction {
			get => m_faction;
			private set => m_faction = value;
		}

		[SerializeField]
		private RosemaryWeapon m_weapon;
		public RosemaryWeapon weapon {
			get => m_weapon;
			private set => m_weapon = value;
		}

		[SerializeField]
		private RosemaryUnit m_target;
		public RosemaryUnit target {
			get => m_target;
			private set => m_target = value;
		}

		[SerializeField]
		private TargetPoint m_targetPoint;
		public TargetPoint targetPoint {
			get => m_targetPoint;
			private set => m_targetPoint = value;
		}

		public void Spawn(
			Vector3 position,
			Quaternion rotation,
			RosemaryWeapon weapon,
			RosemaryFaction faction,
			RosemaryUnit target,
			TargetPoint targetPoint)
		{
			var oldFaction = this.faction;
			var oldWeapon = this.weapon;
			var oldTarget = this.target;
			var oldTargetPoint = this.targetPoint;
			this.faction = faction;
			this.weapon = weapon;
			this.target = target;
			this.targetPoint = targetPoint;
			try
			{
				Spawn(position, rotation);
			}
			finally
			{
				this.faction = oldFaction;
				this.weapon = oldWeapon;
				this.target = oldTarget;
				this.targetPoint = oldTargetPoint;
			}
		}
	}
}
