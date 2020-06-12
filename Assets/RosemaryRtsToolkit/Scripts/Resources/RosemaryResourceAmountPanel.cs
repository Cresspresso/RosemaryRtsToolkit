using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso.Rosemary
{
	[RequireComponent(typeof(RosemarySpawn))]
	public class RosemaryResourceAmountPanel : MonoBehaviour
	{
		public Text labelTextElement;
		public Text valueTextElement;

		private string m_resourceName = string.Empty;
		public string resourceName {
			get => m_resourceName;
			private set
			{
				m_resourceName = value;
				labelTextElement.text = m_resourceName;
			}
		}

		private float m_resourceAmount = 0.0f;
		public float resourceAmount {
			get => m_resourceAmount;
			private set
			{
				m_resourceAmount = value;
				valueTextElement.text = m_resourceAmount.ToString();
			}
		}

		private void UpdateAmount()
		{
			resourceAmount = RosemarySelection.instance.faction.GetResourceBalance(resourceName);
		}

		private void OnEnable()
		{
			var spawn = GetComponent<RosemarySpawn>();
			if (spawn.spawner.spawnMode != SpawnMode.Spawning)
			{
				spawn.Despawn();
				return;
			}

			var spawner = (RosemaryResourcesPanel)spawn.spawner;
			resourceName = spawner.spawnResourceName;
			UpdateAmount();
		}

		private void Update()
		{
			UpdateAmount();
		}
	}
}
