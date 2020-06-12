using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			Aims at collectable resources.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="12/06/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemaryHarvesterAimer : RosemaryAimer
	{
		public string[] resourceNames = new string[] { "Steel" };

		public override bool CanAttackUnit(RosemaryUnit other)
		{
			return IsEnemyAndInViewRange(other)
				&& other.useResourceReward
				&& resourceNames.Contains(other.resourceReward.resourceName);
		}
	}
}
