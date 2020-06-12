using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public partial class RosemaryFaction
	{
		[Header("Outcomes")]
		public bool isResourceFaction = false;

		public FactionLifeOutcome lifeOutcome { get; private set; }
		public bool isAlive => lifeOutcome == FactionLifeOutcome.None_StillAlive;
		public event Action<RosemaryFaction, FactionLifeOutcome> onGivenLifeOutcome;

		public void GiveOutcome(FactionLifeOutcome outcome)
		{
			if (outcome == FactionLifeOutcome.None_StillAlive)
			{
				throw new ArgumentException("Outcome cannot be 'still alive'.");
			}

			if (lifeOutcome != FactionLifeOutcome.None_StillAlive)
			{
				throw new InvalidOperationException("This faction has already been given an outcome.");
			}

			lifeOutcome = outcome;
			Debug.Log("Faction " + name + " was given an outcome: " + outcome, this);
			Utility.TryCatchLog(() => onGivenLifeOutcome?.Invoke(this, lifeOutcome));
		}
	}
}
