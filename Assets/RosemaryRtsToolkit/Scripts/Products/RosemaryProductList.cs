using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public class RosemaryProductList : MonoBehaviour
	{
		private static Dictionary<string, RosemaryProductList> m_instances = new Dictionary<string, RosemaryProductList>();
		public static bool TryGetInstance(string name, out RosemaryProductList instance)
		{
			if (m_instances.TryGetValue(name, out instance))
			{
				if (instance)
				{
					return true;
				}
			}
			instance = FindObjectsOfType<RosemaryProductList>()
				.FirstOrDefault(x => x && x.name == name);
			return (bool)instance;
		}

		[SerializeField]
		private List<RosemaryProduct> m_products = new List<RosemaryProduct>();
		public List<RosemaryProduct> products => m_products;

		private void Awake()
		{
			m_instances.Add(name, this);
		}

		private void OnDestroy()
		{
			m_instances.Remove(name);
		}
	}
}
