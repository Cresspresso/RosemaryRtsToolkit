using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			The set of all <see cref="RosemaryUnit"/> units currently selected by the player.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="08/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	public class RosemarySelection : MonoBehaviour
	{
		public RosemaryFaction faction;

		public RosemaryCommandEffectSpawner moveCommandEffectSpawner;
		public RosemaryCommandEffectSpawner attackCommandEffectSpawner;
		public RosemaryCommandEffectSpawner followCommandEffectSpawner;

		public static RosemarySelection instance => Singleton<RosemarySelection>.instance;
		public static RosemarySelection instanceOrNull => Singleton<RosemarySelection>.instanceOrNull;

		private void Awake()
		{
			Singleton<RosemarySelection>.OnAwake(this);
		}



		private List<RosemaryUnit> m_units = new List<RosemaryUnit>();
		public IReadOnlyList<RosemaryUnit> units => m_units;
		public IEnumerable<RosemaryUnit> controllableUnits => units.Where(x => x && x.faction == faction);

		public int Count => m_units.Count;



		public void Clear()
		{
			foreach (RosemaryUnit item in m_units)
			{
				item.OnDeselected();
			}
			m_units.Clear();
		}

		public bool Add(RosemaryUnit item)
		{
			if (m_units.Contains(item)) { return false; }

			m_units.Add(item);
			item.OnSelected(this);
			return true;
		}

		public bool Contains(RosemaryUnit item)
		{
			return m_units.Contains(item);
		}

		public bool Remove(RosemaryUnit item)
		{
			bool r = m_units.Remove(item);
			if (r && item) { item.OnDeselected(); }
			return r;
		}

		public void RemoveAll(System.Predicate<RosemaryUnit> match)
		{
			int i = 0;
			while (i < m_units.Count)
			{
				var item = m_units[i];
				if (match(item))
				{
					item.OnDeselected();
					m_units.RemoveAt(i);
				}
				else
				{
					++i;
				}
			}
		}



		public void CommandStop()
		{
			var units = controllableUnits;

			foreach (var unit in units)
			{
				unit.CommandStop();
			}
		}

		public void CommandMove(Vector3 destination, Vector3 effectPoint)
		{
			var units = controllableUnits;

			foreach (var unit in units)
			{
				unit.CommandMove(destination);
			}

			if (units.Any())
			{
				moveCommandEffectSpawner.Spawn(effectPoint, Quaternion.identity, TargetPoint.None);
			}
		}

		public void CommandFollow(RosemaryUnit other)
		{
			var units = controllableUnits;

			foreach (var unit in units)
			{
				unit.CommandFollow(other);
			}

			if (units.Any())
			{
				followCommandEffectSpawner.Spawn(
					other.centre, Quaternion.identity,
					TargetPoint.Transform(other.centreTransform));
			}
		}

		public void CommandAttack(RosemaryUnit other)
		{
			var units = controllableUnits;

			foreach (var unit in units)
			{
				unit.CommandAttack(other);
			}

			if (units.Any())
			{
				attackCommandEffectSpawner.Spawn(
					other.centre, Quaternion.identity,
					TargetPoint.Transform(other.centreTransform));
			}
		}

		public void CommandDestroy()
		{
			var units = controllableUnits;

			foreach (var unit in units.ToArray())
			{
				unit.SelfDestruct();
			}
		}

		public void CommandProduceUnit(RosemaryProduct product)
		{
			var units = controllableUnits.Where(u => u.canProduce);

			foreach (var unit in units)
			{
				if (unit.canProduce)
				{
					unit.CommandProduceUnit(product);
				}
			}
		}
	}
}
