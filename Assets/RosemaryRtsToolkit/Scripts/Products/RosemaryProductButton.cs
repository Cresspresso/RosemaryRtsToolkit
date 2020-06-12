using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso.Rosemary
{
	[RequireComponent(typeof(RosemarySpawn))]
	public class RosemaryProductButton : MonoBehaviour
	{
		public Button button;
		public Text titleTextElement;
		public RosemaryProduct product { get; private set; }

		private void Awake()
		{
			button.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			if (button)
			{
				button.onClick.RemoveListener(OnClick);
			}
		}

		private void OnEnable()
		{
			var spawn = GetComponent<RosemarySpawn>();
			if (spawn.spawner.spawnMode != SpawnMode.Spawning)
			{
				spawn.Despawn();
				return;
			}

			var spawner = (RosemaryProductPanel)spawn.spawner;
			product = spawner.spawnProduct;
			titleTextElement.text = product.title;
		}

		private void OnClick()
		{
			RosemarySelection.instance.CommandProduceUnit(product);
		}
	}
}
