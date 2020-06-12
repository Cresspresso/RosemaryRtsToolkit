using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cresspresso.Rosemary
{
	public class RosemaryObjectiveManager : MonoBehaviour
	{
		public float checkPeriod = 0.5f;

		public bool isGameOverForPlayer { get; private set; } = false;

		public event Action<FactionLifeOutcome> onGameOverForPlayer;

		public static RosemaryObjectiveManager instance => Singleton<RosemaryObjectiveManager>.instance;
		public static RosemaryObjectiveManager instanceOrNull => Singleton<RosemaryObjectiveManager>.instanceOrNull;

		private void Awake()
		{
			Singleton<RosemaryObjectiveManager>.OnAwake(this);
			StartCoroutine(Co_CheckVictoryConditions());
		}

		private IEnumerator Co_CheckVictoryConditions()
		{
			while (true)
			{
				yield return new WaitForSeconds(checkPeriod * Random.Range(0.5f, 1.0f));
				var factions = RosemaryFaction.instances.Where(f => !f.isResourceFaction);
				var units = RosemaryUnit.instances;
				foreach (var faction in factions)
				{
					if (faction.isAlive)
					{
						bool anyUnits = units.Any(u => u.faction == faction);
						if (!anyUnits)
						{
							faction.GiveOutcome(FactionLifeOutcome.Defeat_AllUnitsDestroyed);
							continue;
						}
					}
				}
				foreach (var faction in factions)
				{
					if (faction.isAlive)
					{
						bool anyEnemies = factions.Any(f =>
							f.isAlive && f != faction && f.IsEnemy(faction)
						);
						if (!anyEnemies)
						{
							faction.GiveOutcome(FactionLifeOutcome.Victory_EliminatedAllEnemyFactions);
							continue;
						}
					}
				}

				CheckPlayerLifeState();
				if (isGameOverForPlayer)
				{
					break; // end victory check loop
				}
			}
		}

		private void CheckPlayerLifeState()
		{
			var player = RosemarySelection.instance.faction;
			if (!player.isAlive)
			{
				isGameOverForPlayer = true;
				Utility.TryCatchLog(() => onGameOverForPlayer?.Invoke(player.lifeOutcome));
			}
		}
	}
}
