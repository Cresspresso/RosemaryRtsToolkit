using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Cresspresso.Rosemary
{
	/// <summary>
	///		<para>
	///			An entity in the game world which can be selected and commanded.
	///		</para>
	/// </summary>
	/// <changelog>
	///		<log date="01/05/2020"
	///			version="1.0"
	///			author="Elijah Shadbolt">
	///			Initial version.
	///		</log>
	/// </changelog>
	[RequireComponent(typeof(RosemarySpawn))]
	public partial class RosemaryUnit : MonoBehaviour
	{
		private RosemarySpawn m_spawn;
		public RosemarySpawn spawn {
			get
			{
				PrepareSpawn();
				return m_spawn;
			}
		}
		private void PrepareSpawn()
		{
			if (!m_spawn)
			{
				m_spawn = GetComponent<RosemarySpawn>();
				if (!m_spawn)
				{
					Debug.LogError($"{nameof(RosemarySpawn)} component not found on GameObject.", this);
				}
			}
		}

		public RosemaryFaction faction;

		[SerializeField]
		private Transform m_centreTransform;
		/// <summary>
		/// Guaranteed not null.
		/// </summary>
		public Transform centreTransform {
			get
			{
				if (!m_centreTransform)
				{
					m_centreTransform = transform;
				}
				return m_centreTransform;
			}
			set => m_centreTransform = value;
		}
		public Vector3 centre => centreTransform.position;
		public float centreRadius = 1.0f;





		private static HashSet<RosemaryUnit> m_instances = new HashSet<RosemaryUnit>();
		public static IEnumerable<RosemaryUnit> instances => m_instances;






		private void OnEnable()
		{
			m_instances.Add(this);

			if (!faction || spawn.spawner.spawnMode != SpawnMode.PreExisting)
			{
				faction = ((RosemaryUnitSpawner)spawn.spawner).faction;
			}

			OnEnableSelectionPartial();
			OnEnableMovementPartial();
			OnEnableHealthPartial();
			OnEnableFactoryPartial();
		}

		private void OnDisable()
		{
			m_instances.Remove(this);
			OnDisableSelectionPartial();
			OnDisableMovementPartial();
		}

#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(centre, centreRadius);

			OnDrawGizmosSelectedMovementPartial();
		}
#endif
	}
}
