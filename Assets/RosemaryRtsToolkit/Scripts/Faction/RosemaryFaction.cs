using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cresspresso.Rosemary
{
	public enum FactionLifeOutcome
	{
		None_StillAlive,
		Defeat_AllUnitsDestroyed,
		Victory_EliminatedAllEnemyFactions,
	}

	/// <summary>
	///		<para>
	///			Each <see cref="RosemaryUnit"/> unit has a <see cref="RosemaryFaction"/>.
	///		</para>
	///		<para>
	///			Every pair of factions have a relationship between them, either enemy or ally.
	///		</para>
	///		<para>
	///			Units cannot attack other units unless their factions are enemies.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="08/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public partial class RosemaryFaction : MonoBehaviour
	{
		private static HashSet<RosemaryFaction> m_instances = new HashSet<RosemaryFaction>();
		public static IEnumerable<RosemaryFaction> instances => m_instances;

		private void OnEnable()
		{
			m_instances.Add(this);
		}

		private void OnDisable()
		{
			m_instances.Remove(this);
		}

		[Header("Allies")]
#pragma warning disable CS0649
		[SerializeField]
		private RosemaryFaction[] m_initialAllies = new RosemaryFaction[0];
#pragma warning restore CS0649
		private HashSet<RosemaryFaction> m_allies = new HashSet<RosemaryFaction>();
		public IReadOnlyCollection<RosemaryFaction> allies {
			get
			{
				PrepareAllies();
				return m_allies;
			}
		}

		private bool AddAlly(RosemaryFaction other)
		{
			PrepareAllies();
			return m_allies.Add(other);
		}

		private bool RemoveAlly(RosemaryFaction other)
		{
			PrepareAllies();
			return m_allies.Remove(other);
		}

		public static void SetAllies(RosemaryFaction a, RosemaryFaction b)
		{
			a.AddAlly(b);
			b.AddAlly(a);
		}

		public static void SetEnemies(RosemaryFaction a, RosemaryFaction b)
		{
			a.RemoveAlly(b);
			b.RemoveAlly(a);
		}

		public bool IsAlly(RosemaryFaction other)
		{
			PrepareAllies();
			return this == other || m_allies.Contains(other);
		}

		public bool IsEnemy(RosemaryFaction other)
		{
			PrepareAllies();
			return !IsAlly(other);
		}

		private bool m_isAlliesPrepared = false;
		private void PrepareAllies()
		{
			if (m_isAlliesPrepared) { return; }
			m_isAlliesPrepared = true;
			foreach (var ally in m_initialAllies)
			{
				if (!ally) { continue; }
				if (!m_allies.Contains(ally))
				{
					SetAllies(this, ally);
				}
			}
		}

		private void Awake()
		{
			PrepareAllies();
			OnAwake_ResourcePartial();
		}
	}
}
