using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public class RosemaryProduct : MonoBehaviour
	{
#pragma warning disable CS0649
		[SerializeField]
		private RosemaryUnitSpawner m_spawner;
#pragma warning restore CS0649
		public RosemaryUnitSpawner spawner => m_spawner;

#pragma warning disable CS0649
		[SerializeField]
		private ResourceAmount[] m_initialResourceCosts = new ResourceAmount[0];
#pragma warning restore CS0649
		private Dictionary<string, float> m_resourceCosts;
		public Dictionary<string, float> resourceCosts {
			get
			{
				if (null == m_resourceCosts)
				{
					m_resourceCosts = m_initialResourceCosts.ToDictionary(a => a.resourceName, a => a.amount);
				}
				return m_resourceCosts;
			}
		}

		public string title = "Product";
	}
}
