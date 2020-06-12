using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso.Rosemary
{
	public class RosemaryResourcesPanel : RosemarySpawner
	{
		protected override void Start()
		{
			CheckPrefab(nothrow: true);

			base.Start();

			foreach (var spawn in active)
			{
				spawn.Despawn();
			}
		}

		private void CheckPrefab(bool nothrow)
		{
			if (!prefab || !prefab.GetComponent<RosemaryResourceAmountPanel>())
			{
				var msg = $"{nameof(RosemaryResourceAmountPanel)} component not found on prefab.";
				Debug.LogError(msg, this);
				if (!nothrow)
				{
					throw new System.InvalidOperationException(msg);
				}
			}
		}

		private Dictionary<string, RosemaryResourceAmountPanel> m_elements = new Dictionary<string, RosemaryResourceAmountPanel>();

		public string spawnResourceName { get; private set; }

		public RosemaryResourceAmountPanel Spawn(string resourceName)
		{
			CheckPrefab(nothrow: false);

			var oldResourceName = spawnResourceName;
			spawnResourceName = resourceName;
			try
			{
				var spawn = Spawn(Vector3.zero, Quaternion.identity);
				var element = spawn.GetComponent<RosemaryResourceAmountPanel>();
				m_elements.Add(resourceName, element);
				return element;
			}
			finally
			{
				spawnResourceName = oldResourceName;
			}
		}

		protected override void OnSpawnDisableExtra(RosemarySpawn spawn)
		{
			if (!spawn) return;
			var element = spawn.GetComponent<RosemaryResourceAmountPanel>();
			if (!element) return;

			m_elements.Remove(element.resourceName);
		}

		private void Update()
		{
			var faction = RosemarySelection.instance.faction;

			var resourceBalances = faction.resourceBalances;

			foreach (var pair in resourceBalances)
			{
				var resourceName = pair.Key;
				if (!m_elements.ContainsKey(resourceName))
				{
					Spawn(resourceName);
				}
			}

			foreach (var pair in m_elements.ToArray())
			{
				var resourceName = pair.Key;
				if (!resourceBalances.ContainsKey(resourceName))
				{
					if (m_elements.TryGetValue(resourceName, out var element))
					{
						element.GetComponent<RosemarySpawn>().Despawn();
					}
				}
			}

			// DEBUGGING TODO remove
			if (Input.GetKeyDown(KeyCode.I))
			{
				RosemarySelection.instance.faction.SetResourceBalance("Fish", 1000);
				RosemarySelection.instance.faction.SetResourceBalance("Cats", 1000);
				RosemarySelection.instance.faction.SetResourceBalance("Eeep", 1000);
			}
		}
	}
}
