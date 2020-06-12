using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso.Rosemary
{
	public class RosemaryProductPanel : RosemarySpawner
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
			if (!prefab.GetComponent<RosemaryProductButton>())
			{
				var msg = $"{nameof(RosemaryProductButton)} component not found on prefab.";
				Debug.LogError(msg, this);
				if (!nothrow)
				{
					throw new System.InvalidOperationException(msg);
				}
			}
		}

		private Dictionary<RosemaryProduct, RosemaryProductButton> m_buttons = new Dictionary<RosemaryProduct, RosemaryProductButton>();

		public RosemaryProduct spawnProduct { get; private set; }

		public RosemaryProductButton Spawn(RosemaryProduct product)
		{
			CheckPrefab(nothrow: false);

			var oldProduct = spawnProduct;
			spawnProduct = product;
			try
			{
				var spawn = Spawn(Vector3.zero, Quaternion.identity);
				var button = spawn.GetComponent<RosemaryProductButton>();
				m_buttons.Add(product, button);
				return button;
			}
			finally
			{
				spawnProduct = oldProduct;
			}
		}

		protected override void OnSpawnDisableExtra(RosemarySpawn spawn)
		{
			if (!spawn) return;
			var button = spawn.GetComponent<RosemaryProductButton>();
			if (!button) return;

			if (button.product)
			{
				m_buttons.Remove(button.product);
			}
		}

		private void Update()
		{
			var selection = RosemarySelection.instance;

			var productLists = (
				from u in selection.controllableUnits
				where u.canProduce
				select u.productList)
				.Distinct();

			var products = productLists.SelectMany(pl => pl.products);

			foreach (var product in products)
			{
				if (!m_buttons.ContainsKey(product))
				{
					Spawn(product);
				}
			}

			foreach (var pair in m_buttons.ToArray())
			{
				var product = pair.Key;
				if (!products.Contains(product))
				{
					if (m_buttons.TryGetValue(product, out var button))
					{
						button.GetComponent<RosemarySpawn>().Despawn();
					}
				}
			}
		}
	}
}
