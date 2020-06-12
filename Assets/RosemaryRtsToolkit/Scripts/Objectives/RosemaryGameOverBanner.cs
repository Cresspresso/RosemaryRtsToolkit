using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso.Rosemary
{
	public class RosemaryGameOverBanner : MonoBehaviour
	{
		public GameObject visuals;
		public GameObject victoryVisuals;
		public GameObject defeatVisuals;
		public Text reasonTextElement;

		private void Awake()
		{
			visuals.SetActive(false);
			victoryVisuals.SetActive(false);
			defeatVisuals.SetActive(false);
			RosemaryObjectiveManager.instance.onGameOverForPlayer += OnGameOverForPlayer;
		}

		private void OnGameOverForPlayer(FactionLifeOutcome lifeOutcome)
		{
			switch (lifeOutcome)
			{
				default:
				case FactionLifeOutcome.None_StillAlive:
				case FactionLifeOutcome.Defeat_AllUnitsDestroyed:
					Show(false, "All Units Destroyed");
					break;
				case FactionLifeOutcome.Victory_EliminatedAllEnemyFactions:
					Show(true, "All Enemies Eliminated");
					break;
			}
		}

		private void Show(bool isVictory, string reason)
		{
			visuals.SetActive(true);
			victoryVisuals.SetActive(isVictory);
			defeatVisuals.SetActive(!isVictory);
			reasonTextElement.text = reason;
		}
	}
}
