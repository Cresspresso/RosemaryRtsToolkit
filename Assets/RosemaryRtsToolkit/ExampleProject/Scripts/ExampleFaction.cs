#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cresspresso.Rosemary;

namespace Cresspresso
{
	namespace ExampleProject
	{
		public class ExampleFaction : MonoBehaviour, IFaction
		{
			[SerializeField]
			private List<ExampleFaction> m_initialAllies = new List<ExampleFaction>();
			private bool m_initialised;
			public HashSet<ExampleFaction> allies { get; } = new HashSet<ExampleFaction>();

			private void Awake()
			{
				if (!m_initialised)
				{
					allies.UnionWith(m_initialAllies);
					m_initialised = true;
				}
			}

			public bool IsEnemy(IFaction other)
			{
				return (IFaction)this != other && !allies.Contains((ExampleFaction)other);
			}
		}
	}
}
#endif