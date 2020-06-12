using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso.Rosemary
{
	public class DebugFactionChanger : MonoBehaviour
	{
		private RosemaryFaction[] factions;
		private RosemarySelection selection;
		public Text textElement;
		private int index = 0;

		private void Awake()
		{
			factions = FindObjectsOfType<RosemaryFaction>();
			selection = GetComponent<RosemarySelection>();

			index = Array.IndexOf(factions, selection.faction);
			if (index < 0) { index = 0; }

			textElement.text = "Faction: " + selection.faction.name;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				selection.Clear();
				for (int i = 0; i < factions.Length; ++i)
				{
					index = Utility.Cycle(index + 1, factions.Length);
					selection.faction = factions[index];
					if (selection.faction.isAlive) { break; }
				}
				textElement.text = "Faction: " + selection.faction.name;
			}
		}
	}
}
