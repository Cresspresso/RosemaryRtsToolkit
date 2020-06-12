using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public struct PurchaseFailureReason
	{
		float balance;
		float amount;
	}

	public partial class RosemaryFaction
	{
		[Header("Resources")]
#pragma warning disable CS0649
		[SerializeField]
		private ResourceAmount[] m_initialResourceBalances = new ResourceAmount[1] {
			new ResourceAmount("Steel", 1000)
		};
#pragma warning restore CS0649
		private Dictionary<string, float> m_resourceBalances;
		public IReadOnlyDictionary<string, float> resourceBalances {
			get
			{
				PrepareResources();
				return m_resourceBalances;
			}
		}

		private bool m_isResourcesPrepared = false;
		private void PrepareResources()
		{
			if (m_isResourcesPrepared) { return; }
			m_isResourcesPrepared = true;
			m_resourceBalances = m_initialResourceBalances.ToDictionary(a => a.resourceName, a => a.amount);
		}

		public void OnAwake_ResourcePartial()
		{
			PrepareResources();
		}

		public float GetResourceBalance(string resourceName)
		{
			PrepareResources();
			if (m_resourceBalances.TryGetValue(resourceName, out var balance))
			{
				return balance;
			}
			else
			{
				return 0.0f;
			}
		}

		public void SetResourceBalance(string resourceName, float balance)
		{
			PrepareResources();
			if (m_resourceBalances.ContainsKey(resourceName))
			{
				m_resourceBalances[resourceName] = balance;
			}
			else
			{
				m_resourceBalances.Add(resourceName, balance);
			}
		}

		public void IncreaseResourceBalance(string resourceName, float amount)
		{
			var balance = GetResourceBalance(resourceName);
			balance += amount;
			SetResourceBalance(resourceName, balance);
		}

		public bool TryPurchase(
			IReadOnlyDictionary<string, float> resourceCosts,
			out IDictionary<string, float> notEnough)
		{
			PrepareResources();

			notEnough = new Dictionary<string, float>();
			var balances = new Dictionary<string, float>();
			bool fail = false;
			foreach (var pair in resourceCosts)
			{
				var resourceName = pair.Key;
				var cost = pair.Value;
				var balance = GetResourceBalance(resourceName);
				if (balance < cost)
				{
					fail = true;
					notEnough.Add(resourceName, balance);
				}
				if (!fail)
				{
					balances.Add(resourceName, balance);
				}
			}
			if (fail)
			{
				return false;
			}

			// should be nothrow from here down
			foreach (var pair in resourceCosts)
			{
				var resourceName = pair.Key;
				var cost = pair.Value;
				SetResourceBalance(resourceName, balances[resourceName] - cost);
			}
			return true;
		}

		public bool TryPurchase(IReadOnlyDictionary<string, float> resourceCosts)
			=> TryPurchase(resourceCosts, out _);
	}
}
