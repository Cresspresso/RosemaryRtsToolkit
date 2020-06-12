using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	[System.Serializable]
	public struct ResourceAmount
	{
		public string resourceName;// = "Steel";
		public float amount;// = 10.0f;

		public ResourceAmount(string resourceName, float amount)
		{
			this.resourceName = resourceName;
			this.amount = amount;
		}
	}

	public partial class RosemaryUnit
	{
		[Header("Factory")]

		[SerializeField]
		private Transform m_factoryUnitSpawnPoint;
		public Transform factoryUnitSpawnPoint {
			get
			{
				if (!m_factoryUnitSpawnPoint)
				{
					m_factoryUnitSpawnPoint = transform;
				}
				return m_factoryUnitSpawnPoint;
			}
			set => m_factoryUnitSpawnPoint = value;
		}

#pragma warning disable CS0649
		[SerializeField]
		private string m_productListName = "";
#pragma warning restore CS0649
		public string productListName => m_productListName;

		private RosemaryProductList m_productList;
		public RosemaryProductList productList {
			get
			{
				FindProductList();
				return m_productList;
			}
		}
		private void FindProductList()
		{
			if (!m_productList && !string.IsNullOrWhiteSpace(productListName))
			{
				if (RosemaryProductList.TryGetInstance(productListName, out var pl))
				{
					m_productList = pl;
				}
				else
				{
					Debug.LogError("Product List with name '" + productListName + "' not found.", this);
				}
			}
		}
		public bool canProduce => productList;


		private void OnEnableFactoryPartial()
		{
			FindProductList();

			if (canProduce && !canMove)
			{
				this.CommandMove(DefaultBirthDestination);
			}
		}

		private Vector3 DefaultBirthDestination => factoryUnitSpawnPoint.TransformPoint(0, 0, 1);

		public void BirthUnit(RosemaryUnitSpawner spawner)
		{
			var spawn = spawner.Spawn(
				factoryUnitSpawnPoint.position,
				factoryUnitSpawnPoint.rotation,
				faction);
			var unit = spawn.GetComponent<RosemaryUnit>();
			if (unit)
			{
				var pos = canMove
					? DefaultBirthDestination
					: destination;
				unit.CommandMove(pos);
			}
		}

		public void CommandProduceUnit(RosemaryProduct product)
		{
			if (!product)
			{
				throw new System.ArgumentNullException(nameof(product));
			}
			if (!productList.products.Contains(product))
			{
				Debug.Log("Building cannot produce that kind of product: " + product.name, this);
				return;
			}

			var costs = product.resourceCosts;
			IDictionary<string, float> notEnough;
			if (faction.TryPurchase(costs, out notEnough))
			{
				BirthUnit(product.spawner);
			}
			else
			{
				var seq = notEnough.Select(a =>
				{
					var resourceName = a.Key;
					return resourceName + ": "
					+ a.Value + " (" + faction.GetResourceBalance(resourceName) + ")"
					+ " < " + costs[resourceName];
				});
				Debug.Log("Failed to purchase unit: " + string.Join(", ", seq), this);
			}
		}
	}
}
